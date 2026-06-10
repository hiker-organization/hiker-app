using System.ComponentModel;

using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class Profile : ContentView
{
    private readonly ProfileViewModel _viewModel;

    public event EventHandler? EditProfileRequested;
    public event EventHandler? BackRequested;
    public event EventHandler? LogoutRequested;

    public Profile(string context)
    {
        InitializeComponent();

        _viewModel = new ProfileViewModel(context);

        _viewModel.EditProfileRequested += () => EditProfileRequested?.Invoke(this, EventArgs.Empty);
        _viewModel.BackRequested += () => BackRequested?.Invoke(this, EventArgs.Empty);
        _viewModel.LogoutRequested += () => LogoutRequested?.Invoke(this, EventArgs.Empty);
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        BindingContext = _viewModel;
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            await _viewModel.LoadAsync();

            LoadTab();
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ProfileViewModel.CurrentTabIndex))
        {
            LoadTab();
        }
    }

    // Composição/host de view: troca o conteúdo da aba selecionada.
    private void LoadTab()
    {
        try
        {
            switch (_viewModel.CurrentTabIndex)
            {
                case 0:
                    ctv_profile_current_tab.Content = new UserReviewsListing
                    {
                        BindingContext = _viewModel.UserData,
                        DeleteCommand = _viewModel.DeleteReviewCommand
                    };
                    break;

                case 1:
                    ctv_profile_current_tab.Content = new Label { Text = "Aba 03 (Perfil)" };
                    break;

                case 2:
                    ctv_profile_current_tab.Content = new Label { Text = "Aba 04 (Perfil)" };
                    break;
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }
}
