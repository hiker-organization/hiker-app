using App_Hiker.ViewModel.Auth;

namespace App_Hiker.View.Auth;

public partial class ForgotPassword : ContentPage
{
    private readonly ForgotPasswordViewModel _viewModel;

    public ForgotPassword()
    {
        InitializeComponent();

        _viewModel = new ForgotPasswordViewModel();

        _viewModel.BackRequested += OnBackRequested;

        BindingContext = _viewModel;
    }

    private async void OnBackRequested()
    {
        await Navigation.PopAsync();
    }
}
