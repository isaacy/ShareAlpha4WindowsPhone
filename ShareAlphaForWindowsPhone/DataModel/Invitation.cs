using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareAlpha.DataModel
{
    public class Invitation : INotifyPropertyChanged
    {
        public Invitation()
        {

        }

        private string alphaName;

        public string AlphaName
        {
            get { return alphaName; }
            set { alphaName = value; NotifyPropertyChanged("AlphaName"); }
        }

        private string location;

        public string Location
        {
            get { return location; }
            set { location = value; NotifyPropertyChanged("Location"); }
        }


        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged("Message"); 
}
        }
        private DateTimeOffset date;

        public DateTimeOffset Date
        {
            get { return date; }
            set { date = value; NotifyPropertyChanged("Date"); }
        }

        private TimeSpan time;

        public TimeSpan Time
        {
            get { return time; }
            set { time = value; NotifyPropertyChanged("Time"); }
        }

        public void Clear()
        {
            message = string.Empty;
            date = DateTimeOffset.MinValue;
            time = TimeSpan.Zero;
            location = string.Empty;
            alphaName = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // NotifyPropertyChanged will fire the PropertyChanged event, 
        // passing the source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
