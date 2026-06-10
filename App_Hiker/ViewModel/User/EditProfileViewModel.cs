using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.User;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class EditProfileViewModel : BaseViewModel
    {
        private string _nomeExibicao = string.Empty;
        private string _fotoUrl = "logo.png";

        public string NomeExibicao
        {
            get => _nomeExibicao;
            set => SetProperty(ref _nomeExibicao, value);
        }

        public string FotoUrl
        {
            get => _fotoUrl;
            set => SetProperty(ref _fotoUrl, value);
        }

        public ICommand ChangePhotoCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        public event Action? BackRequested;

        public EditProfileViewModel()
        {
            ChangePhotoCommand = new Command(async () => await ChangePhotoAsync());
            SaveCommand = new Command(async () => await SaveAsync());
            BackCommand = new Command(() => BackRequested?.Invoke());
        }

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;

                DataResponse<UserDataResponse> response = await AuthService.Me();

                if (response.data != null)
                {
                    NomeExibicao = response.data.nome_exibicao;

                    if (!string.IsNullOrEmpty(response.data.foto_url))
                    {
                        FotoUrl = response.data.foto_url;
                    }
                }
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ChangePhotoAsync()
        {
            try
            {
                IsBusy = true;

                PickOptions options = new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                };

                FileResult? result = await FilePicker.PickAsync(options);

                if (result == null)
                {
                    return;
                }

                DataResponse<UpdateUserResponse> response = await UserService.UpdatePhoto(result);

                if (response.statusCode == 200 && response.data?.foto_url != null)
                {
                    FotoUrl = response.data.foto_url;

                    await DisplayAlert("Sucesso!", "Foto de perfil atualizada com sucesso.", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível atualizar a foto de perfil.", "OK");
                }
            }
            catch (PermissionException)
            {
                await DisplayAlert("Permissão negada", "Permita o acesso à galeria nas configurações.", "OK");
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.

                await DisplayAlert("Erro", "Erro ao alterar a foto de perfil", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                IsBusy = true;

                string nome_exibicao = NomeExibicao?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nome_exibicao))
                {
                    await DisplayAlert("Campo obrigatório", "O nome de exibição não pode estar vazio.", "OK");

                    return;
                }

                UpdateUserRequest payload = new UpdateUserRequest
                {
                    nome_exibicao = nome_exibicao
                };

                DataResponse<UpdateUserResponse> response = await UserService.Update(payload);

                if (response.statusCode == 200)
                {
                    await DisplayAlert("Sucesso!", "Perfil atualizado com sucesso.", "OK");

                    BackRequested?.Invoke();
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível atualizar o perfil.", "OK");
                }
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
