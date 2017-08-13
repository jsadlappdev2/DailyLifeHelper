using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.Models;
using DailyLifeHelper.DataService;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Plugin.TextToSpeech;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;
//using Google.Cloud.Vision.V1;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Google_translate : ContentPage
    {
        DataService.googleapiservice googleapiservice;
        List<googleapiservice.GoogleTranSource> source;
        public Google_translate()
        {
            InitializeComponent();
            googleapiservice = new DataService.googleapiservice();
        }

        async void translateit(object sender, EventArgs e)
        {
            try
            {
                //get source text and target language

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {
                    q = sourcetext.Text.Trim(),
                    target = language.Text.Trim()

                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");
                resultlabel.Text = result;



            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }
        }
            

      







       
    }
}