using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLifeHelper.Models
{
    public class Image
    {
        public string content { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
    }

    public class Request
    {
        public Image image { get; set; }
        public List<Feature> features { get; set; }
    }

    public class RootObject_GoogleTextDetect
    {
        public List<Request> requests { get; set; }
    }
}
