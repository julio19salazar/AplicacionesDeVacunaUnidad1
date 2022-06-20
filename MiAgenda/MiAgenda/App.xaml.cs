using MiAgenda.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiAgenda
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static event Action BdActualizada;
        public static void Actualizar()
        {
            BdActualizada?.Invoke();
        }

        public App()
        {
            InitializeComponent();
            SetupServices();
            MainPage = new NavigationPage(new MainPage());
        }
        void SetupServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<AgendaRepository>();


            ServiceProvider = services.BuildServiceProvider();

        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
