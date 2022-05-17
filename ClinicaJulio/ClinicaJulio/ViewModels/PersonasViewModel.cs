using ClinicaJulio.Models;
using ClinicaJulio.Services;
using ClinicaJulio.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ClinicaJulio.ViewModels
{
    public class PersonasViewModel : INotifyPropertyChanged
    {
        private string nombredelapersonaabuscar="";

        public string NombreDeLaPersonaABuscar
        {
            get { return nombredelapersonaabuscar; }
            set { nombredelapersonaabuscar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NombreDeLaPersonaABuscar)));
               
            }
        }

        AgregarPersonaView VistaAgregarPersona;
        EditarPersonaView VistaEditarPersona;
        public ICommand    BuscarPersonaCommand { get; set; }
        public ICommand    RegresarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        private List<ErrorModel> errores;

        public List<ErrorModel> Errores
        {
            get { return errores; }
            set
            {
                errores = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errores)));
            }
        }

        private Personas persona;

        public Personas Persona
        {
            get { return persona; }
            set { persona = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Persona)));
            }
        }
        public ObservableCollection<Personas> ListaPersonas { get; set; } = new ObservableCollection<Personas>();

       
        public PersonasViewModel()
        {
            
            VerAgregarCommand = new Command(VerAgregar);
            AgregarCommand = new Command(Agregar);
            VerEditarCommand = new Command(VerEditar);
            EditarCommand = new Command(Editar);
            EliminarCommand = new Command(Eliminar);
            CancelarCommand = new Command(Cancelar);
            BuscarPersonaCommand = new Command(BuscarPersona);
            RegresarCommand = new Command(Regresar);


            SincronizadorServices.ActualizacionRealizada += SincronizadorServices_ActualizacionRealizada;
            SincronizadorServices_ActualizacionRealizada();
        }

        private  void Regresar()
        {
            nombredelapersonaabuscar = "";
            var ps = App.Catalogo.GetAll();
            ListaPersonas.Clear();
            foreach (var item in ps)
            {
                ListaPersonas.Add(item);
            }
        }

        private async  void BuscarPersona()
        {
           var p=  App.Catalogo.GetPersonaById(NombreDeLaPersonaABuscar.ToUpper());
            if (p.Count()>=1)
            {
                ListaPersonas.Clear();
                foreach (var item in p)
                {
                    ListaPersonas.Add(item);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Atención 🚫", $"No se encontraron resultados mediante '{NombreDeLaPersonaABuscar}'", "OK");
                
            }
          
          
            
        }


        
        private async void Editar()
        {
            var result = await App.Sincronizador.Editar(Persona);

            if (result == null)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Errores = result.Select(x => new ErrorModel { Mensaje = x }).ToList();
            }
            //var result = App.Sincronizador.Editar(Persona);
            //if (result==null)
            //{
            //    await App.Current.MainPage.Navigation.PopAsync();
            //}
            //else
            //{
            //    Errores = result.Select(x => new ErrorModel { Mensaje = x }).ToList();
            //}
        }

        private async void VerEditar(object obj)
        {
         
                Persona = obj as Personas;
                if (VistaEditarPersona == null)
                {
                    VistaEditarPersona = new EditarPersonaView() { BindingContext = this };


                }
                Errores = null;
                await App.Current.MainPage.Navigation.PushAsync(VistaEditarPersona);


           

        }

        private void SincronizadorServices_ActualizacionRealizada()
        {
            var contactos = App.Catalogo.GetAll().ToList();

            //Mezclar con el buffer

            foreach (var ce in App.Sincronizador.Buffer)
            {
                if (ce.Estado == Estado.Agregado)
                {
                    contactos.Add(ce.Personas);
                }
                else if (ce.Estado == Estado.Modificado)
                {
                    var c = contactos.FirstOrDefault(x => x.Id == ce.Personas.Id);
                    if (c != null)
                    {
                        c.Nombre = ce.Personas.Nombre;
                        c.Edad = ce.Personas.Edad;
                        c.Sexo = ce.Personas.Sexo;
                        c.Vacuna = ce.Personas.Vacuna;
                        c.Lote = ce.Personas.Lote;
                    }
                }
                else
                {
                    var c = contactos.FirstOrDefault(x => x.Id == ce.Personas.Id);
                    if (c != null)
                    {
                        contactos.Remove(c);
                    }
                }
            }

            ListaPersonas.Clear();
            foreach (var item in contactos.OrderBy(x => x.Nombre))
            {
                ListaPersonas.Add(item);
            }
        }

       
        private async void Cancelar()
        {
            Persona = null;
            Errores = null;
            await App.Current.MainPage.Navigation.PopAsync();
            VistaAgregarPersona = null;
            VistaEditarPersona = null;
        }

        private async void VerAgregar()
        {
            if (VistaAgregarPersona == null)
            {
                VistaAgregarPersona = new AgregarPersonaView() { BindingContext = this };

            }
            Persona = new Personas();
            await App.Current.MainPage.Navigation.PushAsync(VistaAgregarPersona);
        }
        private async void Agregar()
        {
            var result = await App.Sincronizador.Agregar(Persona);

            if (result == null)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Errores = result.Select(x => new ErrorModel { Mensaje = x }).ToList();
            }
        }
        private async void Eliminar(object p)
        {
            Persona = p as Personas;
            bool resultado = await App.Current.MainPage.DisplayAlert("⚠️ Atención",
            $"¿Está seguro de eliminar el registro de: {Persona.Nombre}? ", "Sí", "No");
            if (resultado==true)
            {
                var result = await App.Sincronizador.Eliminar(Persona);
                if (result!=null)
                {
                    var message = string.Join(Environment.NewLine, result);

                    await Application.Current.MainPage.DisplayAlert("Atención 🚫", $"{message}", "OK");
                }

            }
            else
            {
                Cancelar();
            }

        }

        //private void AlCargarLaPagina()
        //{
        //    var contactos = App.Catalogo.GetAll();
        //   ListaPersonas.Clear();
        //    foreach (var item in contactos)
        //    {
        //        ListaPersonas.Add(item);
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
