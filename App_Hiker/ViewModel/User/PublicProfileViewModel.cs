using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Response;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.Review;
using App_Hiker.Service.User;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class PublicProfileViewModel : BaseViewModel
    {
        private readonly string _userNick;

        private UserDataResponse? _userData = new();

        private string _displayName = "Usuario";
        private string _userName = "@usuario";
        private string _postsQuantidade = "0";
        private string _reputacao = "0%";
        private string _fotoUrl = "profile.png";

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

        public UserDataResponse? UserData => _userData;

        public ICommand BackCommand { get; }
        public ICommand LikeCommand { get; }
        public ICommand DislikeCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public event Action? BackRequested;

        private string _currentUserNick = string.Empty;

        public PublicProfileViewModel(string userNick)
        {
            _userNick = userNick;
            BackCommand = new Command(() => BackRequested?.Invoke());
            LikeCommand = new Command<UserReview>(async (review) => await LikeAsync(review));
            DislikeCommand = new Command<UserReview>(async (review) => await DislikeAsync(review));
            DeleteReviewCommand = new Command<UserReview>(async (review) => await DeleteReviewAsync(review));
        }

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;

                var meResponse = await AuthService.Me();
                _currentUserNick = meResponse.data?.nome_usuario ?? string.Empty;

                DataResponse<UserDataResponse> response = await UserService.GetByNick(_userNick);

                _userData = response.data;

                if (response.data != null)
                {
                    response.data.reviews = response.data.reviews
                        .OrderByDescending(review => review.createdAt)
                        .ToList();

                    foreach (var review in response.data.reviews)
                        review.IsOwnReview = review.autor.nome_usuario == _currentUserNick;

                    DisplayName = response.data.nome_exibicao;
                    UserName = response.data.nome_usuario;
                    PostsQuantidade = response.data.reviews.Count.ToString();
                    Reputacao = response.data.reputacao_normalizada;
                    FotoUrl = string.IsNullOrWhiteSpace(response.data.foto_url)
                        ? "profile.png"
                        : response.data.foto_url;
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
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
        }

        private async Task LikeAsync(UserReview? review)
        {
            try
            {
                if (review == null || review.liked)
                {
                    return;
                }

                bool wasDisliked = review.disliked;

                await ReviewService.Like(review.id);

                review.liked = true;
                review.disliked = false;
                review.qnt_likes += 1;

                if (wasDisliked && review.qnt_dislikes > 0)
                {
                    review.qnt_dislikes -= 1;
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
        }

        private async Task DislikeAsync(UserReview? review)
        {
            try
            {
                if (review == null || review.disliked)
                {
                    return;
                }

                bool wasLiked = review.liked;

                await ReviewService.Dislike(review.id);

                review.disliked = true;
                review.liked = false;
                review.qnt_dislikes += 1;

                if (wasLiked && review.qnt_likes > 0)
                {
                    review.qnt_likes -= 1;
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
        }
    }
}
