using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using GraphComponent.GraphBuilder;
using GraphComponent.SettingWindow;
using GraphComponent.PopupWindow;
using GraphComponent;
using System.Windows.Controls;

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
                    title = "Сообщение";
                    break;
                case MessageBoxImage.Error:
                    title = "Ошибка";
                    break;
                case MessageBoxImage.Warning:
                    title = "Предупреждение";
                    break;
                case MessageBoxImage.Information:
                    title = "Информация";
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
                ShowMessage("Неверный код Прюфера", MessageBoxImage.Error);
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
                InfoBar.Content = "";
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
                    ShowMessage("Отсутствует граф для сохранения", MessageBoxImage.Error);
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
                    ShowMessage("Невозможно сохранить граф в файл: \n" + saveDialogFile.FileName, MessageBoxImage.Error);
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
            PopupWindow popup = new PopupWindow("Создать вершину", "Введите номер новой вершины: ", "Создать")
            {
                Owner = this
            };
            popup.ShowDialog();
            GraphView.AddNewVertex(popup.NewID);
        }

        private void NewEdge_OnClick(object sender, RoutedEventArgs e)
        {
            PopupWindowWith2Boxes popup = new PopupWindowWith2Boxes("Создать ребро", "Введите номера двух вершин, которые хотите соединить: ", "Соединить")
            {
                Owner = this
            };
            popup.ShowDialog();
            GraphView.AddNewEdge(popup.ID1, popup.ID2);
        }

        //Workflow handlers  
        private void ConstructByPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            ConstructByPrufer(PruferTextBox.Text.ToString());
            InfoBar.Content = "";
        }

        private void GetPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckTreeStatus())
                return;

            bool isEmpty = GraphView.Tree.IsVerticesEmpty;
            InfoBar.Content = isEmpty ? "" : "Код Прюфера: " + string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Tree).ToArray());
        }

        private void Numerate_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckTreeStatus())
                return;

            var mapId = Numerator.GetIDMap(GraphView.Tree);
            foreach (var pair in mapId)
            {
                pair.Key.ID = pair.Value;
            }
        }

        //GraphPane PopupWindow handlers
        private int MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow("Изменить номер", "Введите новый номер вершины: ", "Сохранить")
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
                ShowMessage("Граф НЕ является деревом", MessageBoxImage.Information);
            else
                ShowMessage("Граф является деревом", MessageBoxImage.Information);
        }

        bool HandleCheckTreeStatus()
        {
            if (!GraphView.Tree.IsTree())
            {
                ShowMessage("Граф НЕ является деревом", MessageBoxImage.Error);
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

        private void GetLength_Click(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckTreeStatus())
                return;

            int length = GraphView.Tree.GetLength();
            int minLength = GraphView.Tree.GetLength(Numerator.GetIDMap(GraphView.Tree));
            InfoBar.Content = "Текущая длина: " + length + "  Минимальная длина: " + minLength;
        }

        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
                TaskGrid.Visibility = Visibility.Visible;
            else
                TaskGrid.Visibility = Visibility.Collapsed;
        }

        private void GenerateCode_OnClick(object sender, RoutedEventArgs e)
        {
            PruferCode.Content = "1 2 3" ;
        }

        private void CheckGraph_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateGraph_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void CheckCode_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
