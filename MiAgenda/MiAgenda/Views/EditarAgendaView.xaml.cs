using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiAgenda.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarAgendaView : ContentPage
	{
		public EditarAgendaView ()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			InitializeComponent ();
		}
	}
}