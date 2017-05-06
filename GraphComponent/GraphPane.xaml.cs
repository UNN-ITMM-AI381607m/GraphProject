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

        //PopupMenuDelegate events
        public event PopupMenuDelegate On_MenuItem_ChangeID;


        public GraphPane()
        {
            selectedVertex = null;
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }


        public Tree Graph
        {
            get { return vm.Tree; }
            set { vm.Tree = value; }
        }

        public void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selectedVertex == null)
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
                case ViewModel.VertexStatus.SUCCES:
                    break;
                case ViewModel.VertexStatus.OUT_OF_BOUNDS:
                    MessageBox.Show("Vertex number must be within the range [0, 1000)", "Error");
                    break;
                case ViewModel.VertexStatus.ALREADY_EXISTS:
                    MessageBox.Show("Vertex with number " + newId + " already exists", "Error");
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
