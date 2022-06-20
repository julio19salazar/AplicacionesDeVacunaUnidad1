using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(MiAgenda.Droid.ClaseToast))]
namespace MiAgenda.Droid
{
   public class ClaseToast:IToast
    {
       

        public void ShowNotification(string mensaje)
        {
            var t = Toast.MakeText(Application.Context, mensaje, ToastLength.Long);
            t.Show();
        }
    }
}