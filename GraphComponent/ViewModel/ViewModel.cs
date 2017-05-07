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
            LayoutAlgorithmType = "Tree";
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
            if (e.PropertyName == "Root")
            {
                UpdateLayout();
            }
        }

        public VertexStatus AddVertex(int id)
        {
            VertexStatus status = CheckValidId(id);
            if (status == VertexStatus.SUCCES)
            {
                tree.AddVertex(new CustomVertex(id));
            }
            return status;
        }

        public void RemoveVertex(CustomVertex vertex)
        {
            tree.RemoveVertex(vertex);
        }

        public void RemoveEdge(CustomEdge edge)
        {
            tree.RemoveEdge(edge);
        }

        public VertexStatus ChangeId(CustomVertex vertex, int newId)
        {
            VertexStatus status = CheckValidId(newId);
            if (status == VertexStatus.SUCCES)
                vertex.ID = newId;
            return status;
        }

        public enum VertexStatus
        {
            SUCCES,
            OUT_OF_BOUNDS,
            ALREADY_EXISTS
        }

        VertexStatus CheckValidId(int id)
        {
            if (id < 0 || id > 999)
                return VertexStatus.OUT_OF_BOUNDS;
            if (tree.Vertices.Any(x => x.ID == id))
                return VertexStatus.ALREADY_EXISTS;
            return VertexStatus.SUCCES;
        }

        public bool AddEdge(CustomVertex from, CustomVertex to)
        {
            CustomEdge newEdge = new CustomEdge(from, to);
            return tree.AddEdge(newEdge);
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
