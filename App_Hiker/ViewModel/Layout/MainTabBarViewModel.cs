using System.Windows.Input;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.Layout
{
    public class MainTabBarViewModel : BaseViewModel
    {
        // Índices das abas: Home=0, NewReview=1, Search=2, Profile=3.
        private const int ProfileTab = 3;

        private int _currentTabIndex = 0;
        private int _previousTabIndex = 0;

        public int CurrentTabIndex
        {
            get => _currentTabIndex;
            set => SetProperty(ref _currentTabIndex, value);
        }

        public int PreviousTabIndex
        {
            get => _previousTabIndex;
            set => SetProperty(ref _previousTabIndex, value);
        }

        public ICommand SelectTabCommand { get; }

        public MainTabBarViewModel()
        {
            SelectTabCommand = new Command<string>(OnSelectTab);
        }

        public void GoToPreviousTab()
        {
            CurrentTabIndex = PreviousTabIndex;
        }

        private void OnSelectTab(string? rawIndex)
        {
            if (!int.TryParse(rawIndex, out int nextTab))
            {
                return;
            }

            if (nextTab == ProfileTab && CurrentTabIndex != ProfileTab)
            {
                PreviousTabIndex = CurrentTabIndex;
            }

            CurrentTabIndex = nextTab;
        }
    }
}
