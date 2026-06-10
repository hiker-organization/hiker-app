using System.Windows.Input;

namespace App_Hiker.View.User;

public partial class UserReviewsListing : ContentView
{
    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(UserReviewsListing));

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public UserReviewsListing()
    {
        InitializeComponent();
    }
}
