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
        enum VertexState
        {
            None,
            Selected,
            Root
        };

        static Dictionary<VertexState, string> VertexColors = new Dictionary<VertexState, string>()
        {
            { VertexState.None, "Black" },
            { VertexState.Root, "Gold" },
            { VertexState.Selected, "Blue" }
        };

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
        bool isRoot;
        bool isSelected;
        public bool Selected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                ResetColor();
            }
        }
        public bool IsRoot
        {
            get
            {
                return isRoot;
            }
            set
            {
                isRoot = value;
                ResetColor();
            }
        }
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

        public CustomVertex()
        {
            ID = 0;
            Init();
        }

        public CustomVertex(int id)
        {
            ID = id;
            Init();
        }

        void Init()
        {
            isSelected = false;
            isRoot = false;
            ResetColor();
        }

        public void ResetColor()
        {
            if (isSelected)
                Color = VertexColors[VertexState.Selected];
            else if (isRoot)
                Color = VertexColors[VertexState.Root];
            else
                Color = VertexColors[VertexState.None];
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
