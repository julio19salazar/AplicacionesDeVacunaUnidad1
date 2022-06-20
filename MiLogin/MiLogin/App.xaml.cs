
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiLogin
{
    public partial class App : Application
    {
        
             public static MiLogin.Services.UserServices User { 
            get; private set; } = new MiLogin.Services.UserServices();
        public App()
        {
            User.RedirigirPrincipal();
            //InitializeComponent();
            //if (User.EstoyAutenticado || User.Renovar().Result)
            //{
            //    User.RedirigirPrincipal();
            //}
            //else
            //{
            //    MainPage = new Views.LoginVIew();
            //}

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
