#if ANDROID
using AndroidX.Lifecycle;
#endif

using InfestCard.Configuration;
using InfestCard.Data;
using InfestCard.Data.Repositories;
using InfestCard.Services.Implementations;
using InfestCard.Services.Interfaces;
using InfestCard.ViewModels;
using InfestCard.ViewModels.Events;
using InfestCard.Views;
using InfestCard.Views.Events;
using InfestCard.Views.Home;
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

            // Event guide
            builder.Services.AddSingleton<
                IGuideNavigationService,
                GuideNavigationService>();

            builder.Services.AddTransient<ExplorerMapViewModel>();
            builder.Services.AddTransient<ExplorerMapPage>();

            // Stands
            builder.Services.AddTransient<StandsViewModel>();
            builder.Services.AddTransient<StandsPage>();

            // Activities
            builder.Services.AddTransient<ActivitiesViewModel>();
            builder.Services.AddTransient<ActivitiesPage>();

            // Workshops
            builder.Services.AddTransient<WorkshopsViewModel>();
            builder.Services.AddTransient<WorkshopsPage>();

            // Tournaments
            builder.Services.AddTransient<TournamentsViewModel>();
            builder.Services.AddTransient<TournamentsPage>();

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
            // Home Page
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddSingleton<HomePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}