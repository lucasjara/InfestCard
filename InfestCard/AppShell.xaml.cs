using InfestCard.Views;

namespace InfestCard
{
    public partial class AppShell : Shell
    {
        public AppShell(CollectionPage collectionPage)
        {
            InitializeComponent();
            Items.Add(new ShellContent
            {
                Title = "Colección",
                Route = "CollectionPage",
                Content = collectionPage
            });
        }
    }
}
