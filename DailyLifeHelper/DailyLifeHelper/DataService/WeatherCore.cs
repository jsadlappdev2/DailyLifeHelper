using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyLifeHelper.Models;

namespace DailyLifeHelper.DataService
{
   public class WeatherCore
    {
        public static async Task<myWeather> GetWeather(string zipCode)
        {
            //Sign up for a free API key at http://openweathermap.org/appid
            string key = "0a89bd38420492ff150a74711822a6ff";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?zip="
                + zipCode + ",au&units=metric&appid=" + key;

            var results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

            if (results["weather"] != null)
            {
                myWeather weather = new myWeather();
                weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " °C";
                weather.Temp_Max = (string)results["main"]["temp_max"] + " °C";
                weather.Temp_Min = (string)results["main"]["temp_min"] + " °C";
                weather.Wind = (string)results["wind"]["speed"] + " mph";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
                weather.Visibility = (string)results["weather"][0]["main"];

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                weather.Sunrise = sunrise.ToString() + " UTC";
                weather.Sunset = sunset.ToString() + " UTC";
                weather.Icon = "http://openweathermap.org/img/w/"+(string)results["weather"][0]["icon"]+".png";
                return weather;
            }
            else
            {
                return null;
            }
        }




        public static async Task<myforecastWeather> GetFCWeather(string zipCode)
        {
            //Sign up for a free API key at http://openweathermap.org/appid
            string key = "0a89bd38420492ff150a74711822a6ff";
            string queryString = "http://api.openweathermap.org/data/2.5/forecast?zip="
                + zipCode + ",au&units=metric&appid=" + key;

            var results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

            if (results["myforecastWeather"] != null)
            {
                myforecastWeather weather = new myforecastWeather();
               // weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " °C";
                weather.Temp_Max = (string)results["main"]["temp_max"] + " °C";
                weather.Temp_Min = (string)results["main"]["temp_min"] + " °C";
              //  weather.Wind = (string)results["wind"]["speed"] + " mph";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
             //   weather.Visibility = (string)results["weather"][0]["main"];

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
               // DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
               // DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                DateTime dt_txt= time.AddSeconds((double)results["dt_txt"]);
              //  weather.Sunrise = sunrise.ToString() + " UTC";
               // weather.Sunset = sunset.ToString() + " UTC";
                weather.Icon = "http://openweathermap.org/img/w/" + (string)results["weather"][0]["icon"] + ".png";
                weather.FCDate = dt_txt.ToString() + " UTC";
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}
