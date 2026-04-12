namespace App_Hiker.View.Auth;

public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();
	}

    private void ToggleValidationFieldsStatus()
    {
        bool email_field_is_disable = !txt_email.IsEnabled;

        txt_email.IsEnabled = email_field_is_disable;

        txt_token_validacao.IsEnabled = !email_field_is_disable;

        txt_nova_senha.IsEnabled = !email_field_is_disable;
    }

    private void btn_alterar_senha_Clicked(object sender, EventArgs e)
    {
        ToggleValidationFieldsStatus();
    }

    private async void btn_cancelar_Clicked(object sender, EventArgs e)
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