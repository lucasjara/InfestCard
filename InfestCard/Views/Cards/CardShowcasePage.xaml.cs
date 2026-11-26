
using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CardShowcasePage : ContentPage
{
    private const double MaxTilt = 18;
    private const double TiltSensitivity = 12;

    private bool hasPlayedEntrance;
    private bool isDragging;
    private bool isReturning;

    private CancellationTokenSource? idleCancellation;

    public CardShowcasePage(CardDetailViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (hasPlayedEntrance)
            return;

        hasPlayedEntrance = true;

        await PlayEntranceAnimationAsync();

        if (IsVisible)
            StartIdleAnimation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        StopIdleAnimation();
    }

    // ==========================================
    // ANIMACIÓN DE ENTRADA
    // ==========================================

    private async Task PlayEntranceAnimationAsync()
    {
        CardSurface.RotationY = -18;
        CardSurface.RotationX = 12;

        HoloLayer.Opacity = 0;
        ShineLayer.Opacity = 0;

        await Task.WhenAll(
            CardSurface.FadeToAsync(
                1, 550, Easing.CubicOut),

            CardSurface.ScaleToAsync(
                1, 700, Easing.SpringOut),

            CardSurface.TranslateToAsync(
                0, 0, 650, Easing.CubicOut)
        );

        // La carta se endereza.
        await Task.WhenAll(
            AnimateRotationAsync(
                CardSurface,
                0, 0, 450),

            HoloLayer.FadeToAsync(
                0.30, 500, Easing.CubicOut),

            ShineLayer.FadeToAsync(
                0.20, 500, Easing.CubicOut)
        );

        // Primer destello.
        await ShineLayer.FadeToAsync(
            0.65, 250, Easing.CubicOut);

        await ShineLayer.FadeToAsync(
            0.20, 450, Easing.CubicOut);
    }

    // ==========================================
    // MOVIMIENTO 3D
    // ==========================================

    private void Card_PanUpdated(
        object? sender,
        PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:

                isDragging = true;
                isReturning = false;

                StopIdleAnimation();

                CardSurface.CancelAnimations();

                break;

            case GestureStatus.Running:

                isDragging = true;

                double rotationY = Math.Clamp(
                    e.TotalX / TiltSensitivity,
                    -MaxTilt,
                    MaxTilt);

                double rotationX = Math.Clamp(
                    -e.TotalY / TiltSensitivity,
                    -MaxTilt,
                    MaxTilt);

                // Inclinamos la carta completa.
                CardSurface.RotationY = rotationY;
                CardSurface.RotationX = rotationX;

                // Movimiento físico de la carta.
                CardSurface.TranslationX =
                    Math.Clamp(e.TotalX * 0.08, -12, 12);

                CardSurface.TranslationY =
                    Math.Clamp(e.TotalY * 0.08, -12, 12);

                // Reflejos según la posición del dedo.
                UpdateHolographicEffects(
                    rotationX,
                    rotationY);

                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:

                isDragging = false;

                _ = ReturnCardToCenterAsync();

                break;
        }
    }

    // ==========================================
    // EFECTOS HOLOGRÁFICOS
    // ==========================================

    private void UpdateHolographicEffects(
        double rotationX,
        double rotationY)
    {
        double intensity = Math.Clamp(
            (Math.Abs(rotationX) + Math.Abs(rotationY)) / 30,
            0.20,
            1);

        HoloLayer.Opacity = 0.20 + intensity * 0.55;

        ShineLayer.Opacity = 0.15 + intensity * 0.65;

        // El reflejo acompaña el movimiento.
        HoloLayer.TranslationX = rotationY * 0.45;
        HoloLayer.TranslationY = -rotationX * 0.45;

        ShineLayer.TranslationX = rotationY * 0.65;
        ShineLayer.TranslationY = -rotationX * 0.65;

        // Rotación visual del reflejo.
        ShineLayer.Rotation = rotationY * 0.7;
    }

    // ==========================================
    // REGRESO AUTOMÁTICO AL CENTRO
    // ==========================================

    private async Task ReturnCardToCenterAsync()
    {
        if (isReturning)
            return;

        isReturning = true;

        StopIdleAnimation();

        CardSurface.CancelAnimations();

        await Task.WhenAll(
            AnimateRotationAsync(
                CardSurface,
                0, 0, 450),

            CardSurface.TranslateToAsync(
                0, 0, 450, Easing.SpringOut),

            HoloLayer.TranslateToAsync(
                0, 0, 450, Easing.CubicOut),

            ShineLayer.TranslateToAsync(
                0, 0, 450, Easing.CubicOut),

            ShineLayer.FadeToAsync(
                0.20, 350, Easing.CubicOut),

            HoloLayer.FadeToAsync(
                0.30, 350, Easing.CubicOut)
        );

        ShineLayer.Rotation = 0;

        isReturning = false;

        if (!isDragging && IsVisible)
            StartIdleAnimation();
    }

    // ==========================================
    // ANIMACIÓN DE ROTACIÓN
    // ==========================================

    private static Task AnimateRotationAsync(
        VisualElement element,
        double targetX,
        double targetY,
        uint duration)
    {
        var completion = new TaskCompletionSource();

        double startX = element.RotationX;
        double startY = element.RotationY;

        new Animation(
            value =>
            {
                element.RotationX =
                    startX + (targetX - startX) * value;

                element.RotationY =
                    startY + (targetY - startY) * value;
            },
            0,
            1
        ).Commit(
            element,
            "CardRotation",
            length: duration,
            easing: Easing.CubicOut,
            finished: (_, _) => completion.TrySetResult()
        );

        return completion.Task;
    }

    // ==========================================
    // MOVIMIENTO FLOTANTE
    // ==========================================

    private void StartIdleAnimation()
    {
        StopIdleAnimation();

        idleCancellation = new CancellationTokenSource();

        _ = PlayIdleAnimationAsync(
            idleCancellation.Token);
    }

    private void StopIdleAnimation()
    {
        if (idleCancellation == null)
            return;

        idleCancellation.Cancel();
        idleCancellation.Dispose();
        idleCancellation = null;

        CardSurface.CancelAnimations();
    }

    private async Task PlayIdleAnimationAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (isDragging || isReturning)
                {
                    await Task.Delay(
                        100,
                        cancellationToken);

                    continue;
                }

                await CardSurface.TranslateToAsync(
                    0, -7, 1300, Easing.SinInOut);

                cancellationToken.ThrowIfCancellationRequested();

                await CardSurface.TranslateToAsync(
                    0, 0, 1300, Easing.SinInOut);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException)
        {
            // La animación se detuvo correctamente.
        }
    }

    // ==========================================
    // REGRESAR
    // ==========================================

    private async void BackButton_Clicked(
        object sender,
        EventArgs e)
    {
        StopIdleAnimation();

        await Task.WhenAll(
            CardSurface.FadeToAsync(
                0, 180, Easing.CubicIn),

            CardSurface.ScaleToAsync(
                0.85, 180, Easing.CubicIn),

            HoloLayer.FadeToAsync(
                0, 150, Easing.CubicIn),

            ShineLayer.FadeToAsync(
                0, 150, Easing.CubicIn)
        );

        await Navigation.PopAsync();
    }
}
