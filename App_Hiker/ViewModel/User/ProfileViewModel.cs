using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Response;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.Review;

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
        public ICommand DeleteReviewCommand { get; }

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
            DeleteReviewCommand = new Command<UserReview>(async (review) => await DeleteReviewAsync(review));
        }

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;

                DataResponse<UserDataResponse> response = new DataResponse<UserDataResponse>();

                if (_context == "AuthUserContext")
                {
                    response = await AuthService.Me();
                }

                _userData = response.data;

                if (response.data != null)
                {
                    response.data.reviews = response.data.reviews
                        .OrderByDescending(review => review.createdAt)
                        .ToList();

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
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteReviewAsync(UserReview? review)
        {
            if (review == null) return;

            bool confirmed = await DisplayAlert("Excluir review", "Deseja excluir esta review?", "Excluir", "Cancelar");
            if (!confirmed) return;

            try
            {
                await ReviewService.Delete(review.id);
                await LoadAsync();
                OnPropertyChanged(nameof(CurrentTabIndex));
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
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
                await HandleApiErrorAsync(ex);
            }
        }
    }
}
