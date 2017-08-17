using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DailyLifeHelper
{
    public partial class SpeechToText : ContentPage
    {
        public delegate ContentPage GetEditorInstance(string InitialEditorText);
        static public GetEditorInstance EditorFactory;
        static ISpeechToText speechRecognitionInstnace;
        public SpeechToText()
        {

           
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                androidLayout.IsVisible = true;
                voiceButton.OnTextChanged += (s) =>
                {
                    textLabelDroid.Text = s;
                };
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                iOSLayout.IsVisible = true;
            this.Content = iOSLayout;
            speechRecognitionInstnace = DependencyService.Get<ISpeechToText>();
            speechRecognitionInstnace.textChanged += OnTextChange;

            }

        }

        public void OnStart(Object sender, EventArgs args)
        {
            speechRecognitionInstnace.Start();
            nameButtonStart.IsEnabled = false;
            nameButtonStop.IsEnabled = true;
        }
        public void OnStop(Object sender, EventArgs args)
        {
            speechRecognitionInstnace.Stop();
            nameButtonStart.IsEnabled = true;
            nameButtonStop.IsEnabled = false;

        }
        public void OnTextChange(object sender, EventArgsVoiceRecognition e)
        {
            textLabeliOS.Text = e.Text;
            if (e.IsFinal)
            {
                nameButtonStart.IsEnabled = true;
            }
        }
    }
}
