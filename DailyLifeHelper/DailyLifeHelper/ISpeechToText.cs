using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLifeHelper
{
    public interface ISpeechToText
    {
        void Start();
        void Stop();
        event EventHandler<EventArgsVoiceRecognition> textChanged;
    }
}
