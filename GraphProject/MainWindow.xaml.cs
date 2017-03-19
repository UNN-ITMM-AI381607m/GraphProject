using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel vm;
        private CustomVertex selectedVertex;
        public MainWindow()
        {
            selectedVertex = null;
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        private void Label_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //TODO: move to context menu (maybe)
            //if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    CustomVertex chosen = ((sender as Label).Content as CustomVertex);
            //    Point cursor = e.GetPosition(this);
            //    CustomVertex newOne = new CustomVertex(vm.ID_counter);
            //    vm.AddNewVertex(newOne);
            //    vm.AddNewGraphEdge(chosen, newOne);
            //}
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
    }
}
