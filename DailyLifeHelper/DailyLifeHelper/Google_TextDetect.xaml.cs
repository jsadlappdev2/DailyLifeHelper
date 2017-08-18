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
using Plugin.TextToSpeech.Abstractions;

//using Google.Cloud.Vision.V1;

namespace DailyLifeHelper
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Google_TextDetect : ContentPage
	{
      
        static CrossLocale? locale = null;
        DataService.googleapiservice googleapiservice;
        List<googleapiservice.GoogleTranSource> source;

        DataService.SetIsLoading myisloading;

        public Google_TextDetect()
        {
            InitializeComponent();
            googleapiservice = new DataService.googleapiservice();
            myisloading = new DataService.SetIsLoading();
            myisloading.IsLoading = false;
        }



        //Get image using camera 
        //Get text from image using google api
        //Read text

        async void GetTexttakephoto_Clicked(object sender, EventArgs e)
        {
       
            if (indicator.IsRunning) return;
         

            try
            {

                string final_text = "";
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

                //Image1.Source = ImageSource.FromStream(() =>
                //{
                //    var stream2 = file.GetStream();
                //    file.Dispose();
                //    return stream2;
                //});

                //2.Google text dection api service

                try
                {
                    //  myisloading.IsLoading = true;
                    indicator.IsRunning = true;
                    indicator.IsVisible = true;
                    
                    //get image into base64string

                    BinaryReader binaryReader = new BinaryReader(stream);

                    byte[] byteData = binaryReader.ReadBytes((int)stream.Length);
                    string base64String = Convert.ToBase64String(byteData);
                    string image_base64string = base64String;

                    //generate json content string
                    string jsonstring_orig = @"{ 
                      
                     ""requests"": [
                           {
                                                 ""image"": {
                                                               ""content"": ""rep_image_base64string""
                                                            },

                                               ""features"": [
                                                            {
                                                              ""type"": ""TEXT_DETECTION""
                                                             }
                                                            ],
                                               ""imageContext"": { ""languageHints"": [""en"" ,""zh-CN"",""zh-TW"" ,""zh""]},


                            }
                    ]
                   }";
                    string jsonstring = jsonstring_orig.Replace("rep_image_base64string", image_base64string);

                    //new http client and call api
                    
                    HttpClient client = new HttpClient();
                    var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                    var url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js";
                    var response = await client.PostAsync(url, content);

                    var status = response.IsSuccessStatusCode;
                    if (status)
                    {

                        string contentString = await response.Content.ReadAsStringAsync();
                        string obj_descrpiton = "";
                        var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));

                        //Get description from textAnnotations
                        foreach (var obj_response in obj.responses)
                        {

                            foreach (var obj_text in obj_response.textAnnotations)
                            {
                                if (obj_text.locale == "en" || obj_text.locale == "zh" || obj_text.locale == "nl")
                                {

                                    obj_descrpiton += obj_text.description;
                                }

                            }



                        }

                        final_text = obj_descrpiton;

                       
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Google Detect Text API execute failed.", "OK"); ;

                    }


                }
                catch (Exception ee)
                {
                    await DisplayAlert("Alert", "Google Detect Text API execute failed: " + ee.Message.ToString(), "OK");
       
                }

                //3. process Text
                TextLabel.Text = final_text.ToString();

            }
            catch (Exception ex)
            {
                await (Application.Current?.MainPage?.DisplayAlert("Error",
                    $"Something bad happened: {ex.Message}", "OK") ??
                    Task.FromResult(true));



            }
            // myisloading.IsLoading = false;
            indicator.IsRunning = false;
            indicator.IsVisible = false;

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


        async void ReadChineseText_Clicked(object sender, EventArgs e)
        {
            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == "zh-CN");
            else
                locale = new CrossLocale { Language = "zh-CN" };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString(),
                                //   speakRate: (float)0.45,
                                   crossLocale: locale);
        }


        async void ReadEnglishText_Clicked(object sender, EventArgs e)
        {
            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == "en-US");
            else
                locale = new CrossLocale { Language = "en-US" };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString(),
                                 //speakRate: (float)0.45,
                                   crossLocale: locale);
        }


        async void TransChineseText_Clicked(object sender, EventArgs e)
        {
            try
            {
                //get source text and target language

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {
                    q = TextLabel.Text.Trim(),
                    target = "en"

                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");
                TransLabel.Text = result;



            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }
        }


        async void TransEnglishText_Clicked(object sender, EventArgs e)
        {
            try
            {
                //get source text and target language

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {
                    q = TextLabel.Text.Trim(),
                    target = "zh-CN"

                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");
                TransLabel.Text = result;



            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }
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



        public class Vertex
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingPoly
        {
            public List<Vertex> vertices { get; set; }
        }

        public class TextAnnotation
        {
            public string locale { get; set; }
            public string description { get; set; }
            public BoundingPoly boundingPoly { get; set; }
        }

        public class DetectedLanguage
        {
            public string languageCode { get; set; }
        }

        public class Property
        {
            public List<DetectedLanguage> detectedLanguages { get; set; }
        }

        public class DetectedLanguage2
        {
            public string languageCode { get; set; }
        }

        public class Property2
        {
            public List<DetectedLanguage2> detectedLanguages { get; set; }
        }

        public class Vertex2
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox
        {
            public List<Vertex2> vertices { get; set; }
        }

        public class DetectedLanguage3
        {
            public string languageCode { get; set; }
        }

        public class Property3
        {
            public List<DetectedLanguage3> detectedLanguages { get; set; }
        }

        public class Vertex3
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox2
        {
            public List<Vertex3> vertices { get; set; }
        }

        public class DetectedLanguage4
        {
            public string languageCode { get; set; }
        }

        public class Property4
        {
            public List<DetectedLanguage4> detectedLanguages { get; set; }
        }

        public class Vertex4
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox3
        {
            public List<Vertex4> vertices { get; set; }
        }

        public class DetectedLanguage5
        {
            public string languageCode { get; set; }
        }

        public class DetectedBreak
        {
            public string type { get; set; }
        }

        public class Property5
        {
            public List<DetectedLanguage5> detectedLanguages { get; set; }
            public DetectedBreak detectedBreak { get; set; }
        }

        public class Vertex5
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox4
        {
            public List<Vertex5> vertices { get; set; }
        }

        public class Symbol
        {
            public Property5 property { get; set; }
            public BoundingBox4 boundingBox { get; set; }
            public string text { get; set; }
        }

        public class Word
        {
            public Property4 property { get; set; }
            public BoundingBox3 boundingBox { get; set; }
            public List<Symbol> symbols { get; set; }
        }

        public class Paragraph
        {
            public Property3 property { get; set; }
            public BoundingBox2 boundingBox { get; set; }
            public List<Word> words { get; set; }
        }

        public class Block
        {
            public Property2 property { get; set; }
            public BoundingBox boundingBox { get; set; }
            public List<Paragraph> paragraphs { get; set; }
            public string blockType { get; set; }
        }

        public class Page
        {
            public Property property { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public List<Block> blocks { get; set; }
        }

        public class FullTextAnnotation
        {
            public List<Page> pages { get; set; }
            public string text { get; set; }
        }

        public class Respons
        {
            public List<TextAnnotation> textAnnotations { get; set; }
            public FullTextAnnotation fullTextAnnotation { get; set; }
        }

        public class RootObject
        {
            public List<Respons> responses { get; set; }
        }


    }
}