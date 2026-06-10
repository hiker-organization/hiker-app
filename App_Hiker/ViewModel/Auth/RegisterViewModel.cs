using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.User.Exception;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.User;

using App_Hiker.Utils;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.Auth
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _nomeUsuario = string.Empty;
        private string _nomeCompleto = string.Empty;
        private string _email = string.Empty;
        private string _numeroCelular = string.Empty;
        private DateTime _dataNascimento = DateTime.Today;
        private string _senha = string.Empty;
        private string _confirmacaoSenha = string.Empty;

        public string NomeUsuario
        {
            get => _nomeUsuario;
            set => SetProperty(ref _nomeUsuario, value);
        }

        public string NomeCompleto
        {
            get => _nomeCompleto;
            set => SetProperty(ref _nomeCompleto, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string NumeroCelular
        {
            get => _numeroCelular;
            set => SetProperty(ref _numeroCelular, value);
        }

        public DateTime DataNascimento
        {
            get => _dataNascimento;
            set => SetProperty(ref _dataNascimento, value);
        }

        public string Senha
        {
            get => _senha;
            set => SetProperty(ref _senha, value);
        }

        public string ConfirmacaoSenha
        {
            get => _confirmacaoSenha;
            set => SetProperty(ref _confirmacaoSenha, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand BackToLoginCommand { get; }

        public event Action? RegistrationSucceeded;
        public event Action? BackRequested;

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterAsync());
            BackToLoginCommand = new Command(() => BackRequested?.Invoke());
        }

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;

                if (Senha != ConfirmacaoSenha)
                {
                    throw new NonMatchingPasswordsException("As senhas passadas não batem! Tente novamente.");
                }

                if (SpecialCharacters.Verify(NomeUsuario, @"[^a-zA-Z0-9_.]"))
                {
                    throw new InvalidUsernameException("Nome de usuário inválido! Caracteres permitidos: letras, números, underscore e ponto final.");
                }

                CreateUserRequest user = new CreateUserRequest
                {
                    nome_usuario = NomeUsuario,
                    nome_exibicao = NomeCompleto,
                    email = Email,
                    senha = Senha,
                    numero_celular = NumeroCelular,
                    data_nascimento = DataNascimento
                };

                DataResponse<CreateUserResponse> api_response = await UserService.Create(user);

                if (api_response.statusCode == 201)
                {
                    await DisplayAlert("Sucesso!", "Sua conta do aplicativo foi criada com sucesso.", "OK");

                    RegistrationSucceeded?.Invoke();
                }
            }
            catch (InvalidUsernameException ex)
            {
                await DisplayAlert("Atenção!", ex.Message, "OK");
            }
            catch (NonMatchingPasswordsException ex)
            {
                await DisplayAlert("Atenção!", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
