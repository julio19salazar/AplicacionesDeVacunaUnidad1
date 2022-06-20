using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MiAgenda
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            IToast toast = DependencyService.Get<IToast>();
            toast.ShowNotification("Aplicación iniciada");
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
     
    }
}
