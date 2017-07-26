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
    public partial class ForgetPassword : ContentPage
    {
        DataService.DataService dataService;
        public ForgetPassword()
        {
            InitializeComponent();
            dataService = new DataService.DataService();
        }
        async void GetpasswordButtonClicked(object sender, EventArgs e)
        {
             EmailUser newemailuser = new EmailUser
            {
               userid = 1,
             email_to = emailEntry.Text.Trim()
            // email_to="shenjr81@gmail.com".ToString()

             };
              int result = 0;
            result= await dataService.SendPasswordAsync(newemailuser);
            messageLabel.Text = result.ToString();
      

        }
    }
}