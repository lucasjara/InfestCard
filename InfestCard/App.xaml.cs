using InfestCard.Views;
using InfestCard.Views.Authentication;
using InfestCard.Views.Home;
using Microsoft.Extensions.DependencyInjection;

namespace InfestCard;

public partial class App : Application
{
    private readonly CollectionPage _CollectionPage;
    private readonly HomePage _homePage;
    private readonly Page _startPage;


    public App(IServiceProvider services)
    {
        InitializeComponent();
        /*
        #if DEBUG
                _startPage = services.GetRequiredService<HomePage>();
        #else
                _CollectionPage = services.GetRequiredService<CollectionPage>();
        #endif
        */
        _startPage = services.GetRequiredService<WelcomePage>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
    #if DEBUG
            NavigationPage.SetHasNavigationBar(_startPage, false);
            return new Window(new NavigationPage(_startPage));
    #else
            NavigationPage.SetHasNavigationBar(_CollectionPage, false);
            return new Window(new NavigationPage(_CollectionPage));
    #endif
    }
}