using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.ViewModels;

namespace InfestCard.ViewModels.Events;

public abstract partial class BaseListViewModel<T> : BaseViewModel
{
    protected const string AllCategory = "Todos";

    public readonly List<T> AllItems = [];

    [ObservableProperty]
    private ObservableCollection<T> items = [];

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedCategory = AllCategory;

    [ObservableProperty]
    private bool hasResults;

    [ObservableProperty]
    private bool hasNoResults;

    public ObservableCollection<string> Categories { get; } = [];

    protected void InitializeCategories(params string[] categories)
    {
        Categories.Clear();
        Categories.Add(AllCategory);

        foreach (var category in categories)
        {
            if (!string.Equals(category, AllCategory, StringComparison.Ordinal) &&
                !Categories.Contains(category))
            {
                Categories.Add(category);
            }
        }
    }

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    partial void OnSelectedCategoryChanged(string value) => ApplyFilters();

    public void SelectCategory(string category) => SelectedCategory = category;

    protected abstract bool MatchesCategory(T item, string category);

    protected abstract bool MatchesSearch(T item, string search);

    protected virtual IEnumerable<T> Order(IEnumerable<T> items) => items;

    public void ApplyFilters()
    {
        var filtered = AllItems
            .Where(item =>
                (SelectedCategory == AllCategory || MatchesCategory(item, SelectedCategory)) &&
                (string.IsNullOrWhiteSpace(SearchText) || MatchesSearch(item, SearchText)));

        Items.Clear();

        foreach (var item in Order(filtered))
        {
            Items.Add(item);
        }

        HasResults = Items.Count > 0;
        HasNoResults = !HasResults;
    }
}
