using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

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

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    List<int> pruferCode = File.ReadAllText(openFileDialog.FileName).Split(' ', ',', ';').Select(int.Parse).ToList();
                    vm.Graph = GraphBuilder.GraphBuilderStrategy.CodeToGraph(pruferCode);
                }
                catch
                {
                    MessageBox.Show(this, "File not found or has invalid format", "Error");
                    return;
                }
            }
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialogFile = new SaveFileDialog();
            if (saveDialogFile.ShowDialog() == true)
            {
                string a = string.Join(" ", GraphBuilder.GraphBuilderStrategy.GraphToCode(vm.Graph).ToArray());
                if (a.Length == 0)
                {
                    MessageBox.Show(this, "Nothing to save", "Error");
                    return;
                }
                try
                {
                    StreamWriter file = new StreamWriter(saveDialogFile.FileName);
                    file.WriteLine(a);
                    file.Close();
                }
                catch
                {
                    MessageBox.Show(this, "Can not save graph to file: \n" + saveDialogFile.FileName, "Error");
                }
            }
        }
    }
}
