using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using App_Hiker.Service;

namespace App_Hiker.View.Auth;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    private async void btn_register_Clicked(object sender, EventArgs e)
    {
        try
        {
            Model.User.Request.CreateUser user = new Model.User.Request.CreateUser()
            {
                nome_usuario = txt_nome_completo.Text.Replace(" ", "_"),
                nome_exibicao = txt_nome_completo.Text,
                email = txt_email.Text,
                senha = txt_senha.Text,
                numero_celular = txt_numero_celular.Text,
                data_nascimento = dtpck_data_nascimento.Date
            };

            Model.Api<Model.User.Response.CreateUser> api_response = await new Service.User.User().Create(user);

            if (api_response.status_code < 200 && api_response.status_code >= 300)
            {
                throw new Exception("Ocorreu um erro ao tentar criar uma conta!");
            }

            await DisplayAlertAsync("Sucesso!", "Sua conta do aplicativo foi criada com sucesso.", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
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