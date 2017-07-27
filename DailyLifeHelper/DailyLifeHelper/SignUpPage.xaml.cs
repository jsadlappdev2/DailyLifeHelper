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
    public partial class SignUpPage : ContentPage
    {
        DataService.DataService dataService;
        public SignUpPage()
        {
            InitializeComponent();
            dataService = new DataService.DataService();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
             newuser addnewuser = new newuser
            {
                username = usernameEntry.Text.Trim(),
                email = emailEntry.Text.Trim(),
                password = passwordEntry.Text.Trim()
             };
            var signUpSucceeded = AreDetailsValid(addnewuser);
            if (signUpSucceeded)
            {
                try
                {
                    string msg = "";
                    int result = 0;
                    result = await dataService.AddNewUserAsync(addnewuser);
                    switch (result)
                    {
                        case 1:
                            msg = "Username has existed, please try another name!";
                            break;
                        case 2:
                            msg = "New user has created and please login!";
                            break;
                        default:
                            msg = "New user created failed and please try again!";
                            break;

                    }
                    messageLabel.Text = msg;
                }
                catch (Exception ex)
                {
                    messageLabel.Text = "New user create error: " + ex.Message.ToString();
                }
            }
            else
            {

                messageLabel.Text = "Please check your entry information!";

            }
        }

        bool AreDetailsValid(newuser user)
        {
            return (!string.IsNullOrWhiteSpace(user.username) && !string.IsNullOrWhiteSpace(user.password) && !string.IsNullOrWhiteSpace(user.email) && user.email.Contains("@"));
        }
    }
}