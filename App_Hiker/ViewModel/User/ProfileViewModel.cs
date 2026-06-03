using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly string _context;

        private UserDataResponse? _userData = new();

        private string _displayName = "Usuário";
        private string _userName = "@Usuário";
        private string _postsQuantidade = "0";
        private string _reputacao = "0%";
        private string _fotoUrl = "profile.png";
        private int _currentTabIndex = 0;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string PostsQuantidade
        {
            get => _postsQuantidade;
            set => SetProperty(ref _postsQuantidade, value);
        }

        public string Reputacao
        {
            get => _reputacao;
            set => SetProperty(ref _reputacao, value);
        }

        public string FotoUrl
        {
            get => _fotoUrl;
            set => SetProperty(ref _fotoUrl, value);
        }

        public int CurrentTabIndex
        {
            get => _currentTabIndex;
            set => SetProperty(ref _currentTabIndex, value);
        }

        public UserDataResponse? UserData => _userData;

        public ICommand SelectTabCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand LogoutCommand { get; }

        public event Action? EditProfileRequested;
        public event Action? BackRequested;
        public event Action? LogoutRequested;

        public ProfileViewModel(string context)
        {
            _context = context;

            SelectTabCommand = new Command<string>(OnSelectTab);
            EditProfileCommand = new Command(() => EditProfileRequested?.Invoke());
            BackCommand = new Command(() => BackRequested?.Invoke());
            LogoutCommand = new Command(async () => await LogoutAsync());
        }

        public async Task LoadAsync()
        {
            try
            {
                DataResponse<UserDataResponse> response = new DataResponse<UserDataResponse>();

                if (_context == "AuthUserContext")
                {
                    response = await AuthService.Me();
                }

                _userData = response.data;

                if (response.data != null)
                {
                    DisplayName = response.data.nome_exibicao;
                    UserName = response.data.nome_usuario;
                    PostsQuantidade = response.data.reviews.Count.ToString();
                    Reputacao = response.data.reputacao_normalizada;
                    FotoUrl = response.data.foto_url;
                }

                OnPropertyChanged(nameof(UserData));
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
        }

        private void OnSelectTab(string? rawIndex)
        {
            if (int.TryParse(rawIndex, out int index))
            {
                CurrentTabIndex = index;
            }
        }

        private async Task LogoutAsync()
        {
            try
            {
                bool confirmed = await DisplayAlert("Sair", "Tem certeza que deseja sair?", "Sair", "Cancelar");

                if (!confirmed)
                {
                    return;
                }

                SecureStorage.Remove("token");

                LogoutRequested?.Invoke();
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
        }
    }
}
