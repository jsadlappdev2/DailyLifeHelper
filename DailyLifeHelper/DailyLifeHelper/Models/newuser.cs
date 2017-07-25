using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLifeHelper.Models
{
    public class newuser
    {
        public int userid { get; set; }
        public string username { get; set; }

        public string email { get; set; }
        public string password { get; set; }

        public DateTime create_date { get; set; }

        public DateTime unvalid_date { get; set; }
        public bool valid_flag { get; set; }

    }
}
