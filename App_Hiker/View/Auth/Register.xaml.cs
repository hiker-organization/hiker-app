namespace App_Hiker.View.Auth;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    private void btn_register_Clicked(object sender, EventArgs e)
    {
        //
    }

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }
}