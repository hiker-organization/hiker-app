using System.Windows.Input;

using App_Hiker.ViewModel.User;

namespace App_Hiker.View.User;

public partial class Feed : ContentView
{
    private readonly FeedViewModel _viewModel;
    private bool _initialized = false;

    public Feed()
    {
        InitializeComponent();

        _viewModel = new FeedViewModel();

        BindingContext = _viewModel;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null && !_initialized)
        {
            _initialized = true;

            if (_viewModel.LoadCommand is ICommand load && load.CanExecute(null))
            {
                load.Execute(null);
            }
        }
    }
}
