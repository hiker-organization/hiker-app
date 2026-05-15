using App_Hiker.Model.User.Response;

namespace App_Hiker.View.User;

public partial class UserReviewsListing : ContentView
{
	public UserReviewsListing()
	{
		InitializeComponent();
	}

    protected override void OnBindingContextChanged()
    {
        try
		{
            base.OnBindingContextChanged();

            LoadUserData();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

	private async void LoadUserData()
	{
		try
		{
			if (this.BindingContext != null)
			{
                UserDataResponse profile_data = (UserDataResponse)this.BindingContext;

                clview_posts_usuario.ItemsSource = profile_data.reviews;
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }
}