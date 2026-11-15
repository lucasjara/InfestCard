namespace InfestCard.Views.Events;

public partial class GuideModulePage : ContentPage
{
    public GuideModulePage(string moduleName)
    {
        InitializeComponent();

        Title = moduleName;
        ModuleTitleLabel.Text = moduleName;
    }
}
