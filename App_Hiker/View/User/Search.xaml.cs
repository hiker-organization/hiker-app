using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class Search : ContentView
{
    private readonly SearchViewModel _viewModel;

    public Search()
    {
        InitializeComponent();

        _viewModel = new SearchViewModel();

        BindingContext = _viewModel;
    }
}
