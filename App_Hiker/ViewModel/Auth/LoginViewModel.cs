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
                ((Command)LoginCommand).ChangeCanExecute();

                string? accessToken = await SecureStorage.Default.GetAsync("access_token");
                if (accessToken == null)
                    return; // Sem sessão salva

                // AuthRefreshHandler renova automaticamente se o access_token expirou
                DataResponse<UserDataResponse> api_response = await AuthService.Me();

                if (api_response.statusCode == 401 || api_response.data == null)
                    throw new AuthenticationFailedException();

                LoginSucceeded?.Invoke();
            }
            catch (AuthenticationFailedException)
            {
                SecureStorage.Default.Remove("access_token");
                SecureStorage.Default.Remove("refresh_token");
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

                if (api_response.statusCode == 200 && !string.IsNullOrEmpty(api_response.access_token))
                {
                    await SecureStorage.Default.SetAsync("access_token", api_response.access_token);
                    await SecureStorage.Default.SetAsync("refresh_token", api_response.refresh_token);

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
