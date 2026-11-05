using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CardDetailPage : ContentPage
{
    public CardDetailViewModel ViewModel => (CardDetailViewModel)BindingContext;

    public CardDetailPage(CardDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
