using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.DataService;
using DailyLifeHelper.Models;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        DataService.DataService dataService;
        public LoginPage()
        {
            InitializeComponent();
            dataService = new DataService.DataService();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text.Trim();
            string password = passwordEntry.Text.Trim();
            var signUpSucceeded = AreDetailsValid(username, password);
            if (signUpSucceeded) {

                try
                {
                    string msg = "";
                    int result = 0;
                    result = await dataService.ValidUser(username,password);
                    switch (result)
                    {
                        case 1:
                            msg = "Username doesn't exist, please try again!";
                            usernameEntry.Text = string.Empty;
                            passwordEntry.Text = string.Empty;
                            break;
                        case 2:
                            //  msg = "Great! User has been valided!!!!";
                            // break;
                            //user valid ok and go to Home page
                            App.IsUserLoggedIn = true;
                            Navigation.InsertPageBefore(new Home(), this);
                            await Navigation.PopAsync();
                            break;


                        default:
                            msg = "Validation error, please try again!";
                            usernameEntry.Text = string.Empty;
                            passwordEntry.Text = string.Empty;
                            break;

                    }
                    messageLabel.Text = msg;
                }
                catch (Exception ex)
                {
                    messageLabel.Text = "Validation process error: " + ex.Message.ToString();
                    usernameEntry.Text = string.Empty;
                    passwordEntry.Text = string.Empty;
                }


            }
            else
            {
                messageLabel.Text = "Please check your entry information!";
            }




            }
        async void ForgetpasswordButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgetPassword());
        }

        bool AreDetailsValid(string username, string password)
        {
            return (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) );
        }


    }
}