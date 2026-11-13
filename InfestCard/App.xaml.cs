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
