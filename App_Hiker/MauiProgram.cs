using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

namespace App_Hiker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    // Texto.

                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Aclonica-Regular.ttf", "AclonicaRegular");

                    fonts.AddFont("Amiko-Bold.ttf", "AmikoBold");
                    fonts.AddFont("Amiko-Regular.ttf", "AmikoRegular");
                    fonts.AddFont("Amiko-Semibold.ttf", "AmikoSemibold");

                    // Ícones.

                    fonts.AddFont("Font-Awesome-7-Brands-Regular-400.otf", "FontAwesomeBrandsRegular400");
                    fonts.AddFont("Font-Awesome-7-Free-Regular-400.otf", "FontAwesomeFreeRegular400");
                    fonts.AddFont("Font-Awesome-7-Free-Solid-900.otf", "FontAwesomeFreeSolid900");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
