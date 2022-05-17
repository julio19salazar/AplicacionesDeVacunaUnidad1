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
    public partial class EditarPersonaView : ContentPage
    {
        public EditarPersonaView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}