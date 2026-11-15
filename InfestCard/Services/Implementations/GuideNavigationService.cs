using InfestCard.Services.Interfaces;
using InfestCard.Views.Events;

namespace InfestCard.Services.Implementations;

public class GuideNavigationService : IGuideNavigationService
{
    public async Task NavigateToModuleAsync(string moduleName)
    {
        var rootPage = Application.Current?
            .Windows
            .FirstOrDefault()?
            .Page;

        if (rootPage is null)
            return;

        var page = new GuideModulePage(moduleName);

        await rootPage.Navigation.PushAsync(page);
    }
}
