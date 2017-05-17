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
using System;

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

        #region Message Handling

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

        bool HandleCheckOrientedTreeStatus()
        {
            if (!GraphBuilderStrategy.ValidateOrientedGraph(GraphView.Tree))
            {
                ShowMessage("Граф НЕ является ориентированным деревом", MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        bool HandleCheckNonOrientedTreeStatus()
        {
            if (!GraphBuilderStrategy.ValidateNonOrientedGraph(GraphView.Tree))
            {
                ShowMessage("Граф НЕ является деревом", MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region Menu Handling

        private void OpenFile_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ConstructByPrufer(File.ReadAllText(openFileDialog.FileName));
                InfoBar.Content = "";
            }
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

        private void New_OnClick(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            GraphView.Tree = new Tree();
            InfoBar.Content = "";
            PruferTextBox.Clear();
        }

        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
                TaskGrid.Visibility = Visibility.Visible;
            else
                TaskGrid.Visibility = Visibility.Collapsed;
        }

        private void OpenTheory_Click(object sender, RoutedEventArgs e)
        {

            Window theoryWindow = new TheoryWindow();
            theoryWindow.Show();
            
            //w.setValue("text");

        }

        #endregion

        #region ToolBar Handling

        private void NewVertex_OnClick(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow("Создать вершину", "Введите номер новой вершины: ", "Создать")
            {
                Owner = this
            };
            if (popup.ShowDialog() == true)
                GraphView.AddNewVertex(popup.Result);
        }

        private void NewEdge_OnClick(object sender, RoutedEventArgs e)
        {
            PopupWindowWith2Boxes popup = new PopupWindowWith2Boxes("Создать ребро", "Введите номера двух вершин, которые хотите соединить: ", "Соединить")
            {
                Owner = this
            };
            if (popup.ShowDialog() == true)
                GraphView.AddNewEdge(popup.Result1, popup.Result2);
        }

        #endregion

        #region Workflow Handling

        private void ConstructByPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            List<int> pruferCode = null;

            try
            {
                pruferCode = PruferTextBox.Text.Trim(' ').Split(' ', ',', ';').Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                ShowMessage("Неверный формат", MessageBoxImage.Error);
                return;
            }

            if (!GraphBuilderStrategy.ValidateCode(pruferCode))
            {
                ShowMessage("Неверный код Прюфера", MessageBoxImage.Error);
                return;
            }
            GraphView.Tree = GraphBuilderStrategy.CodeToGraph(pruferCode);
            UpdateLayoutThroughViewModel();
            InfoBar.Content = "";
        }

        private void GetPrufer_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckNonOrientedTreeStatus())
                return;

            bool isEmpty = GraphView.Tree.IsVerticesEmpty;
            InfoBar.Content = isEmpty ? "" : "Код Прюфера: " + string.Join(" ", GraphBuilderStrategy.GraphToCode(GraphView.Tree).ToArray());
        }

        private void Numerate_OnClick(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckNonOrientedTreeStatus())
                return;

            var mapId = Numerator.GetIDMap(GraphView.Tree);
            foreach (var pair in mapId)
            {
                pair.Key.ID = pair.Value;
            }
        }

        #endregion

        #region Graph Control

        private int MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow("Изменить номер", "Введите новый номер вершины: ", "Сохранить")
            {
                Owner = this
            };

            if (popup.ShowDialog() == true)
                return popup.Result;
            else
                return -1;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateLayoutThroughViewModel();
        }

        private void CheckTree_Click(object sender, RoutedEventArgs e)
        {
            if (!GraphBuilderStrategy.ValidateOrientedGraph(GraphView.Tree))
                ShowMessage("Граф НЕ является ориентированным деревом", MessageBoxImage.Information);
            else
                ShowMessage("Граф является ориентированным деревом", MessageBoxImage.Information);
        }

        private void FindRoot_Click(object sender, RoutedEventArgs e)
        {
            if (HandleCheckOrientedTreeStatus())
            {
                GraphView.Tree.FindRoot();
            }
        }

        private void GetLength_Click(object sender, RoutedEventArgs e)
        {
            if (!HandleCheckNonOrientedTreeStatus())
                return;

            int length = GraphView.Tree.GetLength();
            int minLength = GraphView.Tree.GetLength(Numerator.GetIDMap(GraphView.Tree));
            InfoBar.Content = "Текущая длина: " + length + "  Минимальная длина: " + minLength;
        }

        void UpdateLayoutThroughViewModel()
        {
            (GraphView.DataContext as ViewModel).UpdateLayout();
        }

        void ConstructByPrufer(string str)
        {
            List<int> pruferCode = str.Split(' ', ',', ';').Select(int.Parse).ToList();
            if (!GraphBuilderStrategy.ValidateCode(pruferCode))
            {
                ShowMessage("Неверный код Прюфера", MessageBoxImage.Error);
                return;
            }
            GraphView.Tree = GraphBuilderStrategy.CodeToGraph(pruferCode);
            UpdateLayoutThroughViewModel();
        }

        #endregion
    }
}
