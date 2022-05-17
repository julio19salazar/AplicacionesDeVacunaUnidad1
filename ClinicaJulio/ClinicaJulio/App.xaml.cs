using ClinicaJulio.Models;
using ClinicaJulio.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClinicaJulio
{
    public partial class App : Application
    {
        public static CatalogoPersonas Catalogo { get; set; } = new CatalogoPersonas();
        public static SincronizadorServices Sincronizador { get; set; }
        public App()
        {
            InitializeComponent();
            Sincronizador = new SincronizadorServices(Catalogo);
            MainPage = new NavigationPage( new MainPage());
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
