using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using GraphComponent.SettingWindow;

namespace GraphComponent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GraphPane : UserControl
    {
        private ViewModel vm;
        private CustomVertex selectedVertex;

        public delegate int PopupMenuDelegate(object sender, RoutedEventArgs e);

        //PopupMenuDelegate events
        public event PopupMenuDelegate On_MenuItem_ChangeID;


        public GraphPane()
        {
            selectedVertex = null;
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }


        public CustomGraph Graph
        {
            get { return vm.Graph; }
            set { vm.Graph = value; }
        }

        public void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedVertex == null)
            {
                selectedVertex = (sender as Label).Content as CustomVertex;
                selectedVertex.Color = "Blue";
            }
            else
            {
                CustomVertex second = ((sender as Label).Content as CustomVertex);
                if (second != selectedVertex)
                {
                    vm.AddNewGraphEdge(selectedVertex, second);
                }
                selectedVertex.Color = "Black";
                selectedVertex = null;
            }
        }

        private void MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            var result = On_MenuItem_ChangeID(sender, e);
            //WTFF
            vm.ChangeId((((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Label).Content as CustomVertex, result);
        }

        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmVertex") as ContextMenu;
            cm.PlacementTarget = sender as Label;
            cm.IsOpen = true;
        }

        public void AddNewVertex(int number)
        {
            vm.AddNewVertex(number);
        }

        private void MenuItem_DeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveVertex(((((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Label).Content as CustomVertex));
        }

        public void ChangeAlgo(string name)
        {
            graphLayout.LayoutAlgorithmType = name;
        }
    }
}
