using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

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
            Model.Auth.Request.LoginUser payload = new Model.Auth.Request.LoginUser
            {
                email = txt_email.Text,
                password = txt_senha.Text
            };

            Model.Auth.Response.LoginUser api_response = await new Service.Auth.Auth().Login(payload);

            if (api_response.access_token != String.Empty)
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