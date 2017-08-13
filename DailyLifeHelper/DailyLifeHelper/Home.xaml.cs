using DailyLifeHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabpage : ContentPage
    {
        public HomeTabpage()
        {
            InitializeComponent();
            // logon_username.Text = "Welcome " + App.sysusername + " !";
        }
        private async void CallAPIButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new todo());
        }

        async void todo_image_tapped(object sender, EventArgs args)
        {
            // var imageSender = (Image)sender;
            await Navigation.PushAsync(new todo());
        }
        void navigation_image_tapped(object sender, EventArgs args)
        {
            // var imageSender = (Image)sender;
            DisplayAlert("Alert", "Image tapped test", "OK");
        }

        async void photo_image_tapped(object sender, EventArgs args)
        {
            //  var imageSender = (Image)sender;
            await Navigation.PushAsync(new takephoto());
        }

        async void weather_tapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new WeatherbyZIP());
        }

        async void forecast_tapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new forecaset());
        }
        async void GPS1_tapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Map_External());
        }
        async void deveoping(object sender, EventArgs args)
        {
            // var imageSender = (Image)sender;
            await DisplayAlert("Alert", "Under developing, please wait", "OK");
        }
        async void photototext(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PhotoToText());
        }

        async void getlanguage(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new checklanguage());
        }


        async void googletextdetect(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Google_TextDetect());
        }

        async void googletranslateit(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Google_translate());
        }
    }
}