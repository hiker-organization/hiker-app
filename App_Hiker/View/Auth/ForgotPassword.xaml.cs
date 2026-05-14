using App_Hiker.Model.Api;
using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

using App_Hiker.Service.Auth;

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

                    txt_codigo_validacao.IsEnabled = false;

                    txt_nova_senha.IsEnabled = false;

                    btn_alterar_senha.Text = "Prosseguir";
                break;

                case ResetPasswordStats.Ready:
                    txt_email.IsEnabled = false;

                    txt_codigo_validacao.IsEnabled = true;

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

    private async Task SendEmailToResetPassword()
    {
        try
        {
            string email = txt_email.Text;

            MessageResponse api_response = await AuthService.ForgotPassword(email);

            await DisplayAlertAsync("Atenção!", "Uma mensagem de redefinição de senha foi enviada para o e-mail informado, caso ele exista.", "OK");

            ToggleValidationFieldsStatus();
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
            ResetPasswordRequest payload = new ResetPasswordRequest()
            {
                email = txt_email.Text,
                token = txt_codigo_validacao.Text,
                senha = txt_nova_senha.Text
            };

            MessageResponse api_response = await AuthService.ResetPassword(payload);

            await DisplayAlertAsync("Atenção!", "Sua senha foi alterada com sucesso! Efetue o login para prosseguir.", "OK");

            await Navigation.PopAsync();
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
                await SendEmailToResetPassword();
            }
            else
            {
                ResetUserPassword();
            }
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