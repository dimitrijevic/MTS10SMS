using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MTS10SMS.Droid;

namespace MTS10SMS.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
        Icon = "@drawable/icon",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            System.Threading.Thread.Sleep(1000);
            StartActivity(typeof (MainActivity));
            // Create your application here
        }
    }
}