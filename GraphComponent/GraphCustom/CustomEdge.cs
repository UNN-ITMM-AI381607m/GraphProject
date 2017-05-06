using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using System.ComponentModel;

namespace GraphComponent
{
    public class CustomEdge : Edge<CustomVertex>, INotifyPropertyChanged
    {
        public string ID
        {
            get;
            private set;
        }

        public CustomEdge(CustomVertex source, CustomVertex target) : base(source, target)
        {
            ID = source.ID + " - " + target.ID;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}
