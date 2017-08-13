using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.Models;
using DailyLifeHelper.DataService;
using Plugin.TextToSpeech;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class forecaset : ContentPage
    {
        DataService.WeatherService weatherservice { get; } = new WeatherService();

        WeatherRoot weatherRoot = null;

        WeatherForecastRoot forecast;
        public WeatherForecastRoot Forecast
        {
            get { return forecast; }
            set { forecast = value; OnPropertyChanged(); }
        }

        public forecaset()
        {
            InitializeComponent();
            weatherservice = new DataService.WeatherService();
        }

        public async void GetFCWeather_Clicked(object sender, EventArgs eee)
        {

            try
            {
                //Get cityid
                weatherRoot = await weatherservice.GetWeather(zipCodeEntry2.Text.ToString());
               // msg1.Text = weatherRoot.CityId.ToString();
               //get forcast data
                Forecast = await weatherservice.GetForecast(weatherRoot.CityId);
                ListViewWeather.ItemsSource = Forecast.Items;
            }
            catch (Exception ee)
            {

                await DisplayAlert("Alert", "Get Forecast Weather failed and Error is: "+ee.Message.ToString(), "OK");
            }
          
        }


        public async void speak(object sender, EventArgs e)
        {
            //get temp and date for selected row
            var mi = ((MenuItem)sender);
            WeatherRoot selectweather = (WeatherRoot)mi.CommandParameter;
            string speak_weather = selectweather.DisplayDate + " " +selectweather.DisplayTemp;
            //speak
            CrossTextToSpeech.Current.Speak(speak_weather);
        }
    }
}