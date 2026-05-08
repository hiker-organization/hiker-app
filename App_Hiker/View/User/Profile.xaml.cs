namespace App_Hiker.View.User;

public partial class Profile : ContentView
{
    private Application? current_app = (Application?)App.Current;

    private enum InternalTabs
    {
        Posts,
        TimeLine,
        Favorites
    };

    private int current_profile_tab_index = 0;

	public Profile()
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
                foreach (IView tab in grid_profile_tabs.Children)
                {
                    if (tab is Button button)
                    {
                        if (Grid.GetColumn(button) == current_profile_tab_index)
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
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private async void LoadTab()
    {
        try
        {
            InternalTabs tab_option = (InternalTabs)this.current_profile_tab_index;

            switch (tab_option)
            {
                case InternalTabs.Posts:
                    ctv_profile_current_tab.Content = new Label() { Text = "Aba 01 (Perfil)" };
                break;

                case InternalTabs.TimeLine:
                    ctv_profile_current_tab.Content = new Label() { Text = "Aba 02 (Perfil)" };
                break;

                case InternalTabs.Favorites:
                    ctv_profile_current_tab.Content = new Label() { Text = "Aba 03 (Perfil)" };
                break;
            }

            ApplyTabsStyles();
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

            this.current_profile_tab_index = Grid.GetColumn(selected_tab);

            LoadTab();

        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }
}