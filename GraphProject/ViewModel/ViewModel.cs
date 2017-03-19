﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphSharp.Controls;
using System.ComponentModel;
using GraphProject.GraphBuilder;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
