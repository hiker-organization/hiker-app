using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class PublicProfile : ContentView
{
    private readonly PublicProfileViewModel _viewModel;
    private bool _initialized = false;

    public event EventHandler? BackRequested;

    public PublicProfile(string userNick)
    {
        InitializeComponent();

        _viewModel = new PublicProfileViewModel(userNick);
        _viewModel.BackRequested += () => BackRequested?.Invoke(this, EventArgs.Empty);

        BindingContext = _viewModel;
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null && !_initialized)
        {
            _initialized = true;
            await _viewModel.LoadAsync();
        }
    }
}
