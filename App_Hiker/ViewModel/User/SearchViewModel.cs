using System.Collections.ObjectModel;
using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Response;

using App_Hiker.Service.Review;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.User
{
    public class SearchViewModel : BaseViewModel
    {
        private string _searchText = String.Empty;
        private bool _hasSearched = false;

        public ObservableCollection<UserReview> Reviews { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        // Controla a mensagem de estado vazio (só aparece após uma busca).
        public bool HasSearched
        {
            get => _hasSearched;
            set => SetProperty(ref _hasSearched, value);
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel()
        {
            SearchCommand = new Command(async () => await SearchAsync());
        }

        private async Task SearchAsync()
        {
            try
            {
                string term = SearchText?.Trim() ?? String.Empty;

                if (string.IsNullOrEmpty(term))
                {
                    return;
                }

                IsBusy = true;

                DataResponse<FeedResponse> response = await ReviewService.Search(term);

                Reviews.Clear();

                foreach (UserReview review in response.data?.reviews ?? new List<UserReview>())
                {
                    Reviews.Add(review);
                }

                HasSearched = true;
            }
            catch (Exception ex)
            {
                App.ShowInDebugConsole(ex.Message); // Temporário.
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
