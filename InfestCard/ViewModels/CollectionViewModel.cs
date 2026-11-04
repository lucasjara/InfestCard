using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.Models;

namespace InfestCard.ViewModels;

public partial class CollectionViewModel : BaseViewModel
{
    public CollectionViewModel()
    {
        Cards.CollectionChanged += (_, _) => OnPropertyChanged(nameof(IsEmpty));
    }

    public ObservableCollection<Card> Cards { get; } = [];

    public bool IsEmpty => Cards.Count == 0;
}
