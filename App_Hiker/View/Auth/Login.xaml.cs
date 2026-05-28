using App_Hiker.Model.Api;
using App_Hiker.Model.Auth.Exception;
using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;

using System.IdentityModel.Tokens.Jwt;

namespace App_Hiker.View.Auth;

public partial class Login : ContentPage
{
    private bool authentication_was_verified = false;

	public Login()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            string? token = await SecureStorage.Default.GetAsync("token");

            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

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

                await Shell.Current.GoToAsync("//Home");

                this.authentication_was_verified = true;
            }
        }
        catch (AuthenticationFailedException)
        {
            this.authentication_was_verified = true;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        string originalText = btn_login?.Text ?? "Entrar";
        try
        {
            // show loading
            btn_login.IsEnabled = false;
            btn_login.Text = string.Empty;
            if (btn_login_indicator != null)
            {
                btn_login_indicator.IsVisible = true;
                btn_login_indicator.IsRunning = true;
            }

            LoginRequest payload = new LoginRequest
            {
                email = txt_email.Text,
                password = txt_senha.Text
            };

            LoginResponse api_response = await AuthService.Login(payload);

            if (api_response.statusCode == 200 && api_response.access_token != String.Empty)
            {
                await SecureStorage.SetAsync("token", api_response.access_token);

                await Shell.Current.GoToAsync("//Home");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
        finally
        {
            // restore UI if still on this page
            try
            {
                btn_login.IsEnabled = true;
                btn_login.Text = originalText;
                if (btn_login_indicator != null)
                {
                    btn_login_indicator.IsRunning = false;
                    btn_login_indicator.IsVisible = false;
                }
            }
            catch { /* ignore restore errors */ }
        }
    }

    private async void btn_register_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Register());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void btn_alterar_senha_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ForgotPassword());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }
}