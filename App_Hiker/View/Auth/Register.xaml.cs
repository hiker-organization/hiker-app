using App_Hiker.Model.Api;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;
using App_Hiker.Model.User.Exception;

using App_Hiker.Service.User;

using App_Hiker.Utils;

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
            if (txt_senha.Text != txt_confirmacao_senha.Text)
            {
                throw new NonMatchingPasswordsException("As senhas passadas não batem! Tente novamente.");
            }

            if (SpecialCharacters.Verify(txt_usuario.Text, @"[^a-zA-Z0-9_-]"))
            {
                throw new InvalidUsernameException("Nome de usuário inválido! Caracteres permitidos: letras, números, underscore e hífen.");
            }

            CreateUserRequest user = new CreateUserRequest()
            {
                nome_usuario = txt_usuario.Text,
                nome_exibicao = txt_nome_completo.Text,
                email = txt_email.Text,
                senha = txt_senha.Text,
                numero_celular = txt_numero_celular.Text,
                data_nascimento = dtpck_data_nascimento.Date
            };

            DataResponse<CreateUserResponse> api_response = await UserService.Create(user);

            if (api_response.statusCode == 201)
            {
                await DisplayAlertAsync("Sucesso!", "Sua conta do aplicativo foi criada com sucesso.", "OK");

                await Navigation.PopAsync();
            }
        }
        catch (InvalidUsernameException ex)
        {
            await DisplayAlertAsync("Atenção!", ex.Message, "OK");
        }
        catch (NonMatchingPasswordsException ex)
        {
            await DisplayAlertAsync("Atenção!", ex.Message, "OK");
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