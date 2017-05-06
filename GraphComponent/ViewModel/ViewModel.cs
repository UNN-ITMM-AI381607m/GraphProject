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
        private CustomGraph graph;
        private List<string> layoutAlgorithmTypes = new List<string>();

        public CustomGraph Graph
        {
            get { return graph; }
            set
            {
                SortLayout(true);
                graph = value;
                NotifyPropertyChanged("Graph");
                SortLayout(false);
            }
        }

        void SortLayout(bool enabled)
        {
            if (enabled)
                LayoutAlgorithmType = "Tree";
            else
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
            graph = new CustomGraph(true);

            //Add Layout Algorithm Types
            layoutAlgorithmTypes.Add("None");
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");

            LayoutAlgorithmType = "LinLog";
        }

        public VertexStatus AddNewVertex(int id)
        {
            VertexStatus status = CheckValidId(id);
            if (status == VertexStatus.SUCCES)
            {
                graph.AddVertex(new CustomVertex(id));
            }
            return status;
        }

        public void RemoveVertex(CustomVertex vertex)
        {
            graph.RemoveVertex(vertex);
        }

        public void RemoveEdge(CustomEdge edge)
        {
            graph.RemoveEdge(edge);
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
            if (graph.Vertices.Any(x => x.ID == id))
                return VertexStatus.ALREADY_EXISTS;
            return VertexStatus.SUCCES;
        }

        public CustomEdge AddNewGraphEdge(CustomVertex from, CustomVertex to)
        {
            CustomEdge newEdge = new CustomEdge(from, to);
            graph.AddEdge(newEdge);
            return newEdge;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
