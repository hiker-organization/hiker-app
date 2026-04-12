namespace App_Hiker.View.Auth;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void btn_login_Clicked(object sender, EventArgs e)
    {
        //
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