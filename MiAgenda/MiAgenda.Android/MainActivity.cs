using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase.Messaging;

namespace MiAgenda.Droid
{
    [Activity(Label = "MiAgenda", Icon = "@drawable/agenda", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Activity CurrentActivity { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {

            CurrentActivity = this;
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            FirebaseMessaging.Instance.SubscribeToTopic("chat");

            NotificationManager manager = Application.Context.GetSystemService(Application.NotificationService)
                          as NotificationManager;
            manager.CreateNotificationChannel(new NotificationChannel("CANALCHAT", "Canal del Chat", NotificationImportance.Max));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}