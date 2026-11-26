using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfestCard.Services.Interfaces;

namespace InfestCard.ViewModels.Events;

public partial class ExplorerMapViewModel : ObservableObject
{
    private readonly IGuideNavigationService _navigationService;

    public ExplorerMapViewModel(
        IGuideNavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task NavigateToModuleAsync(string moduleName)
    {
        if (string.IsNullOrWhiteSpace(moduleName))
            return;

        await _navigationService.NavigateToModuleAsync(moduleName);
    }
}
