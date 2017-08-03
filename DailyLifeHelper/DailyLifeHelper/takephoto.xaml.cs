using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amazon.S3;
using System.IO;
using Amazon.S3.Model;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class takephoto : ContentPage
    {
        public takephoto()
        {
            InitializeComponent();
        }

        private async void TakePictureButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                Directory="test",
                SaveToAlbum = true,
                Name = "test.jpg"
            });

            if (file == null)
                return;

           await  DisplayAlert("File Location", file.Path, "OK");

            Image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });


        }
        private async void UploadPictureButton_Clicked(object sender, EventArgs e)
        {

            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Upload", ":( Picking a photo is not supported.", "OK");
                return;

            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            Image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

        }

        private async void UploadtoS3_Clicked(object sender, EventArgs e)
        {
            // IAmazonS3 client = new AmazonS3Client("Your Access Key", "Your Secrete Key", Amazon.RegionEndpoint.USWest2);
            // IAmazonS3 S3Client = new AmazonS3Client("AKIAJNPG6CK35BJNXTRQ", "i8N/N7okD0s5wH0AOEam3jjvojbtwX3k1C3bYy6p", Amazon.RegionEndpoint.APSoutheast2);
            // Create a client
            try
            {
                AmazonS3Client client = new AmazonS3Client("AKIAJNPG6CK35BJNXTRQ", "i8N/N7okD0s5wH0AOEam3jjvojbtwX3k1C3bYy6p", Amazon.RegionEndpoint.APSoutheast2);
                // Create a client

                // Create a PutObject request
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = "awss3dailylifehelperapp/images",
                    Key = "text",
                    FilePath = Image1.ToString()
                };

                // Put object
                PutObjectResponse response = await client.PutObjectAsync(request);
            }

            catch (Exception ee)
            {
                await DisplayAlert("Alert", ee.Message.ToString(), "OK");
            }

        }
    }
}