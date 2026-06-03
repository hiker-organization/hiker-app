using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class EditProfile : ContentView
{
    private readonly EditProfileViewModel _viewModel;

    public event EventHandler? BackRequested;

    public EditProfile()
    {
        InitializeComponent();

        _viewModel = new EditProfileViewModel();

        _viewModel.BackRequested += () => BackRequested?.Invoke(this, EventArgs.Empty);

        BindingContext = _viewModel;
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            await _viewModel.LoadAsync();
        }
    }
}
