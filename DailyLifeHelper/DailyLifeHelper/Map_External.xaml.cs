using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.ExternalMaps;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map_External : ContentPage
    {
        public Map_External()
        {
            InitializeComponent();
        }

        async void GoBtn_Clicked(object sender, System.EventArgs e)
        {
            /// <summary>
            /// Navigate to an address
            /// </summary>
            /// <param name="name">Label to display</param>
            /// <param name="street">Street</param>
            /// <param name="city">City</param>
            /// <param name="state">Sate</param>
            /// <param name="zip">Zip</param>
            /// <param name="country">Country</param>
            /// <param name="countryCode">Country Code if applicable</param>
            /// <param name="navigationType">Navigation type</param>
            ///Task<bool> NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default);

            try
            {
                var success = await CrossExternalMaps.Current.NavigateTo("Another Home", "33 Devereux Road", "Linden Park", "SA", "5065", "AU", "AU");
                //msg.Text = success.ToString();
            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Error: "+ee.Message.ToString(), "OK");
                
            }
        }
        }
}