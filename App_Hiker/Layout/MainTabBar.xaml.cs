using App_Hiker.View.Review;
using App_Hiker.View.User;
using App_Hiker.View.Auth;

namespace App_Hiker.Layout;

public partial class MainTabBar : ContentPage
{
    private Application? current_app = (Application?)App.Current;

    private enum InternalTabs
    {
        Home,
        NewReview,
        Search,
        Profile
    };

    private InternalTabs current_main_tab_index = InternalTabs.Home;
    private InternalTabs previous_main_tab_index = InternalTabs.Home;

    public MainTabBar()
    {
        InitializeComponent();

        LoadTab();
    }

    private async void ApplyTabsStyles()
    {
        try
        {
            if (current_app != null)
            {
                foreach (IView tab in grid_main_tabs.Children)
                {
                    if (tab is Button button)
                    {
                        int tabIndex = int.TryParse(button.ClassId, out int parsedIndex)
                            ? parsedIndex
                            : -1;

                        if (tabIndex == (int)current_main_tab_index)
                        {
                            button.TextColor = (Color)current_app.Resources["Primary"];
                        }
                        else
                        {
                            button.TextColor = (Color)current_app.Resources["BaseContent"];
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void LoadTab()
    {
        try
        {
            InternalTabs tab_option = this.current_main_tab_index;

            switch (tab_option)
            {
                case InternalTabs.Home:
                    ctview_page.Content = new Feed();
                    break;

                case InternalTabs.NewReview:
                    ctview_page.Content = new NewReview("AuthUserContext");
                    break;

                case InternalTabs.Search:
                    break;

                case InternalTabs.Profile:
                    Profile profile_view = new Profile("AuthUserContext");
                    profile_view.EditProfileRequested += OnEditProfileRequested;
                    profile_view.BackRequested += OnProfileBackRequested;
                    profile_view.LogoutRequested += OnLogoutRequested;
                    ctview_page.Content = profile_view;
                    break;
            }

            ApplyTabsStyles();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
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
            this.current_main_tab_index = this.previous_main_tab_index;
            LoadTab();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporario.
        }
    }

    private async void OnLogoutRequested(object? sender, EventArgs e)
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

    private async void tab_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button selected_tab = (Button)sender;
            if (!int.TryParse(selected_tab.ClassId, out int selectedIndex))
            {
                return;
            }

            InternalTabs nextTab = (InternalTabs)selectedIndex;

            if (nextTab == InternalTabs.Profile && this.current_main_tab_index != InternalTabs.Profile)
            {
                this.previous_main_tab_index = this.current_main_tab_index;
            }

            this.current_main_tab_index = nextTab;

            LoadTab();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }
}