using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.Models;
using DailyLifeHelper.DataService;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class openweather : ContentPage
    {
        DataService.DataService dataService;
        List<openweather> openweatherinfo;
        public openweather()
        {
            InitializeComponent();
            dataService = new DataService.DataService();
        }
        async void QueryButton_Clicked()
        {
            try
            {
                openweatherinfo = await dataService.GetOpenweatherAsync(Postcode.Text.ToString());
                openweatherlistsview.ItemsSource = openweatherinfo;
                msg.Text = "Get weather success!";
            }
            catch (Exception e)
            {
                msg.Text = "Get weather error: " + e.Message.ToString();
            }
        }
    }
}