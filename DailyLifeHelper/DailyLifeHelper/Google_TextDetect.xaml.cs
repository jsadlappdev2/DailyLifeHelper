using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DailyLifeHelper.Models;
using DailyLifeHelper.DataService;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Plugin.TextToSpeech;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;
//using Google.Cloud.Vision.V1;

namespace DailyLifeHelper
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Google_TextDetect : ContentPage
	{
        DataService.googleapiservice googleapiservice;

        public Google_TextDetect()
        {
            InitializeComponent();
            googleapiservice = new DataService.googleapiservice();
        }



        //Get image using camera 
        //Get text from image using google api
        //Read text

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
                    CustomPhotoSize = 20 //Resize to 90% of original
                    // Name = "test.jpg"
                });

                if (file == null)
                    return;

                var stream = file.GetStream();

                Image1.Source = ImageSource.FromStream(() =>
                {
                    var stream2 = file.GetStream();
                    file.Dispose();
                    return stream2;
                });

                //Google text dection api service
                RootObject_GoogleTextDetect newphoto = new RootObject_GoogleTextDetect
                {

                };

                //   await DataService.googleapiservice.GoogleDetectTextFromIMage(newphoto);


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


        //test
        async void GetTexttakephoto_test(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                result = await googleapiservice.GoogleDetectTextFromIMage();
                await DisplayAlert("Alert", "Google Detect Text API execute result: " + result.ToString(), "OK");
            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google Detect Text API execute ERROR: " + ee.Message.ToString(), "OK");

            }
        }


        async void ReadText_Clicked(object sender, EventArgs e)
        {

            CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
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