using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphSharp.Controls;
using System.ComponentModel;
using GraphComponent.GraphBuilder;

namespace GraphComponent
{
    public class ViewModel : INotifyPropertyChanged
    {
        public enum TreeMode
        {
            DIRECTED,
            UNDIRECTED
        }

        public enum VertexStatus
        {
            SUCCESS,
            OUT_OF_BOUNDS,
            ALREADY_EXISTS
        }

        public enum EdgeStatus
        {
            NONE = 0,
            INVALID_ID1,
            INVALID_ID2,
            ALREADY_EXISTS,
            SUCCESS
        }

        public static TreeMode Mode = TreeMode.UNDIRECTED;

        private string layoutAlgorithmType;
        private Tree tree;
        private List<string> layoutAlgorithmTypes = new List<string>();

        public Tree Tree
        {
            get
            {
                if (Mode == TreeMode.DIRECTED)
                    return tree;
                else
                    return GetUndirected(tree);
            }
            set
            {
                tree = value;
                NotifyPropertyChanged("Tree");
            }
        }

        public Tree GetWorkTree()
        {
            return tree;
        }

        private Tree GetUndirected(Tree tree)
        {
            Tree undirectedTree = new Tree();
            undirectedTree.AddVertexRange(tree.Vertices);
            undirectedTree.AddEdgeRange(tree.Edges);
            foreach (var edge in tree.Edges)
            {
                undirectedTree.AddEdge(new CustomEdge(edge.Target, edge.Source));
            }
            return undirectedTree;
        }

        public void UpdateLayout()
        {
            LayoutAlgorithmType = "EfficientSugiyama";
            LayoutAlgorithmType = "None";
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public ViewModel()
        {
            Tree = new Tree();
        }

        public VertexStatus AddVertex(int id)
        {
            VertexStatus status = CheckValidId(id);
            if (status == VertexStatus.SUCCESS)
            {
                tree.AddVertex(new CustomVertex(id));
            }
            NotifyPropertyChanged("Tree");
            return status;
        }

        public void RemoveVertex(CustomVertex vertex)
        {
            tree.RemoveVertex(vertex);
            NotifyPropertyChanged("Tree");
        }

        public CustomVertex GetVertex(int id)
        {
            var vertices = tree.Vertices.ToList();
            for (int i = 0; i < vertices.Count; ++i)
            {
                if (vertices[i].ID == id)
                {
                    return vertices[i];
                };
            }
            return null;
        }

        public EdgeStatus AddEdge(CustomVertex from, CustomVertex to)
        {
            CustomEdge newEdge = new CustomEdge(from, to);
            var res = tree.AddEdge(newEdge);
            if (!res && Mode == TreeMode.DIRECTED)
            {
                var edge = GetEdge(to, from);
                if (edge != null) tree.RemoveEdge(edge);
                res = tree.AddEdge(newEdge);
            }
            NotifyPropertyChanged("Tree");
            return res ? EdgeStatus.SUCCESS : EdgeStatus.ALREADY_EXISTS;
        }

        public EdgeStatus AddEgde(int from, int to)
        {
            var v1 = GetVertex(from);
            if (v1 == null) return EdgeStatus.INVALID_ID1;
            var v2 = GetVertex(to);
            if (v2 == null) return EdgeStatus.INVALID_ID2;

            return AddEdge(v1, v2);
        }

        public void RemoveEdge(CustomEdge edge)
        {
            if (!tree.RemoveEdge(edge) && Mode == TreeMode.UNDIRECTED)
            {
                CustomEdge reverseEdge;
                tree.TryGetEdge(edge.Target, edge.Source, out reverseEdge);
                tree.RemoveEdge(reverseEdge);
            }
            NotifyPropertyChanged("Tree");
        }

        public CustomEdge GetEdge(CustomVertex source, CustomVertex target)
        {
            var edges = tree.Edges.ToList();
            for (int i = 0; i < edges.Count; ++i)
            {
                if (edges[i].Source == source && edges[i].Target == target)
                {
                    return edges[i];
                };
            }
            return null;
        }

        public VertexStatus ChangeId(CustomVertex vertex, int newId)
        {
            VertexStatus status = CheckValidId(newId);
            if (status == VertexStatus.SUCCESS)
                vertex.ID = newId;
            NotifyPropertyChanged("Tree");
            return status;
        }

        VertexStatus CheckValidId(int id)
        {
            if (id < 0 || id > 999)
                return VertexStatus.OUT_OF_BOUNDS;
            if (tree.Vertices.Any(x => x.ID == id))
                return VertexStatus.ALREADY_EXISTS;
            return VertexStatus.SUCCESS;
        }

        public void SetRoot(CustomVertex newRoot)
        {
            if (tree.Root != null)
                tree.Root.IsRoot = false;
            tree.Root = newRoot;
            if (newRoot != null)
            {
                tree.Root.IsRoot = true;
                tree.ReconstructTree();
                UpdateLayout();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        internal void SwitchMode()
        {
            if (Mode == TreeMode.DIRECTED)
                Mode = TreeMode.UNDIRECTED;
            else
                Mode = TreeMode.DIRECTED;

            var tmp = tree;
            tree = new Tree();
            tree.AddVertexRange(tmp.Vertices);
            tree.AddEdgeRange(tmp.Edges);
            Tree = tree;
        }
    }
}
