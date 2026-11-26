using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class ExperiencesPage : ContentPage
{
    public ExperiencesPage(ExperiencesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}