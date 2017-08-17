using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;
using ImageCircle.Forms.Plugin.Droid;
using DailyLifeHelper;


namespace DailyLifeHelper.Droid
{
    [Activity(Label = "DailyLifeHelper.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public event EventHandler<ActivityResultEventArgs> ActivityResult = delegate { };
        protected override void OnCreate(Bundle bundle)
        {
            // TabLayoutResource = Resource.Layout.Tabbar;
            // ToolbarResource = Resource.Layout.Toolbar;

           

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResult(this, new ActivityResultEventArgs
            {
                RequestCode = requestCode,
                ResultCode = resultCode,
                Data = data
            });
        }
    }
}

