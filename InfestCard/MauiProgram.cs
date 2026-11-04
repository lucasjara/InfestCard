using Microsoft.Extensions.Logging;
using InfestCard.Data;
using InfestCard.Data.Repositories;
using InfestCard.Services.Implementations;
using InfestCard.Services.Interfaces;
using InfestCard.ViewModels;
using InfestCard.Views;
using ZXing.Net.Maui.Controls;

namespace InfestCard
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddSingleton<ICardRepository, CardRepository>();
            builder.Services.AddSingleton<ICardStorageService, CardStorageService>();
            builder.Services.AddSingleton<ICameraPermissionService, CameraPermissionService>();
            builder.Services.AddSingleton<IQrScannerService, QrScannerService>();
            builder.Services.AddSingleton<IAppNavigationService, AppNavigationService>();
            builder.Services.AddSingleton<CollectionViewModel>();
            builder.Services.AddSingleton<CollectionPage>();
            builder.Services.AddTransient<ScannerViewModel>();
            builder.Services.AddTransient<ScannerPage>();
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
