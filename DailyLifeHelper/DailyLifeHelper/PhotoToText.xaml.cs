using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.TextToSpeech;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;
using DailyLifeHelper.Models;

namespace DailyLifeHelper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoToText : ContentPage
    {
        public PhotoToText()
        {
            InitializeComponent();
        }
        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                Directory = "test",
                SaveToAlbum = true
                // Name = "test.jpg"
            });

            if (file == null)
                return;

            // await  DisplayAlert("File Location", file.Path, "OK");
          //  file_path.Text = file.Path;

            Image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

        }

        async void SelectPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Upload", ":( Picking a photo is not supported.", "OK");
                return;

            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            // await DisplayAlert("File Location", file.Path, "OK");

         //   file_path.Text = file.Path;



            Image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });


        }

        //read text from pick up photo

        async void GetText_Clicked(object sender, EventArgs e)
        {
            try
            {
                //1.pickup photo first
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No Upload", ":( Picking a photo is not supported.", "OK");
                    return;

                }
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                    return;

                var stream = file.GetStream();

                //2.Add  OCR logic.
                string subscriptionKey = "c514e88fbb2a47f382ae9c62581b2cd9";
                string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr";
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);                
                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                BinaryReader binaryReader = new BinaryReader(stream);
                byte[] byteData = binaryReader.ReadBytes((int)stream.Length);
                string final_test = "";
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data"//application/octet-stream
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);

                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();
                  
                    var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));
                  

                   

                    foreach (var region_objall in obj.regions)
                    {
                        foreach (var lines_objall in region_objall.lines)
                        {
                            foreach (var words_objall in lines_objall.words)
                            {

                                final_test += words_objall.text + " ";
                            }

                        }
                    }
                }

                    //3. process Text
                    TextLabel.Text = final_test.ToString();

            }
            catch (Exception ex)
            {
                await (Application.Current?.MainPage?.DisplayAlert("Error",
                    $"Something bad happened: {ex.Message}", "OK") ??
                    Task.FromResult(true));

              

            }

        }



        //read text from camera photo

        async void GetTexttakephoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                //1.take photo first
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {

                    Directory = "test",
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Custom,
                    CustomPhotoSize = 30 //Resize to 90% of original
                    // Name = "test.jpg"
                });

                if (file == null)
                    return;

                var stream = file.GetStream();

                //2.Add  OCR logic.
                string subscriptionKey = "c514e88fbb2a47f382ae9c62581b2cd9";
                string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr";
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                BinaryReader binaryReader = new BinaryReader(stream);
                byte[] byteData = binaryReader.ReadBytes((int)stream.Length);
                string final_test = "";
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data"//application/octet-stream
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);

                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();

                    var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));




                    foreach (var region_objall in obj.regions)
                    {
                        foreach (var lines_objall in region_objall.lines)
                        {
                            foreach (var words_objall in lines_objall.words)
                            {

                                final_test += words_objall.text + " ";
                            }

                        }
                    }
                }

                //3. process Text
                TextLabel.Text = final_test.ToString();

            }
            catch (Exception ex)
            {
                await (Application.Current?.MainPage?.DisplayAlert("Error",
                    $"Something bad happened: {ex.Message}", "OK") ??
                    Task.FromResult(true));



            }

        }


        async void ReadText_Clicked(object sender, EventArgs e)
        {

            CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
        }


        async void Querydictory_Clicked(object sender, EventArgs e)
        {

            await DisplayAlert("Alert", "Under developing, please wait", "OK");
        }


        async void Translate_Clicked(object sender, EventArgs e)
        {

            await DisplayAlert("Alert", "Under developing, please wait", "OK");
        }






        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }

    }
}