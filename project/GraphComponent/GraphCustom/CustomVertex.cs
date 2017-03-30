using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using System.ComponentModel;

namespace GraphComponent
{
    public class CustomVertex : INotifyPropertyChanged
    {
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }
        private string color;
        private int id;
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public CustomVertex() { }

        public CustomVertex(int id)
        {
            ID = id;
            color = "Black";
        }

        public override string ToString()
        {
            return string.Format("{0}", ID);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
