using App_Hiker.Model.Api;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;
using App_Hiker.Service.Auth;
using App_Hiker.Service.User;

namespace App_Hiker.View.User;

public partial class EditProfile : ContentView
{
    public event EventHandler? BackRequested;

    private UserDataResponse? user_data = new UserDataResponse();

    public EditProfile()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            InitializeResources();
        }
    }

    private async void InitializeResources()
    {
        try
        {
            await LoadUserData();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void SetUserData(UserDataResponse data)
    {
        try
        {
            entry_nome_exibicao.Text = data.nome_exibicao;

            if (data.foto_url != null)
                img_user_photo.Source = data.foto_url;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private async Task LoadUserData()
    {
        try
        {
            DataResponse<UserDataResponse> response = await AuthService.Me();

            this.user_data = response.data;

            if (response.data != null)
            {
                SetUserData(response.data);
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private async void btn_change_photo_Clicked(object sender, EventArgs e)
    {
        try
        {
            var options = new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            };

            FileResult? result = await FilePicker.PickAsync(options);

            if (result == null) return;

            // Mostra a imagem localmente de imediato (feedback visual instantâneo)
            //Stream preview_stream = await result.OpenReadAsync();
            //img_user_photo.Source = ImageSource.FromStream(() => preview_stream);

            // Faz upload para o backend e atualiza com a URL definitiva
            DataResponse<UpdateUserResponse> response = await UserService.UpdatePhoto(result);

            if (response.statusCode == 200 && response.data?.foto_url != null)
            {
                img_user_photo.Source = response.data.foto_url;
                await ShowAlert("Sucesso!", "Foto de perfil atualizada com sucesso.", "OK");
            }
            else
            {
                await ShowAlert("Erro", "Não foi possível atualizar a foto de perfil.", "OK");
            }
        }
        catch (PermissionException)
        {
            await ShowAlert("Permissão negada", "Permita o acesso à galeria nas configurações.", "OK");
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
            await ShowAlert("Erro", "Erro ao alterar a foto de perfil", "OK");
        }
    }

    private async void btn_save_Clicked(object sender, EventArgs e)
    {
        try
        {
            string nome_exibicao = entry_nome_exibicao.Text?.Trim() ?? String.Empty;

            if (string.IsNullOrWhiteSpace(nome_exibicao))
            {
                await ShowAlert("Campo obrigatório", "O nome de exibição não pode estar vazio.", "OK");
                return;
            }

            UpdateUserRequest payload = new UpdateUserRequest
            {
                nome_exibicao = nome_exibicao,
            };

            DataResponse<UpdateUserResponse> response = await UserService.Update(payload);

            if (response.statusCode == 200)
            {
                await ShowAlert("Sucesso!", "Perfil atualizado com sucesso.", "OK");
                BackRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                await ShowAlert("Erro", "Não foi possível atualizar o perfil.", "OK");
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void btn_back_Clicked(object sender, EventArgs e)
    {
        try
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private async Task ShowAlert(string title, string message, string button)
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page != null)
            await page.DisplayAlertAsync(title, message, button);
        else
            System.Diagnostics.Debug.WriteLine($"Não foi possível exibir alerta: nenhuma Window ativa. {title} - {message}");
    }
}
