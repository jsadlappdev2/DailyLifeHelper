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
            try
            {
                await dataService.AddNewUserAsync(addnewuser);
            }
            catch (Exception ex)
            {
                messageLabel.Text = "error: " + ex.Message.ToString();
            }
        }
}
}