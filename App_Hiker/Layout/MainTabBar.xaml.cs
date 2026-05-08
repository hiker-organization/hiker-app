using App_Hiker.View.User;

namespace App_Hiker.Layout;

public partial class MainTabBar : ContentPage
{
	private Application? current_app = (Application?)App.Current;

	private enum InternalTabs
	{
		Home,
		NewPost,
		Profile
	};

	private int current_main_tab_index = 0;

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
                        if (Grid.GetColumn(button) == current_main_tab_index)
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
            InternalTabs tab_option = (InternalTabs)this.current_main_tab_index;

            switch (tab_option)
            {
                case InternalTabs.Home:
                    ctview_page.Content = new Label() { Text = "Aba 01" };
                break;

                case InternalTabs.NewPost:
                    ctview_page.Content = new Label() { Text = "Aba 02" };
                    break;

                case InternalTabs.Profile:
					ctview_page.Content = new Profile();
                break;
            }

			ApplyTabsStyles();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro!", ex.Message, "OK");
        }
    }

    private async void tab_Clicked(object sender, EventArgs e)
    {
		try
		{
			Button selected_tab = (Button)sender;

			this.current_main_tab_index = Grid.GetColumn(selected_tab);

			LoadTab();

        }
		catch (Exception ex)
		{
			await DisplayAlertAsync("Erro!", ex.Message, "OK");
		}
    }
}