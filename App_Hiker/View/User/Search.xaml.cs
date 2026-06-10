using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class Search : ContentView
{
    private readonly SearchViewModel _viewModel;

    public event EventHandler<string>? UserProfileRequested;

    public Search()
    {
        InitializeComponent();

        _viewModel = new SearchViewModel();
        _viewModel.UserProfileRequested += OnUserProfileRequested;

        BindingContext = _viewModel;
    }

    private void OnUserProfileRequested(string nick)
    {
        UserProfileRequested?.Invoke(this, nick);
    }
}
