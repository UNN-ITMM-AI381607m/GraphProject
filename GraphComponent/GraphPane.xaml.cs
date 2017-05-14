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

        public delegate int PopupMenuDelegate(object sender, RoutedEventArgs e);
        public delegate void ShowMessageBoxDelegate(string message, MessageBoxImage icon);

        //PopupMenuDelegate events
        public event PopupMenuDelegate On_MenuItem_ChangeID;
        public event ShowMessageBoxDelegate ShowMessage;


        public GraphPane()
        {
            selectedVertex = null;
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        public Tree Tree
        {
            get { return vm.Tree; }
            set { vm.Tree = value; }
        }

        public void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selectedVertex == null || !selectedVertex.Selected)
            {
                selectedVertex = (sender as Label).Content as CustomVertex;
                selectedVertex.Selected = true;
            }
            else
            {
                CustomVertex second = ((sender as Label).Content as CustomVertex);
                if (second != selectedVertex)
                {
                    vm.AddEdge(selectedVertex, second);
                }
                selectedVertex.Selected = false;
                selectedVertex = null;
            }
        }

        private void MenuItem_ChangeID_Click(object sender, RoutedEventArgs e)
        {
            CustomVertex vertex = GetVertexFromContextMenu(sender);
            var result = On_MenuItem_ChangeID(sender, e);
            if (result == -1)
                return;
            HandleNewIdStatus(vm.ChangeId(vertex, result), result);
        }

        void HandleNewIdStatus(ViewModel.VertexStatus status, int newId)
        {
            switch (status)
            {
                case ViewModel.VertexStatus.SUCCESS:
                    break;
                case ViewModel.VertexStatus.OUT_OF_BOUNDS:
                case ViewModel.VertexStatus.ALREADY_EXISTS:
                    ShowMessage("Номер вершины должен быть в диапазоне: [0, 1000)", MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }

        public void AddNewVertex(int number)
        {
            if (number == -1)
                return;
            HandleNewIdStatus(vm.AddVertex(number), number);
        }

        public bool AddNewEdge(int id1, int id2)
        {
            var status = vm.AddEgde(id1, id2);
            HandleCreatingEdgeStatus(status, id1, id2);
            return status == ViewModel.EdgeStatus.SUCCESS;
        }

        void HandleCreatingEdgeStatus(ViewModel.EdgeStatus status, int id1, int id2)
        {
          switch(status)
          {
                case ViewModel.EdgeStatus.INVALID_ID1:
                    ShowMessage("Вершина с номером " + id1 + " не существует!", MessageBoxImage.Error);
                    break;
                case ViewModel.EdgeStatus.INVALID_ID2:
                    ShowMessage("Вершина с номером " + id2 + " не существует!", MessageBoxImage.Error);
                    break;
            }
        }

        private void MenuItem_DeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            CustomVertex vertex = GetVertexFromContextMenu(sender);
            vm.RemoveVertex(vertex);
        }

        private void MenuItem_DeleteEdge_Click(object sender, RoutedEventArgs e)
        {
            CustomEdge edge = GetEdgeFromContextMenu(sender);
            vm.RemoveEdge(edge);
        }

        private void MenuItem_MarkAsRoot_Click(object sender, RoutedEventArgs e)
        {
            if (!GraphBuilder.GraphBuilderStrategy.ValidateOrientedGraph(Tree))
            {
                ShowMessage("Граф НЕ является ориентированным деревом", MessageBoxImage.Error);
                return;
            }

            CustomVertex newRoot = GetVertexFromContextMenu(sender);
            vm.SetRoot(newRoot);
        }

        CustomVertex GetVertexFromContextMenu(object sender)
        {
            return (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as GraphSharp.Controls.VertexControl).Vertex as CustomVertex;
        }

        CustomEdge GetEdgeFromContextMenu(object sender)
        {
            return (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as GraphSharp.Controls.EdgeControl).Edge as CustomEdge;
        }
    }
}
