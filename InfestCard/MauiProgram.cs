#if ANDROID
using AndroidX.Lifecycle;
#endif
using InfestCard.Configuration;
using InfestCard.Data;
using InfestCard.Data.Repositories;
using InfestCard.Services.Implementations;
using InfestCard.Services.Interfaces;
using InfestCard.ViewModels;
using InfestCard.Views;
using Microsoft.Extensions.Logging;
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

            // Main
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();

            // Database
            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddSingleton<ICardRepository, CardRepository>();
            builder.Services.AddSingleton<ICardStorageService, CardStorageService>();

            // API
            builder.Services.AddSingleton(new ApiOptions
            {
                BaseUrl = string.Empty
            });

            builder.Services.AddSingleton(new HttpClient());
            builder.Services.AddSingleton<ICardApiService, CardApiService>();

            // Images
            builder.Services.AddSingleton<ICardImageService, CardImageService>();

            // Camera and QR
            builder.Services.AddSingleton<ICameraPermissionService, CameraPermissionService>();
            builder.Services.AddTransient<IQrScannerService, QrScannerService>();

            // Navigation
            builder.Services.AddSingleton<IAppNavigationService, AppNavigationService>();

            // Collection
            builder.Services.AddSingleton<CollectionViewModel>();
            builder.Services.AddSingleton<CollectionPage>();

            // Card detail
            builder.Services.AddTransient<CardDetailViewModel>();
            builder.Services.AddTransient<CardDetailPage>();

            // Scanner
            builder.Services.AddTransient<ScannerViewModel>();
            builder.Services.AddTransient<ScannerPage>();

            // QR Code
            builder.Services.AddTransient<QrCodePage>();

            // Sharing
            builder.Services.AddSingleton<ICardShareService, CardShareService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
