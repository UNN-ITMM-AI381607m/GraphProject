using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using GraphComponent.GraphBuilder;
using GraphComponent.SettingWindow;
using GraphComponent.PopupWindow;
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
            GraphView.ShowMessage += ShowMessage;
        }

        private void ShowMessage(string message, MessageBoxImage icon)
        {
            string title = "";
            switch (icon)
            {
                case MessageBoxImage.None:
                case MessageBoxImage.Question:
                    title = "Message";
                    break;
                case MessageBoxImage.Error:
                    title = "Error";
                    break;
                case MessageBoxImage.Warning:
                    title = "Warning";
                    break;
                case MessageBoxImage.Information:
                    title = "Information";
                    break;
                default:
                    break;
            }
            MessageBox.Show(this, message, title, MessageBoxButton.OK, icon);
        }

        private void ConstructByPrufer(string str)
        {
            try
            {
                List<int> pruferCode = str.Split(' ', ',', ';').Select(int.Parse).ToList();
                GraphView.Tree = GraphBuilderStrategy.CodeToGraph(pruferCode);
                UpdateLayoutThroughViewModel();
            }
            catch
            {
                ShowMessage("Invalid Prufer code", MessageBoxImage.Error);
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

        void UpdateLayoutThroughViewModel()
        {
            (GraphView.DataContext as ViewModel).UpdateLayout();
        }

        private void SaveFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialogFile = new SaveFileDialog()
            {
                Filter = "Text Files | *.txt"
            };
            if (saveDialogFile.ShowDialog() == true)
            {
                string a = string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Tree).ToArray());
                if (a.Length == 0)
                {
                    ShowMessage("Nothing to save", MessageBoxImage.Error);
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
                    ShowMessage("Can not save graph to file: \n" + saveDialogFile.FileName, MessageBoxImage.Error);
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
            PopupWindowWith2Boxes popup = new PopupWindowWith2Boxes("Create New Edge", "Enter id two vertices you want to merge: ", "Join")
            {
                Owner = this
            };
            popup.ShowDialog();
            GraphView.AddNewEdge(popup.ID1, popup.ID2);
            UpdateLayoutThroughViewModel();
        }

        //Workflow handlers  
        private void ConstructByPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            ConstructByPrufer(PruferTextBox.Text.ToString());
            PruferResult.Content = "";
        }

        private void GetPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckTreeStatus())
                return;

            bool isEmpty = GraphView.Tree.IsVerticesEmpty;
            PruferResult.Content = isEmpty ? "" : "Generated Prufer Code: " + string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Tree).ToArray());
        }

        private void Numerate_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckTreeStatus())
                return;

            GraphView.Tree = Numerator.Renumber(GraphView.Tree);
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateLayoutThroughViewModel();
        }

        private void CheckTree_Click(object sender, RoutedEventArgs e)
        {
            if (!GraphView.Tree.IsTree())
                ShowMessage("Graph is NOT a Tree", MessageBoxImage.Information);
            else
                ShowMessage("Graph is a Tree", MessageBoxImage.Information);
        }

        bool HandleCheckTreeStatus()
        {
            if (!GraphView.Tree.IsTree())
            {
                ShowMessage("Graph is NOT a Tree", MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void FindRoot_Click(object sender, RoutedEventArgs e)
        {
            if (HandleCheckTreeStatus())
            {
                GraphView.Tree.FindRoot();
            }
        }
		
		private void New_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            GraphView.Tree = new Tree();
        }
    }
}
