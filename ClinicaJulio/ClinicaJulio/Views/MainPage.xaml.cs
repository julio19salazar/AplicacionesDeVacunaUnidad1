using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClinicaJulio
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private void btnBuscarPersona_Clicked(object sender, EventArgs e)
        {
           
        }

        private void btnRegresar_Clicked(object sender, EventArgs e)
        {
            txtPersonaBuscar.Text = "";
        }
    }
}
