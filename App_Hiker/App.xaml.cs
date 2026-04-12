using System.Diagnostics;

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

        internal static void ShowInDebugConsole(string message)
        {
            Debug.WriteLine("--------------------------------------------------");
            Debug.WriteLine(message);
            Debug.WriteLine("--------------------------------------------------\n");
        }

        internal static void SetRequestedTheme(AppTheme requested_theme)
        {
            Application? current_app = (Application?) App.Current;

            if (current_app != null)
            {
                // Efetuando a troca do tema do aplicativo de forma manual e forçada.

                current_app.UserAppTheme = requested_theme;

                // Aplicando as configurações do tema requisitado.

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

        private async void ApplyRequestedThemeColors()
        {
            Application? current_app = (Application?)App.Current;

            if (current_app != null)
            {
                // Evento de troca de tema gerenciado pelo dispositivo (Android, iOS, Windows, etc.).

                current_app.RequestedThemeChanged += async (sender, e) =>
                {
                    if (e.RequestedTheme != current_app.UserAppTheme)
                    {
                        SetRequestedTheme(e.RequestedTheme);
                    }
                };

                // Aplicação do tema inicial.

                SetRequestedTheme(AppTheme.Dark);
            }
        }
    }
}