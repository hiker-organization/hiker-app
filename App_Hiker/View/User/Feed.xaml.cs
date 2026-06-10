using System.Windows.Input;

using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class Feed : ContentView
{
    private readonly FeedViewModel _viewModel;
    private bool _initialized = false;

    public event Action<string>? UserProfileRequested;

    public Feed()
    {
        InitializeComponent();

        _viewModel = new FeedViewModel();
        _viewModel.UserProfileRequested += OnUserProfileRequested;

        BindingContext = _viewModel;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null && !_initialized)
        {
            _initialized = true;

            if (_viewModel.LoadCommand is ICommand load && load.CanExecute(null))
            {
                load.Execute(null);
            }
        }
    }

    private void OnRemainingItemsThresholdReached(object? sender, EventArgs e)
    {
        if (_viewModel.HasMoreReviews && !_viewModel.IsLoadingMore && _viewModel.LoadMoreCommand.CanExecute(null))
        {
            _viewModel.LoadMoreCommand.Execute(null);
        }
    }

    private void OnUserProfileRequested(string nick)
    {
        UserProfileRequested?.Invoke(nick);
    }
}
