using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using GraphComponent.GraphBuilder;
using GraphComponent.SettingWindow;
using GraphComponent;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //GraphPane popupmenu event handlers
            GraphView.On_MenuItem_ChangeID += MenuItem_ChangeID_Click;
        }

        private void ConstructByPrufer(string str)
        {
            try
            {
                List<int> pruferCode = str.Split(' ', ',', ';').Select(int.Parse).ToList();
                GraphView.Graph = GraphBuilderStrategy.CodeToGraph(pruferCode);
            }
            catch
            {
                MessageBox.Show(this, "Invalid Prufer code", "Error");
                return;
            }
        }

        //File menu handlers
        private void OpenFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ConstructByPrufer(File.ReadAllText(openFileDialog.FileName));
                PruferResult.Content = "";
            }
        }

        private void SaveFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialogFile = new SaveFileDialog();
            saveDialogFile.Filter = "Text Files | *.txt";
            if (saveDialogFile.ShowDialog() == true)
            {
                string a = string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Graph).ToArray());
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
                    MessageBox.Show("Can not save graph to file: \n" + saveDialogFile.FileName, "Error");
                }
            }
        }


        private void Close_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Close();
        }

        //Toolbar handlers
        private void NewVertex_OnClick(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow("Create New Vertex", "Enter number for new vertex: ", "Create")
            {
                Owner = this
            };
            popup.ShowDialog();
            GraphView.AddNewVertex(popup.NewID);
        }

        private void NewEdge_OnClick(object sender, RoutedEventArgs e)
        {
        }

        //Workflow handlers  
        private void ConstructByPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            ConstructByPrufer(PruferTextBox.Text.ToString());
            PruferResult.Content = "";
        }


        private void GetPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            bool isEmpty = GraphView.Graph.IsVerticesEmpty;
            PruferResult.Content = isEmpty ? "" : "Generated Prufer Code: " + string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Graph).ToArray());
        }

        private void Numerate_OnClick(object sender, RoutedEventArgs e)
        {
            GraphView.Graph = Numerator.Renumber(GraphView.Graph);
        }

        //GraphPane PopupWindow handlers
        private int MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow("Change Number", "Enter new number: ", "Save")
            {
                Owner = this
            };
 
            popup.ShowDialog();
            return popup.NewID;
        }
    }
}
