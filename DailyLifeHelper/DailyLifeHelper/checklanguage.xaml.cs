using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;

namespace DailyLifeHelper
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class checklanguage : ContentPage
	{
        static CrossLocale? locale = null;
        public checklanguage()
        {
            InitializeComponent();
            sliderPitch.Maximum = 1.0f;
            sliderPitch.Minimum = 0f;
            sliderPitch.Value = 0.5f;

            sliderRate.Maximum = 2.0f;
            sliderRate.Minimum = 0f;
            sliderRate.Value = 1.0f;


            sliderVolume.Maximum = 1.0f;
            sliderVolume.Minimum = 0f;
            sliderVolume.Value = 1.0f;
        }

        async void checkbtn_Clicked(object sender, EventArgs e)
        {

            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            var items = locales.Select(a => a.ToString()).ToArray();


            var selected = await DisplayActionSheet("Language", "OK", null, items);
            if (string.IsNullOrWhiteSpace(selected) || selected == "OK")
                return;
            //      languageButton.Text = selected;

            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == selected);
            else
                locale = new CrossLocale { Language = selected };//fine for iOS/WP

            //list selected language
            langlabel.Text = locale.ToString();



        }

        async void speakbtn_Clicked(object sender, EventArgs e)
        {
            //use default
            if (useDefaults.IsToggled)
            {
                CrossTextToSpeech.Current.Speak(textlabel.Text.ToString());
                return;
            }

            CrossTextToSpeech.Current.Speak(textlabel.Text.ToString(),
                     pitch: (float)sliderPitch.Value,
                    speakRate: (float)sliderRate.Value,
                    volume: (float)sliderVolume.Value,
                    crossLocale: locale);



        }





    }
}