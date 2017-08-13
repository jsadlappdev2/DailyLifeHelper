using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DailyLifeHelper.Models
{
    public class myWeather
    {
        public string Title { get; set; }
        public string Temperature { get; set; }
        public string Temp_Min { get; set; }
        public string Temp_Max { get; set; }
        public string Wind { get; set; }
        public string Humidity { get; set; }
        public string Visibility { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public string Icon { get; set; }


        public myWeather()
        {
            //Because labels bind to these values, set them to an empty string to
            //ensure that the label appears on all platforms by default.
            this.Title = " ";
            this.Temperature = " ";
            this.Temp_Max = " ";
            this.Temp_Min = " ";
            this.Wind = " ";
            this.Humidity = " ";
            this.Visibility = " ";
            this.Sunrise = " ";
            this.Sunset = " ";
            this.Icon = " ";
        }
    }


    public class myforecastWeather
    {
        public string Title { get; set; }
        public string Temperature { get; set; }
        public string Temp_Min { get; set; }
        public string Temp_Max { get; set; }
        public string Wind { get; set; }
        public string Humidity { get; set; }
        public string Visibility { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public string Icon { get; set; }
        public string FCDate { get; set; } 


        public myforecastWeather()
        {
            //Because labels bind to these values, set them to an empty string to
            //ensure that the label appears on all platforms by default.
            this.Title = " ";
            this.Temperature = " ";
            this.Temp_Max = " ";
            this.Temp_Min = " ";
            this.Wind = " ";
            this.Humidity = " ";
            this.Visibility = " ";
            this.Sunrise = " ";
            this.Sunset = " ";
            this.Icon = " ";
            this.FCDate = " ";
        }
    }
}