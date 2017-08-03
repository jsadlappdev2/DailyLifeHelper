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
            var imageSender = (Image)sender;
            await Navigation.PushAsync(new todo());
        }
        void navigation_image_tapped(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            DisplayAlert("Alert", "Image tapped test", "OK");
        }

        async void photo_image_tapped(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            await Navigation.PushAsync(new takephoto());
        }
    }
}