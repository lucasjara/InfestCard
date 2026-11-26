using System.Collections.ObjectModel;
using InfestCard.Models;
using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CollectionFiltersPage : ContentPage
{
    private readonly CollectionViewModel viewModel;

    public ObservableCollection<CardTypeFilter> FilterOptions { get; } = [];

    public CollectionFiltersPage(CollectionViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;

        foreach (var filter in viewModel.AvailableTypes)
        {
            FilterOptions.Add(new CardTypeFilter
            {
                Type = filter.Type,
                Label = filter.Label,
                Icon = filter.Icon,
                IsSelected = viewModel.SelectedTypes.Contains(filter.Type)
            });
        }
        BindingContext = this;
    }

    private async void OnApplyClicked(object? sender, EventArgs e)
    {
        var selectedTypes = FilterOptions
            .Where(filter => filter.IsSelected)
            .Select(filter => filter.Type)
            .ToList();

        viewModel.ApplyTypeFilters(selectedTypes);

        await Navigation.PopModalAsync();
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        foreach (var filter in FilterOptions)
            filter.IsSelected = false;
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
