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
        private CustomVertex firstChosen;
        public MainWindow()
        {
            firstChosen = null;
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        private void Label_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                CustomVertex chosen = ((sender as Label).Content as CustomVertex);
                Point cursor = e.GetPosition(this);
                CustomVertex newOne = new CustomVertex(vm.ID_counter);
                vm.AddNewVertex(newOne);
                vm.AddNewGraphEdge(chosen, newOne);
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (firstChosen == null)
            {
                firstChosen = (sender as Label).Content as CustomVertex;
                firstChosen.Color = "Blue";
            }
            else
            {
                CustomVertex second = ((sender as Label).Content as CustomVertex);
                if (second != firstChosen)
                {
                    vm.AddNewGraphEdge(firstChosen, second);
                }
                firstChosen.Color = "Black";
                firstChosen = null;
            }
        }
    }
}
