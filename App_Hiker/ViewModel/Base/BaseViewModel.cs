using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App_Hiker.ViewModel.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;

            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helpers de UI que não acoplam o ViewModel a tipos de View.

        protected static Task<bool> DisplayAlert(string title, string message, string accept, string? cancel = null)
        {
            Page? page = Application.Current?.Windows.FirstOrDefault()?.Page;

            if (page == null)
            {
                System.Diagnostics.Debug.WriteLine($"Não foi possível exibir alerta: nenhuma Window ativa. {title} - {message}");

                return Task.FromResult(false);
            }

            return cancel == null
                ? page.DisplayAlertAsync(title, message, accept).ContinueWith(_ => true)
                : page.DisplayAlertAsync(title, message, accept, cancel);
        }

        protected static Task GoToAsync(string route)
        {
            return Shell.Current.GoToAsync(route);
        }
    }
}
