using App_Hiker.ViewModel.Auth;

namespace App_Hiker.View.Auth;

public partial class Register : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public Register()
    {
        InitializeComponent();

        _viewModel = new RegisterViewModel();

        _viewModel.RegistrationSucceeded += OnNavigateBack;
        _viewModel.BackRequested += OnNavigateBack;

        BindingContext = _viewModel;
    }

    private async void OnNavigateBack()
    {
        await Navigation.PopAsync();
    }
}
