using MiAgenda.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClinicaJulio.Services
{
   public class AgendaTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Edicion { get; set; }
        public DataTemplate Lectura { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var chat = (AgendaModel)item;
            if (chat.Id % 2 == 0)
            {
                return Edicion;
            }
            else
            {
                return Lectura;
            }
        }
    }
}
