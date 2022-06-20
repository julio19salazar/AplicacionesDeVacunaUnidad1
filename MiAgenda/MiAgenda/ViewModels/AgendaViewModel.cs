using MiAgenda.Helpers;
using MiAgenda.Models;
using MiAgenda.Repositories;
using MiAgenda.Services;
using MiAgenda.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiAgenda.ViewModels
{
  public class AgendaViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<AgendaModel> ListaAgendas { get; set; } =
         new ObservableCollection<AgendaModel>();
        IToast toast = DependencyService.Get<IToast>();
        
        public AgendaRepository Repository { get; set; }
        public ICommand EnviarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public string MiActividad { get; set; }
        public string MiFechaDeRealizar { get; set; }
        public string MiImportancia { get; set; }

        AgregarAgendaView VistaAgregarAgenda;
        EditarAgendaView VistaEditarAgenda;


        private AgendaModel agenda;

        public AgendaModel Agenda
        {
            get { return agenda; }
            set
            {
                agenda = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Agenda)));
            }
        }
        public AgendaViewModel()
        {
            //Username = DependencyService.Get<IUsuario>().GetUsername();

            cliente = new HttpClientHelper<AgendaModel>(new Uri("https://181g0543.81g.itesrc.net/api/agendas"));
            VerAgregarCommand = new Command(VerAgregar);
            VerEditarCommand = new Command(VerEditar);
            EnviarCommand = new Command(Enviar);
            EliminarCommand = new Command(Eliminar);
            EditarCommand = new Command(Editar);
            GuardarCommand = new Command<AgendaModel>(Guardar);
            CancelarCommand = new Command(Cancelar);
            Repository = App.ServiceProvider.GetService<AgendaRepository>();
            App.BdActualizada += App_BdActualizada; 

            Actualizar();
        }
        private async void VerEditar(object obj)
        {

            Agenda = obj as AgendaModel;
            if (VistaEditarAgenda == null)
            {
                VistaEditarAgenda = new EditarAgendaView() { BindingContext = this };


            }
           
            await App.Current.MainPage.Navigation.PushAsync(VistaEditarAgenda);




        }
        private async void VerAgregar()
        {
            if (VistaAgregarAgenda == null)
            {
                VistaAgregarAgenda = new AgregarAgendaView() { BindingContext = this };

            }
            Agenda =  new AgendaModel();
            await App.Current.MainPage.Navigation.PushAsync(VistaAgregarAgenda);
        }
        private async void Cancelar()
        {
            MiActividad = null;
           MiFechaDeRealizar = null;
           MiImportancia = null;
            
            await App.Current.MainPage.Navigation.PopAsync();
            VistaAgregarAgenda = null;
            VistaEditarAgenda = null;
         
        }
        private void App_BdActualizada()
        {
            Actualizar();
        }

        private async void Guardar(AgendaModel obj)
        {
            //obj.Editado = false;
            await cliente.Put(obj);
            OnPropertyChanged();

        }

        private async void Editar()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await cliente.Put(Agenda);
                toast.ShowNotification("¡La actividad se ha actualizado con exito!");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Atención 🚫", "Acceda a una red wifi para poder realizar la operación", "OK");
            }

           
            OnPropertyChanged();

        }

        private async void Eliminar(object a)
        {
            Agenda = a as AgendaModel;
            bool resultado = await App.Current.MainPage.DisplayAlert("⚠️ Atención",
            $"¿Está seguro de eliminar la actividad: {agenda.Actividad}? ", "Sí", "No");
            if (resultado == true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await cliente.Delete(Agenda);
                    toast.ShowNotification("¡La actividad se ha eliminado con exito!");

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atención 🚫", "Acceda a una red wifi para poder realizar la operación", "OK");
                }
               
            }
            else
            {
                Cancelar();
            }

        }

        HttpClientHelper<AgendaModel> cliente;

        private async void Enviar()
        {
            //SignalR

           
                if (!(string.IsNullOrWhiteSpace(agenda.Actividad) &&
                string.IsNullOrWhiteSpace(agenda.FechaDeRealizar) && string.IsNullOrWhiteSpace(agenda.Importancia))) //No hacer nada si esta vacio el texto
                {


                   

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await cliente.Post(Agenda);
                    Agenda = null;
                   
                    OnPropertyChanged();
                    toast.ShowNotification("¡La actividad se ha agregado con exito!");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atención 🚫", "Acceda a una red wifi para poder realizar la operación", "OK");
                }
           




                   

                }
            
           
        }
        ~AgendaViewModel()
        {
            //Desuscripcion
            App.BdActualizada -= App_BdActualizada;
        }

       

        void Actualizar()
        {
            ListaAgendas.Clear();
            foreach (var item in Repository.GetAll())
            {
                ListaAgendas.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(property));
        }
    }
}
