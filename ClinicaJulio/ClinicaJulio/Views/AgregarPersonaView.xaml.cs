using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClinicaJulio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPersonaView : ContentPage
    {
        public AgregarPersonaView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }
    }
}