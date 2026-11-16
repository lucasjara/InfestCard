using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class WorkshopsPage : ContentPage
{
    public WorkshopsPage(WorkshopsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}