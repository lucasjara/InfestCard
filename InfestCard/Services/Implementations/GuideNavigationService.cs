using InfestCard.Services.Interfaces;
using InfestCard.Views.Events;
using Microsoft.Extensions.DependencyInjection;

namespace InfestCard.Services.Implementations;

public class GuideNavigationService : IGuideNavigationService
{
    private readonly IServiceProvider _services;

    public GuideNavigationService(IServiceProvider services)
    {
        _services = services;
    }

    public async Task NavigateToModuleAsync(string moduleName)
    {
        var navigation = Application.Current?
            .Windows
            .FirstOrDefault()?
            .Page?
            .Navigation;

        if (navigation is null)
            return;

        Page page = moduleName switch
        {
            "stands" => _services.GetRequiredService<StandsPage>(),
            "activities" => _services.GetRequiredService<ActivitiesPage>(),
            "workshops" => _services.GetRequiredService<WorkshopsPage>(),
            "tournaments" => _services.GetRequiredService<TournamentsPage>(),
            "cosplay" => _services.GetRequiredService<CosplayPage>(),
            "experiences" => _services.GetRequiredService<ExperiencesPage>(),
            "sponsors" => _services.GetRequiredService<SponsorsPage>(),
            "services" => _services.GetRequiredService<ServicesPage>(),
            "information" => _services.GetRequiredService<InformationPage>(),

            _ => new GuideModulePage(moduleName)
        };
        NavigationPage.SetHasNavigationBar(page, false);
        await navigation.PushAsync(page);
    }
}