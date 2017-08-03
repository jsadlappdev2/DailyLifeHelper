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
using Amazon.Util;
using Amazon.S3.Transfer;


namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class takephoto : ContentPage
    {
        public takephoto()
        {
            InitializeComponent();
        }

         async void TakePictureButton_Clicked(object sender, EventArgs e)
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
                SaveToAlbum = true
               // Name = "test.jpg"
            });

            if (file == null)
                return;

            // await  DisplayAlert("File Location", file.Path, "OK");
            file_path.Text = file.Path;

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

            // await DisplayAlert("File Location", file.Path, "OK");

            file_path.Text = file.Path;
            


            Image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            

        }

        private async void UploadtoS3_Clicked(object sender, EventArgs e)
        {
            //get image name from file_path
            string filepath = file_path.Text.ToString();
            int index1 = filepath.LastIndexOf('/')+1;
            string filename = filepath.Substring(index1 );

          await  sendMyFileToS3(file_path.Text.ToString(), "awss3dailylifehelperapp","images", filename, "youraccesskey", "yoursecuritykey");

        }

        void GetImageFromS3_Clicked(object sender, EventArgs e)
        {

            Image2.Source = S3Url.Text.ToString();
        }






        async Task sendMyFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3, string youraccesskey, string yoursecuritykey)
        {
            // input explained :
            // localFilePath = we will use a file stream , instead of path
            // bucketName : the name of the bucket in S3 ,the bucket should be already created
            // subDirectoryInBucket : if this string is not empty the file will be uploaded to
            // a subdirectory with this name
            // fileNameInS3 = the file name in the S3
            // create an instance of IAmazonS3 class ,in my case i choose RegionEndpoint.EUWest1
            // you can change that to APNortheast1 , APSoutheast1 , APSoutheast2 , CNNorth1
            // SAEast1 , USEast1 , USGovCloudWest1 , USWest1 , USWest2 . this choice will not
            // store your file in a different cloud storage but (i think) it differ in performance
            // depending on your location


            // IAmazonS3 client = new AmazonS3Client("Your Access Key", "Your Secrete Key", Amazon.RegionEndpoint.USWest2);
            IAmazonS3 client = new AmazonS3Client(youraccesskey, yoursecuritykey, Amazon.RegionEndpoint.APSoutheast2);

            // create a TransferUtility instance passing it the IAmazonS3 created in the first step
            TransferUtility utility = new TransferUtility(client);
            // making a TransferUtilityUploadRequest instance
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
            {
                request.BucketName = bucketName; //no subdirectory just bucket name
            }
            else
            {   // subdirectory and bucket name
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }
            request.FilePath = localFilePath;
            request.Key = fileNameInS3; //file name up in S3
                                        //request.FilePath = localFilePath; //local file name
                                        //  request.InputStream = localFilePath;
            request.CannedACL = S3CannedACL.PublicReadWrite;
            try
            {
                await utility.UploadAsync(request); //commensing the transfer
                msg.Text = "File upload successfully!";
                S3Url.Text = "https://s3-ap-southeast-2.amazonaws.com/" + bucketName + "/" + subDirectoryInBucket + "/" + fileNameInS3;
            }
            catch (Exception err)
            {
                msg.Text = "Upload failed and Error: " + err.Message.ToString();

            }


        }

    }
}