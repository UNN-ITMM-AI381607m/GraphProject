using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

using GraphComponent.SettingWindow;
using System.Threading.Tasks;
using System;

namespace GraphComponent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GraphPane : UserControl
    {
        private ViewModel vm;
        private CustomVertex selectedVertex;
        private Step step;
        public delegate int PopupMenuDelegate(object sender, RoutedEventArgs e);
        public delegate void ShowMessageBoxDelegate(string message, MessageBoxImage icon);
        public delegate void NotifyPropertyChanged(string info);

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

        public ViewModel ViewModel
        {
            get { return vm; }
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
                    ShowMessage("Номер вершины должен быть в диапазоне: [0, 1000)", MessageBoxImage.Error);
                    break;
                case ViewModel.VertexStatus.ALREADY_EXISTS:
                    ShowMessage("Номер уже существует", MessageBoxImage.Error);
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

        public void AddNewVertex()
        {
            vm.AddVertex();
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
            if (ViewModel.Mode == ViewModel.TreeMode.UNDIRECTED || !GraphBuilder.GraphBuilderStrategy.ValidateOrientedGraph(vm.Tree))
            {
                ShowMessage("Граф НЕ является ориентированным деревом", MessageBoxImage.Error);
                return;
            }

            CustomVertex newRoot = GetVertexFromContextMenu(sender);
            vm.SetRoot(newRoot);
        }
        public class Step
        {
            public int pp_count;
            int counter ;
            Dictionary<List<CustomVertex>, int> pp;
            List<CustomVertex> verct;
            public Step(ViewModel vm, CustomVertex newRoot)
            {
                counter = 1;
                verct = vm.Tree.Vertices.ToList<CustomVertex>();

                List<CustomEdge> edges = vm.Tree.Edges.ToList<CustomEdge>();
                foreach (CustomVertex vv in verct)
                    vv.ID = 0;
                newRoot.ID = 1;

                List<List<CustomVertex>> paths = new List<List<CustomVertex>>();
                paths.Add(new List<CustomVertex> { newRoot });
                int n = verct.Count - 1;

                while (n > 0)
                {

                    for (int i = 0; i < paths.Count; i++)
                    {
                        CustomVertex last = paths[i].Last<CustomVertex>();
                        List<CustomVertex> outs = new List<CustomVertex>();
                        foreach (CustomEdge edge in edges)
                        {

                            if (edge.Source == last)
                            {
                                outs.Add(edge.Target);
                            }

                        }

                        if (outs.Count == 1)
                        {
                            --n; paths[i].Add(outs.First<CustomVertex>());
                        }
                        if (outs.Count > 1)
                        {
                            foreach (CustomVertex v in outs)
                            {
                                --n;
                                paths.Add(paths[i].ToList<CustomVertex>());
                                paths.Last<List<CustomVertex>>().Add(v);
                            }
                            paths.Remove(paths[i]);
                        }

                    }
                }
                pp = new Dictionary<List<CustomVertex>, int>();
                foreach (List<CustomVertex> path in paths)
                    pp.Add(path, path.Count);

               
                Tree tre = new Tree();
                
                pp_count = pp.Count;
            }
            public void DoStep()
            {
                foreach (CustomVertex vv in verct)
                {
                    vv.ResetColor();
                }

                int current_min = int.MaxValue;
                foreach (KeyValuePair<List<CustomVertex>, int> d in pp)
                    if (current_min > d.Value) current_min = d.Value;
                
                 for (int i = 0; i < pp.Count; i++)
                    {
                        if (pp.Count == 0) break;
                        if (pp.ElementAt(i).Value == current_min)
                        {
                            for (int j = 0; j < pp.ElementAt(i).Key.Count; ++j)
                                if (pp.ElementAt(i).Key[j].ID == 0)
                            { pp.ElementAt(i).Key[j].ID = ++counter;
                        }
                        pp.ElementAt(i).Key.Last().Color = "Red";
                        pp.Remove(pp.ElementAt(i).Key);
                            --pp_count;
                        break;
                        }
                        current_min = int.MaxValue;
                        foreach (KeyValuePair<List<CustomVertex>, int> d in pp)
                            if (current_min > d.Value) current_min = d.Value;
                    }
                if (pp_count >= 0) Task.Delay(1300).ContinueWith(t => DoStep());
                else
                {
                    foreach (CustomVertex vv in verct)
                        vv.ResetColor();
                }
            }
        }
        //private void MenuItem_ChangeToRootForAll(object sender, RoutedEventArgs e)
        //{
        //    if (!GraphBuilder.GraphBuilderStrategy.ValidateOrientedGraph(Tree))
        //    {
        //        ShowMessage("Граф НЕ является ориентированным деревом", MessageBoxImage.Error);
        //        return;
        //    }

        //    CustomVertex newRoot = GetVertexFromContextMenu(sender);
        //    vm.SetRoot(newRoot);
            
        //    step = new Step(vm, newRoot);
           
        //    step.DoStep();
        //}

        public void DoNumerateStep(CustomVertex root)
        {
            step = new Step(vm, root);
            step.DoStep();
        }

        public void Switch()
        {
            vm.SwitchMode();
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
