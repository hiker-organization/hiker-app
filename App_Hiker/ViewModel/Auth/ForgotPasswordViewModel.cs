using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Auth.Request;

using App_Hiker.Service.Auth;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.Auth
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private enum ResetPasswordStats
        {
            Unready = 0,
            Ready = 1
        }

        private ResetPasswordStats _stats = ResetPasswordStats.Unready;

        private string _email = string.Empty;
        private string _codigoValidacao = string.Empty;
        private string _novaSenha = string.Empty;

        private bool _emailEnabled = true;
        private bool _codigoEnabled = false;
        private bool _novaSenhaEnabled = false;
        private string _botaoTexto = "Prosseguir";

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string CodigoValidacao
        {
            get => _codigoValidacao;
            set => SetProperty(ref _codigoValidacao, value);
        }

        public string NovaSenha
        {
            get => _novaSenha;
            set => SetProperty(ref _novaSenha, value);
        }

        public bool EmailEnabled
        {
            get => _emailEnabled;
            set => SetProperty(ref _emailEnabled, value);
        }

        public bool CodigoEnabled
        {
            get => _codigoEnabled;
            set => SetProperty(ref _codigoEnabled, value);
        }

        public bool NovaSenhaEnabled
        {
            get => _novaSenhaEnabled;
            set => SetProperty(ref _novaSenhaEnabled, value);
        }

        public string BotaoTexto
        {
            get => _botaoTexto;
            set => SetProperty(ref _botaoTexto, value);
        }

        public ICommand AlterarSenhaCommand { get; }
        public ICommand CancelarCommand { get; }

        public event Action? BackRequested;

        public ForgotPasswordViewModel()
        {
            AlterarSenhaCommand = new Command(async () => await AlterarSenhaAsync());
            CancelarCommand = new Command(() => BackRequested?.Invoke());
        }

        private void ToggleValidationFieldsStatus()
        {
            _stats = _stats == ResetPasswordStats.Unready
                ? ResetPasswordStats.Ready
                : ResetPasswordStats.Unready;

            switch (_stats)
            {
                case ResetPasswordStats.Unready:
                    EmailEnabled = true;
                    CodigoEnabled = false;
                    NovaSenhaEnabled = false;
                    BotaoTexto = "Prosseguir";
                    break;

                case ResetPasswordStats.Ready:
                    EmailEnabled = false;
                    CodigoEnabled = true;
                    NovaSenhaEnabled = true;
                    BotaoTexto = "Enviar";
                    break;
            }
        }

        private async Task AlterarSenhaAsync()
        {
            try
            {
                if (_stats == ResetPasswordStats.Unready)
                {
                    await SendEmailToResetPasswordAsync();
                }
                else
                {
                    await ResetUserPasswordAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK");
            }
        }

        private async Task SendEmailToResetPasswordAsync()
        {
            try
            {
                MessageResponse api_response = await AuthService.ForgotPassword(Email);

                await DisplayAlert("Atenção!", "Uma mensagem de redefinição de senha foi enviada para o e-mail informado, caso ele exista.", "OK");

                ToggleValidationFieldsStatus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK");
            }
        }

        private async Task ResetUserPasswordAsync()
        {
            try
            {
                ResetPasswordRequest payload = new ResetPasswordRequest
                {
                    email = Email,
                    token = CodigoValidacao,
                    senha = NovaSenha
                };

                MessageResponse api_response = await AuthService.ResetPassword(payload);

                await DisplayAlert("Atenção!", "Sua senha foi alterada com sucesso! Efetue o login para prosseguir.", "OK");

                BackRequested?.Invoke();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK");
            }
        }
    }
}
