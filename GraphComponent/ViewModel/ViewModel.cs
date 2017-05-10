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
        private string layoutAlgorithmType;
        private Tree tree;
        private List<string> layoutAlgorithmTypes = new List<string>();

        public Tree Tree
        {
            get { return tree; }
            set
            {
                tree = value;
                tree.PropertyChanged += Tree_PropertyChanged;
                NotifyPropertyChanged("Tree");
            }
        }

        public void UpdateLayout()
        {
            LayoutAlgorithmType = "EfficientSugiyama";
            LayoutAlgorithmType = "None";
        }

        public List<string> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
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

        public ViewModel()
        {
            Tree = new Tree();
            //Add Layout Algorithm Types
            //layoutAlgorithmTypes.Add("None");
            //layoutAlgorithmTypes.Add("BoundedFR");
            //layoutAlgorithmTypes.Add("Circular");
            //layoutAlgorithmTypes.Add("CompoundFDP");
            //layoutAlgorithmTypes.Add("EfficientSugiyama");
            //layoutAlgorithmTypes.Add("FR");
            //layoutAlgorithmTypes.Add("ISOM");
            //layoutAlgorithmTypes.Add("KK");
            //layoutAlgorithmTypes.Add("LinLog");
            //layoutAlgorithmTypes.Add("Tree");
        }

        private void Tree_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Root" && tree.Root != null)
            {
                UpdateLayout();
            }
        }

        public VertexStatus AddVertex(int id)
        {
            VertexStatus status = CheckValidId(id);
            if (status == VertexStatus.SUCCESS)
            {
                tree.AddVertex(new CustomVertex(id));
            }
            return status;
        }

        public void RemoveVertex(CustomVertex vertex)
        {
            tree.RemoveVertex(vertex);
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
            if (!res)
            {
                var edge = GetEdge(to, from);
                if (edge != null) tree.RemoveEdge(edge);
                res = tree.AddEdge(newEdge);
            }
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
            tree.RemoveEdge(edge);
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
            tree.Root = newRoot;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
