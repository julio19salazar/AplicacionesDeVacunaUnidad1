using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Firebase.Messaging;
using MiAgenda.Models;
using MiAgenda.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiAgenda.Droid
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public   class ServicioMensajes: FirebaseMessagingService
    {

       public override void OnMessageReceived(RemoteMessage p0)
        {

          
            try
            {

                AgendaRepository repository =
                    new AgendaRepository();
                AgendaModel agenda = null;
                if (p0.Data != null)
                {
                    var datos = p0.Data;

                    if (datos["Accion"] == "Agregar")
                    {


                        agenda = new AgendaModel()
                        {



                            Actividad = p0.Data["Actividad"],
                            Id = int.Parse(datos["Id"]),
                            FechaDeRealizar = datos["FechaDeRealizar"],
                          Importancia = datos["Importancia"]
                        };
                        var x = agenda;
                        var s = x;
                        repository.Insert(agenda);

                    }
                     if (datos["Accion"] == "Eliminar")
                    {

                     

                        var id = int.Parse(datos["Id"]);
                        var m = repository.Get(id);

                        if (m != null)
                        {
          
                            repository.Delete(m);
                        }
                    }
                     if (datos["Accion"] == "Editar")
                    {
                     

                        var id = int.Parse(datos["Id"]);
                        var m = repository.Get(id);

                        if (m != null)
                        {
                            m.Actividad = p0.Data["Actividad"];
                            m.FechaDeRealizar = datos["FechaDeRealizar"];
                            m.Importancia = datos["Importancia"];
                            repository.Update(m);
                        }
                    }

                    if (App.Current == null)
                    {
                        if (agenda != null)
                        {
                            ShowNotification(agenda.Id, agenda.Actividad, agenda.FechaDeRealizar);
                        }
                    }
                    else
                    {
                        App.Actualizar();
                    }
                }

            }
            catch (Exception ex)
            {
                var x = ex.Message;
               
            }

           
            base.OnMessageReceived(p0);
        }


        public void ShowNotification(int id, string title, string text)
        {


            NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context,
                "CANALCHAT")
                .SetContentTitle(title)
                .SetContentText(text)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetShowWhen(true)
                .SetSmallIcon(Resource.Drawable.ic_mtrl_chip_checked_circle);

            NotificationManager manager = Application.Context.GetSystemService(Application.NotificationService)
                as NotificationManager;

            manager.Notify(id, builder.Build());


        }
    }
}