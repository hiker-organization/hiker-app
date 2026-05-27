using App_Hiker.Model.Api;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.User;

namespace App_Hiker.View.User;

public partial class Profile : ContentView
{
    private Application? current_app = (Application?)App.Current;

    private enum InternalTabs
    {
        PersonalData,
        Posts,
        TimeLine,
        Favorites
    };

    private InternalTabs current_profile_tab_index = InternalTabs.PersonalData;

    private string internal_context = String.Empty;

    private UserDataResponse? user_data = new UserDataResponse();

	public Profile(string context)
	{
		InitializeComponent();

        this.internal_context = context;

        InitializeResources();
    }

    private async void InitializeResources()
    {
        try
        {
            await LoadUserData();

            LoadTab();
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private void SetUserData(UserDataResponse data)
    {
        try
        {
            lbl_user_real_name.Text = data.nome_exibicao;
            lbl_user_name.Text = data.nome_usuario;
            lbl_posts_quantity.Text = data.reviews.Count.ToString();
            lbl_reputation.Text = data.reputacao_normalizada;
            img_user_photo.Source = data.foto_url;
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }

    private async Task LoadUserData()
    {
        try
        {
            DataResponse<UserDataResponse> response = new DataResponse<UserDataResponse>();

            if (this.internal_context == "AuthUserContext")
            {
                response = await AuthService.Me();
            }

            this.user_data = response.data;

            if (response.data != null)
            {
                SetUserData(response.data);
            }
        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
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
                        if (Grid.GetColumn(button) == (int)current_profile_tab_index)
                        {
                            button.TextColor = (Color)current_app.Resources["Primary"];
                        }
                        else
                        {
                            button.TextColor = (Color)current_app.Resources["BaseContent"];
                        }
                    }
                }

                grid_selected_tab_marker.SetColumn(bv_selected_tab_marker, (int)this.current_profile_tab_index);
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
            InternalTabs tab_option = this.current_profile_tab_index;

            switch (tab_option)
            {
                case InternalTabs.PersonalData:
                    ctv_profile_current_tab.Content = new UserPersonalData();
                break;

                case InternalTabs.Posts:
                    ctv_profile_current_tab.Content = new UserReviewsListing()
                    {
                        BindingContext = this.user_data,
                    };
                break;

                case InternalTabs.TimeLine:
                    ctv_profile_current_tab.Content = new Label() { Text = "Aba 03 (Perfil)" };
                break;

                case InternalTabs.Favorites:
                    ctv_profile_current_tab.Content = new Label() { Text = "Aba 04 (Perfil)" };
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

            this.current_profile_tab_index = (InternalTabs)Grid.GetColumn(selected_tab);

            LoadTab();

        }
        catch (Exception ex)
        {
            App.ShowInDebugConsole(ex.Message); // Temporário.
        }
    }
}