using InfestCard.Views;
using InfestCard.Views.Home;
using Microsoft.Extensions.DependencyInjection;

namespace InfestCard;

public partial class App : Application
{
    private readonly HomePage _homePage;

    public App(IServiceProvider services)
    {
        InitializeComponent();

        _homePage = services.GetRequiredService<HomePage>();
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        NavigationPage.SetHasNavigationBar(_homePage, false);

        return new Window(new NavigationPage(_homePage));
    }
}