using InfestCard.Views;

namespace InfestCard.Views.Authentication;

public partial class WelcomePage : ContentPage
{
    private readonly CollectionPage _collectionPage;

    public WelcomePage(CollectionPage collectionPage)
    {
        InitializeComponent();

        _collectionPage = collectionPage;
    }

    private async void OnGuestClicked(
        object sender,
        EventArgs e)
    {
        GuestButton.IsEnabled = false;

        try
        {
            NavigationPage.SetHasNavigationBar(
                _collectionPage,
                false
            );

            await Navigation.PushAsync(_collectionPage);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"No fue posible abrir la colección: {ex.Message}",
                "Aceptar"
            );
        }
        finally
        {
            GuestButton.IsEnabled = true;
        }
    }

    private async void OnGoogleLoginClicked(
        object sender,
        EventArgs e)
    {
        await DisplayAlertAsync(
            "Inicio de sesión",
            "La autenticación con Google estará disponible próximamente.",
            "Aceptar"
        );
    }
}