namespace InfestCard.Services.Interfaces;

public interface IGuideNavigationService
{
    Task NavigateToModuleAsync(string moduleName);
}
