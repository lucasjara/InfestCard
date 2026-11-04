using Microsoft.Extensions.DependencyInjection;

namespace InfestCard
{
    public partial class App : Application
    {
        private readonly AppShell appShell;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            appShell = services.GetRequiredService<AppShell>();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(appShell);
        }
    }
}