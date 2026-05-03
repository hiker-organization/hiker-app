using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

using App_Hiker.Service.Auth;

namespace App_Hiker.View.Auth;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        try
        {
            LoginRequest payload = new LoginRequest
            {
                email = txt_email.Text,
                password = txt_senha.Text
            };

            LoginResponse api_response = await AuthService.Login(payload);

            if (api_response.statusCode == 200 && api_response.access_token != String.Empty)
            {
                await SecureStorage.SetAsync("token", api_response.access_token);

                await DisplayAlertAsync("Sucesso!", "Seja bem vindo ao Hiker.", "OK");

                await Shell.Current.GoToAsync("//Profile");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
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