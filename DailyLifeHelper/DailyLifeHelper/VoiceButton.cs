using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DailyLifeHelper
{
    
        public class VoiceButton : Button
        {
            public Action<string> OnTextChanged { get; set; }
        }
 
}
