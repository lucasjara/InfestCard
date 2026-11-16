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
            "Stands" => _services.GetRequiredService<StandsPage>(),
            "Activities" => _services.GetRequiredService<ActivitiesPage>(),

            _ => new GuideModulePage(moduleName)
        };
        NavigationPage.SetHasNavigationBar(page, false);
        await navigation.PushAsync(page);
    }
}
