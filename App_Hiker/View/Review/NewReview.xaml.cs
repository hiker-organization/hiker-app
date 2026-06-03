using App_Hiker.ViewModel.Review;

namespace App_Hiker.View.Review;

public partial class NewReview : ContentView
{
    private readonly NewReviewViewModel _viewModel;

    public NewReview(string context)
    {
        InitializeComponent();

        _viewModel = new NewReviewViewModel(context);

        BindingContext = _viewModel;

        _ = _viewModel.LoadUserDataAsync();
    }
}
