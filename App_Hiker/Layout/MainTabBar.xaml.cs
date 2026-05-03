namespace App_Hiker.Layout;

public partial class MainTabBar : ContentPage
{
	private enum InternalTabs
	{
		Home,
		New,
		Profile
	};

	private int current_tab_index = 0;

	public MainTabBar()
	{
		InitializeComponent();

		LoadTab();
	}

	private async void LoadTab()
	{
		try
		{
            InternalTabs tab_option = (InternalTabs)this.current_tab_index;

            switch (tab_option)
            {
                case InternalTabs.Home:
                    ctview_page.Content = new Label() { Text = "Aba 01" };
                break;

                case InternalTabs.New:
                    ctview_page.Content = new Label() { Text = "Aba 02" };
                    break;

                case InternalTabs.Profile:
                    ctview_page.Content = new Label() { Text = "Aba 03" };
                break;
            }
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

			this.current_tab_index = Grid.GetColumn(selected_tab);

			LoadTab();

        }
		catch (Exception ex)
		{
			await DisplayAlertAsync("Erro!", ex.Message, "OK");
		}
    }
}