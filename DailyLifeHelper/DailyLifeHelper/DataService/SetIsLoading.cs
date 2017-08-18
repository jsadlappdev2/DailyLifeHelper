using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLifeHelper.DataService
{
    public class SetIsLoading
    {
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                // NotifyPropertyChanged();
            }
        }

    }
}
