using CommunityToolkit.Mvvm.ComponentModel;

namespace InfestCard.Models;

public partial class CardTypeFilter : ObservableObject
{
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    [ObservableProperty]
    private bool isSelected;
}