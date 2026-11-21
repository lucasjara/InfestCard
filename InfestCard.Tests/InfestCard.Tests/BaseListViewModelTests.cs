using InfestCard.ViewModels.Events;

namespace InfestCard.Tests;

public class TestItem
{
    public string Title { get; set; } = "";
    public string Category { get; set; } = "";
}

public class TestListViewModel : BaseListViewModel<TestItem>
{
    public TestListViewModel() { InitializeCategories("A", "B"); }

    protected override bool MatchesCategory(TestItem item, string category) => item.Category == category;
    protected override bool MatchesSearch(TestItem item, string search) => item.Title.Contains(search, StringComparison.OrdinalIgnoreCase);
}

public class BaseListViewModelTests
{
    [Fact]
    public void ApplyFilters_FiltersByCategory()
    {
        var vm = new TestListViewModel();
        vm.AllItems.Add(new TestItem { Title = "x", Category = "A" });
        vm.AllItems.Add(new TestItem { Title = "y", Category = "B" });
        vm.SelectedCategory = "A";
        vm.ApplyFilters();
        Assert.Single(vm.Items);
        Assert.Equal("x", vm.Items[0].Title);
    }

    [Fact]
    public void ApplyFilters_SearchText()
    {
        var vm = new TestListViewModel();
        vm.AllItems.Add(new TestItem { Title = "hello", Category = "A" });
        vm.AllItems.Add(new TestItem { Title = "world", Category = "A" });
        vm.SearchText = "hello";
        vm.ApplyFilters();
        Assert.Single(vm.Items);
    }

    [Fact]
    public void HasResults_FalseWhenEmpty()
    {
        var vm = new TestListViewModel();
        vm.ApplyFilters();
        Assert.False(vm.HasResults);
        Assert.True(vm.HasNoResults);
    }
}
