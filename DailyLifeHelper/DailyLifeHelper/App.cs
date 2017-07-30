using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DailyLifeHelper
{
    public class App : Application

    {

        public static bool IsUserLoggedIn { get; set; }

        public static string sysusername { get; set; }

        public App()
        {
            // The root page of your application
            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {

                MainPage = new NavigationPage(new Home());
              //  MainPage = new TabbedPage
              //  {
                   // Children =
                   // {
                       // new todo(),
                      //  new Page1()

                   // }

              //  };
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    internal class Home : Page
    {
    }
}
