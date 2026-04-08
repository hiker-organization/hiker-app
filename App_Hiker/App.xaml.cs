using Microsoft.Extensions.DependencyInjection;

namespace App_Hiker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApplyRequestedThemeColors();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new AppShell());

            window.Height = 600;
            window.Width = 350;

            return window;
        }

        private void SetRequestedTheme(AppTheme requested_theme)
        {
            Application? current_app = (Application?) App.Current;

            if (current_app != null)
            {
                ICollection<ResourceDictionary> app_dictionaries = current_app.Resources.MergedDictionaries;

                ResourceDictionary? current_app_theme = app_dictionaries.FirstOrDefault(dictionary =>
                {
                    return dictionary is Resources.Styles.Dark.Colors || dictionary is Resources.Styles.Light.Colors;
                });

                if (current_app_theme != null)
                {
                    app_dictionaries.Remove(current_app_theme);
                }

                switch (requested_theme)
                {
                    case AppTheme.Dark:
                        app_dictionaries.Add(new Resources.Styles.Dark.Colors());
                    break;

                    default:
                        app_dictionaries.Add(new Resources.Styles.Light.Colors());
                    break;
                }
            }
        }

        private void ApplyRequestedThemeColors()
        {
            Application? current_app = (Application?)App.Current;

            if (current_app != null)
            {
                // Aplicação inicial.

                SetRequestedTheme(current_app.RequestedTheme);

                // Evento de troca de tema durante a execução do aplicativo.

                current_app.RequestedThemeChanged += (sender, e) =>
                {
                    SetRequestedTheme(e.RequestedTheme);
                };
            }
        }
    }
}