using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using GraphProject.SettingWindow;

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

        private void OpenFile_Click(object sender, RoutedEventArgs e)
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

        private void SaveFile_Click(object sender, RoutedEventArgs e)
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

        private void MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow();
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
