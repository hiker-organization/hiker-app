namespace App_Hiker.View.Auth;

public partial class ForgotPassword : ContentPage
{
    private enum ResetPasswordStats
    {
        Unready = 0,
        Ready = 1
    }

    private ResetPasswordStats reset_password_stats = ResetPasswordStats.Unready;

	public ForgotPassword()
	{
		InitializeComponent();
	}

    private async void ToggleValidationFieldsStatus()
    {
        try
        {
            this.reset_password_stats = (this.reset_password_stats == ResetPasswordStats.Unready) ? ResetPasswordStats.Ready : ResetPasswordStats.Unready;

            switch (this.reset_password_stats)
            {
                case ResetPasswordStats.Unready:
                    txt_email.IsEnabled = true;

                    txt_token_validacao.IsEnabled = false;

                    txt_nova_senha.IsEnabled = false;

                    btn_alterar_senha.Text = "Prosseguir";
                break;

                case ResetPasswordStats.Ready:
                    txt_email.IsEnabled = false;

                    txt_token_validacao.IsEnabled = true;

                    txt_nova_senha.IsEnabled = true;

                    btn_alterar_senha.Text = "Enviar";
                break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void SendEmailToResetPassword()
    {
        try
        {
            string email = txt_email.Text;

            Model.MessageResponse api_response = await new Service.Auth.Auth().ForgotPassword(email);

            await DisplayAlertAsync("Atenção!", api_response.message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void ResetUserPassword()
    {
        try
        {
            Model.Auth.Request.ResetPassword payload = new Model.Auth.Request.ResetPassword()
            {
                token = txt_token_validacao.Text,
                senha = txt_nova_senha.Text
            };

            Model.MessageResponse api_response = await new Service.Auth.Auth().ResetPassword(payload);

            await DisplayAlertAsync("Atenção!", api_response.message, "OK");
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
            if (this.reset_password_stats == ResetPasswordStats.Unready)
            {
                SendEmailToResetPassword();
            }
            else
            {
                ResetUserPassword();
            }

            ToggleValidationFieldsStatus();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
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