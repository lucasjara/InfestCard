/*
using InfestCard.Views;
using Microsoft.Extensions.DependencyInjection;

namespace InfestCard
{
    public partial class App : Application
    {
        private readonly CollectionPage collectionPage;

        public App(IServiceProvider services)
        {
            InitializeComponent();

            collectionPage = services.GetRequiredService<CollectionPage>();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            NavigationPage.SetHasNavigationBar(collectionPage, false);

            return new Window(new NavigationPage(collectionPage));
        }
    }
}
*/
using InfestCard.Views.Events;

namespace InfestCard
{
    public partial class App : Application
    {
        private readonly ExplorerMapPage _explorerMapPage;

        public App(ExplorerMapPage explorerMapPage)
        {
            InitializeComponent();

            _explorerMapPage = explorerMapPage;
        }

        protected override Window CreateWindow(
            IActivationState? activationState)
        {
            NavigationPage.SetHasNavigationBar(
                _explorerMapPage,
                false);

            var navigationPage = new NavigationPage(_explorerMapPage)
            {
                BarBackgroundColor = Color.FromArgb("#09090F"),
                BarTextColor = Colors.White
            };

            return new Window(navigationPage);
        }
    }
}
