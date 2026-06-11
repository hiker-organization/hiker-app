using System.IdentityModel.Tokens.Jwt;
using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Auth.Exception;
using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        // Intenções de navegação tratadas pelo code-behind.
        public event Action? LoginSucceeded;
        public event Action? RegisterRequested;
        public event Action? ForgotPasswordRequested;

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            RegisterCommand = new Command(() => RegisterRequested?.Invoke());
            ForgotPasswordCommand = new Command(() => ForgotPasswordRequested?.Invoke());
        }

        public async Task CheckExistingSessionAsync()
        {
            try
            {
                IsBusy = true;

                string? token = await SecureStorage.Default.GetAsync("token");

                if (token == null)
                {
                    return;
                }

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = handler.ReadJwtToken(token);

                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    SecureStorage.Default.Remove("token");

                    return;
                }

                DataResponse<UserDataResponse> api_response = await AuthService.Me();

                if (api_response.statusCode == 401 || api_response.data == null)
                {
                    throw new AuthenticationFailedException();
                }

                LoginSucceeded?.Invoke();
            }
            catch (AuthenticationFailedException)
            {
                // Sessão inválida: permanece na tela de login.
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoginAsync()
        {
            try
            {
                IsBusy = true;
                ((Command)LoginCommand).ChangeCanExecute();

                LoginRequest payload = new LoginRequest
                {
                    email = Email,
                    password = Password
                };

                LoginResponse api_response = await AuthService.Login(payload);

                if (api_response.statusCode == 200 && api_response.access_token != string.Empty)
                {
                    await SecureStorage.SetAsync("token", api_response.access_token);

                    LoginSucceeded?.Invoke();
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }
}
