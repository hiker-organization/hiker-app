using System.ComponentModel;

using App_Hiker.View.Auth;
using App_Hiker.View.Review;
using App_Hiker.View.User;
using App_Hiker.ViewModel.Layout;

namespace App_Hiker.Layout;

public partial class MainTabBar : ContentPage
{
    private readonly MainTabBarViewModel _viewModel;

    public MainTabBar()
    {
        InitializeComponent();

        _viewModel = new MainTabBarViewModel();
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        BindingContext = _viewModel;

        LoadTab();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainTabBarViewModel.CurrentTabIndex))
        {
            LoadTab();
        }
    }

    // Composição/host de view: troca o conteúdo conforme a aba ativa.
    private void LoadTab()
    {
        try
        {
            switch (_viewModel.CurrentTabIndex)
            {
                case 0:
                    ShowFeed();
                    break;

                case 1:
                    ctview_page.Content = new NewReview("AuthUserContext");
                    break;

                case 2:
                    ShowSearch();
                    break;

                case 3:
                    Profile profile_view = new Profile("AuthUserContext");
                    profile_view.EditProfileRequested += OnEditProfileRequested;
                    profile_view.BackRequested += OnProfileBackRequested;
                    profile_view.LogoutRequested += OnLogoutRequested;
                    ctview_page.Content = profile_view;
                    break;
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void ShowFeed()
    {
        Feed feed_view = new Feed();
        feed_view.UserProfileRequested += OnFeedUserProfileRequested;
        ctview_page.Content = feed_view;
    }

    private void ShowSearch()
    {
        Search search_view = new Search();
        search_view.UserProfileRequested += OnSearchUserProfileRequested;
        ctview_page.Content = search_view;
    }

    private void OnFeedUserProfileRequested(string userNick)
    {
        try
        {
            PublicProfile public_profile_view = new PublicProfile(userNick);
            public_profile_view.BackRequested += OnPublicProfileBackRequested;
            ctview_page.Content = public_profile_view;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnPublicProfileBackRequested(object? sender, EventArgs e)
    {
        try
        {
            ShowFeed();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnSearchUserProfileRequested(object? sender, string userNick)
    {
        try
        {
            PublicProfile public_profile_view = new PublicProfile(userNick);
            public_profile_view.BackRequested += OnPublicProfileBackRequested;
            ctview_page.Content = public_profile_view;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnEditProfileRequested(object? sender, EventArgs e)
    {
        try
        {
            EditProfile edit_profile_view = new EditProfile();
            edit_profile_view.BackRequested += OnEditProfileBackRequested;
            ctview_page.Content = edit_profile_view;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnEditProfileBackRequested(object? sender, EventArgs e)
    {
        try
        {
            // Volta para o perfil recriando a view (recarrega os dados atualizados)
            Profile profile_view = new Profile("AuthUserContext");
            profile_view.EditProfileRequested += OnEditProfileRequested;
            profile_view.BackRequested += OnProfileBackRequested;
            profile_view.LogoutRequested += OnLogoutRequested;
            ctview_page.Content = profile_view;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnProfileBackRequested(object? sender, EventArgs e)
    {
        try
        {
            _viewModel.GoToPreviousTab();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void OnLogoutRequested(object? sender, EventArgs e)
    {
        try
        {
            // Navega para a tela de login limpando a pilha de navegação
            Application.Current!.Windows[0].Page = new NavigationPage(new Login());
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }
}
