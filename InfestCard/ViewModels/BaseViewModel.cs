using CommunityToolkit.Mvvm.ComponentModel;

namespace InfestCard.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;
}
