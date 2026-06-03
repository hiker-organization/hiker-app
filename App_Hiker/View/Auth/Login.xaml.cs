using App_Hiker.ViewModel.Auth;

namespace App_Hiker.View.Auth;

public partial class Login : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public Login()
    {
        InitializeComponent();

        _viewModel = new LoginViewModel();

        _viewModel.LoginSucceeded += OnLoginSucceeded;
        _viewModel.RegisterRequested += OnRegisterRequested;
        _viewModel.ForgotPasswordRequested += OnForgotPasswordRequested;

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.CheckExistingSessionAsync();
    }

    private async void OnLoginSucceeded()
    {
        await Shell.Current.GoToAsync("//Home");
    }

    private async void OnRegisterRequested()
    {
        await Navigation.PushAsync(new Register());
    }

    private async void OnForgotPasswordRequested()
    {
        await Navigation.PushAsync(new ForgotPassword());
    }
}
