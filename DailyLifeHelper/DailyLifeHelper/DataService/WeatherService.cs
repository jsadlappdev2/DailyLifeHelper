using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyLifeHelper.Models;
using System.Net.Http;
using static Newtonsoft.Json.JsonConvert;

namespace DailyLifeHelper.DataService
{

  
    public class WeatherService
    {
        const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0},au&units=metric&appid=0a89bd38420492ff150a74711822a6ff";
        const string ForecaseUri = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units=metric&appid=0a89bd38420492ff150a74711822a6ff";

        public async Task<WeatherRoot> GetWeather(string cityname)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCityUri, cityname);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
            }

        }

        public async Task<WeatherForecastRoot> GetForecast(int cityid)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ForecaseUri, cityid);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherForecastRoot>(json);
            }

        }
    }
}
