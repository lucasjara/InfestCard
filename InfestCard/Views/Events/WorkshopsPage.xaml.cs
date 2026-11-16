using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class WorkshopsPage : ContentPage
{
    public WorkshopsPage(WorkshopsViewModel viewModel)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = viewModel;
    }
}