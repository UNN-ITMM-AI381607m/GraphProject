using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphSharp.Controls;
using System.ComponentModel;
using GraphProject.PrufCode;

namespace GraphProject
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string layoutAlgorithmType;
        private CustomGraph graph;
        private List<string> layoutAlgorithmTypes = new List<string>();

        public int ID_counter
        {
            get;
            private set;
        }

        public ViewModel()
        {
            ID_counter = 0;
            graph = new CustomGraph(true);

            List<CustomVertex> vertices = new List<CustomVertex>();
            for (ID_counter = 0; ID_counter < 5; ID_counter++)
            {
                vertices.Add(new CustomVertex(ID_counter));
            }

            foreach (CustomVertex vertex in vertices)
            {
                graph.AddVertex(vertex);
            }

            AddNewGraphEdge(vertices[0], vertices[1]);
            AddNewGraphEdge(vertices[0], vertices[4]);
            AddNewGraphEdge(vertices[2], vertices[1]);
            AddNewGraphEdge(vertices[3], vertices[0]);

            //Add Layout Algorithm Types
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

            PrufCodes A = new PrufCodes();
            List<int> B = new List<int>();
            B.Add(6);
            B.Add(1);
            B.Add(6);
            B.Add(1);
            B.Add(1);
            graph = A.CodeToGraph(B);

            List<int> i = A.GraphToCode(graph);
        }

        public List<String> LayoutAlgorithmTypes
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

        public void AddNewVertex(CustomVertex vertex)
        {
            ID_counter++;
            graph.AddVertex(vertex);
        }

        public CustomEdge AddNewGraphEdge(CustomVertex from, CustomVertex to)
        {
            string edgeString = string.Format("{0}-{1} Connected", from.ID, to.ID);
            
            CustomEdge newEdge = new CustomEdge(edgeString, from, to);
            if (!graph.Edges.Any(item => item.ID == newEdge.ID))
            {

                graph.AddEdge(newEdge);
                return newEdge;
            }
            return null;
        }
        public CustomGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


    }
}
