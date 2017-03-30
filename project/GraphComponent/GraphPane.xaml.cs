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
            PopupWindow popup = new PopupWindow();
           // popup.Owner = this;
            popup.ShowDialog();
            int result = popup.NewID;
            if (result == -1)
            {
                return;
            }

            //WTFF
            ((((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Label).Content as CustomVertex).ID = result;
        }

        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmVertex") as ContextMenu;
            cm.PlacementTarget = sender as Label;
            cm.IsOpen = true;
        }

        private void CloseFile_Click(object sender, RoutedEventArgs e)
        {
            vm.Graph = new CustomGraph();
        }

        private void MenuItem_NewVertex_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow();
           // popup.Owner = this;
            popup.ShowDialog();
            int result = popup.NewID;
            if (result == -1)
            {
                return;
            }

            vm.AddNewVertex(result);
        }
    }
}
