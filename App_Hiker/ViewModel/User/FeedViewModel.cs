using System.Collections.ObjectModel;
using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Response;

using App_Hiker.Service.Review;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class FeedViewModel : BaseViewModel
    {
        public ObservableCollection<UserReview> Reviews { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand LikeCommand { get; }
        public ICommand DislikeCommand { get; }

        public FeedViewModel()
        {
            LoadCommand = new Command(async () => await LoadFeedAsync());
            LikeCommand = new Command<UserReview>(async (review) => await LikeAsync(review));
            DislikeCommand = new Command<UserReview>(async (review) => await DislikeAsync(review));
        }

        private async Task LoadFeedAsync()
        {
            try
            {
                DataResponse<FeedResponse> response = await ReviewService.GetFeed();

                Reviews.Clear();

                foreach (UserReview review in response.data?.reviews ?? new List<UserReview>())
                {
                    Reviews.Add(review);
                }
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
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
                App.ShowInDebugConsole(ex.Message); // Temporário.
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
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
        }
    }
}
