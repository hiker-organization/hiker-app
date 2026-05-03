namespace App_Hiker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            /*if (await SecureStorage.GetAsync("token") != null)
            {
                await Shell.Current.GoToAsync("//Profile");
            }
            else
            {
                await Shell.Current.GoToAsync("//Auth");
            }*/

            await Shell.Current.GoToAsync("//Auth");
        }
    }
}
