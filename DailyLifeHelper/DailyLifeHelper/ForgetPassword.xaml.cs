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
                email_to = emailEntry.Text.Trim()  
               

             };
            try
            {
                int result = 0;
                result = await dataService.SendPasswordJustCheckemail(newemailuser);
                string msg = "";
                switch (result)
                {
                    case 2:
                        msg = "Username and password has sent, please check your email!";
                        break;
                    case 3:
                        msg = "Username and password has sent, please check your email!";
                        break;
                    case 5:
                        msg = "Your input email is not registered email, cannot send you password!";
                        break;
                       
                    default:
                        msg = "Send email failed, please try again!";
                        break;

                }
                messageLabel.Text = msg;
            }
            catch (Exception ex)
            {
                messageLabel.Text = "Send email failed with error: " + ex.Message.ToString();
            }


        }
    }
}