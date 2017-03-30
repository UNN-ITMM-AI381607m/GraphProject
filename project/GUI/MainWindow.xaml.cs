using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using GraphComponent.GraphBuilder;
using GraphComponent.SettingWindow;

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
            GraphView.On_MenuItem_NewVertex += MenuItem_NewVertex_Click;
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
                MessageBox.Show(this, "File not found or has invalid format", "Error");
                return;
            }
        }

        //File menu handlers
        private void NewGraph_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
        }

        private void OpenFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            PruferResult.Content = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ConstructByPrufer(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private void SaveFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialogFile = new SaveFileDialog();
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


        public void Close_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Close();
        }

        //Workflow handlers  
        public void ConstructByPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            PruferResult.Content = "";
            ConstructByPrufer(PruferTextBox.Text.ToString());
        }


        public void GetPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            bool isEmpty = GraphView.Graph.IsVerticesEmpty;
            PruferResult.Content = isEmpty ? "" : "Generated Prufer Code: " + string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Graph).ToArray());
        }

        public void Numerate_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void GraphView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //GraphPane SystemWindow handlers
        private int MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
            int result = popup.NewID;
            if (result == -1)
            {
                return 0;
            }
            return result;
        }

        private int MenuItem_NewVertex_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
            int result = popup.NewID;
            if (result == 0)
            {
                return 0;
            }
            return result;
        }
    }
}
