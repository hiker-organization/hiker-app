using System.Collections.ObjectModel;
using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.Review;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class FeedViewModel : BaseViewModel
    {
        private const int PageSize = 10;

        public ObservableCollection<UserReview> Reviews { get; } = new();

        private int? _nextCursor = null;
        private bool _hasMoreReviews = true;
        private bool _isLoadingMore = false;
        private bool _hasLoadedInitialReviews = false;
        private string _currentUserNick = string.Empty;

        public bool HasMoreReviews
        {
            get => _hasMoreReviews;
            private set => SetProperty(ref _hasMoreReviews, value);
        }

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            private set => SetProperty(ref _isLoadingMore, value);
        }

        public bool HasLoadedInitialReviews
        {
            get => _hasLoadedInitialReviews;
            private set => SetProperty(ref _hasLoadedInitialReviews, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand LikeCommand { get; }
        public ICommand DislikeCommand { get; }
        public ICommand DeleteReviewCommand { get; }
        public ICommand OpenProfileCommand { get; }

        public event Action<string>? UserProfileRequested;

        public FeedViewModel()
        {
            LoadCommand = new Command(async () => await LoadFeedAsync());
            LoadMoreCommand = new Command(async () => await LoadMoreAsync());
            LikeCommand = new Command<UserReview>(async (review) => await LikeAsync(review));
            DislikeCommand = new Command<UserReview>(async (review) => await DislikeAsync(review));
            DeleteReviewCommand = new Command<UserReview>(async (review) => await DeleteReviewAsync(review));
            OpenProfileCommand = new Command<string>(OnOpenProfile);
        }

        private async Task LoadFeedAsync()
        {
            await LoadFeedPageAsync(reset: true);
        }

        private async Task LoadMoreAsync()
        {
            await LoadFeedPageAsync(reset: false);
        }

        private async Task LoadFeedPageAsync(bool reset)
        {
            try
            {
                if (IsLoadingMore || (!reset && !HasMoreReviews))
                {
                    return;
                }

                if (reset)
                {
                    IsBusy = true;
                }

                IsLoadingMore = true;

                if (reset)
                {
                    var me = await AuthService.Me();
                    _currentUserNick = me.data?.nome_usuario ?? string.Empty;
                }

                DataResponse<FeedResponse> response = await ReviewService.GetFeed(
                    reset ? null : _nextCursor,
                    PageSize);

                List<UserReview> reviews = response.data?.reviews ?? new List<UserReview>();

                if (reset)
                {
                    Reviews.Clear();
                }

                foreach (UserReview review in reviews)
                {
                    review.IsOwnReview = review.autor.nome_usuario == _currentUserNick;
                    Reviews.Add(review);
                }

                _nextCursor = response.data?.nextCursor;
                HasMoreReviews = _nextCursor.HasValue && reviews.Count > 0;

                if (reset)
                {
                    HasLoadedInitialReviews = true;
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsLoadingMore = false;
                if (reset)
                {
                    IsBusy = false;
                }
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

        private async Task DeleteReviewAsync(UserReview? review)
        {
            if (review == null) return;

            bool confirmed = await DisplayAlert("Excluir review", "Deseja excluir esta review?", "Excluir", "Cancelar");
            if (!confirmed) return;

            try
            {
                await ReviewService.Delete(review.id);
                Reviews.Remove(review);
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
        }

        private void OnOpenProfile(string? userNick)
        {
            if (string.IsNullOrWhiteSpace(userNick))
            {
                return;
            }

            UserProfileRequested?.Invoke(userNick.Trim());
        }
    }
}
