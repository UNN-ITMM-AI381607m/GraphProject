using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using System.ComponentModel;

namespace GraphComponent
{
    public class Tree : BidirectionalGraph<CustomVertex, CustomEdge>, INotifyPropertyChanged
    {
        public Tree(bool allowParallelEdges = false) : base(allowParallelEdges) { }
        public Tree(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }

        CustomVertex rootVertex;
        public CustomVertex Root
        {
            get
            {
                return rootVertex;
            }
            set
            {
                if (rootVertex != null)
                    rootVertex.IsRoot = false;
                rootVertex = value;
                if (rootVertex != null)
                {
                    rootVertex.IsRoot = true;
                    ReconstructTree();
                }
                NotifyPropertyChanged("Root");
            }
        }

        void ReconstructTree()
        {
            List<CustomVertex> visited = new List<CustomVertex>();
            Visit(rootVertex, visited);
        }

        void Visit(CustomVertex toVisit, List<CustomVertex> visited)
        {
            IEnumerable<CustomEdge> edges;
            if (TryGetInEdges(toVisit, out edges))
            {
                foreach (var e in edges)
                {
                    var start = e.Source;
                    if (!visited.Contains(start))
                        base.AddEdge(new CustomEdge(toVisit, start));
                }
                RemoveInEdgeIf(toVisit, x => !visited.Contains(x.Source));
            }
            visited.Add(toVisit);
            if (TryGetOutEdges(toVisit, out edges))
            {
                foreach (var e in edges)
                {
                    Visit(e.Target, visited);
                }
            }
        }

        public override bool AddEdge(CustomEdge e)
        {
            if (!AllowParallelEdges && Edges.Any(x => x.Source == e.Target && x.Target == e.Source))
                return false;

            var newTree = Clone();
            newTree.AddEdge(e);

            ClarifyRoot(newTree);
            if (!CheckTree(newTree))
                return false;

            base.AddEdge(e);
            return true;
        }

        void ClarifyRoot(BidirectionalGraph<CustomVertex, CustomEdge> newTree)
        {
            if (rootVertex != null && newTree.TryGetInEdges(rootVertex, out IEnumerable<CustomEdge> inRootEdges) && inRootEdges.Count() == 0)
                return;

            foreach (var v in Vertices)
            {
                if (newTree.TryGetInEdges(v, out IEnumerable<CustomEdge> inEdges) && inEdges.Count() == 0)
                {
                    Root = v;
                    return;
                }
            }
            Root = null;
        }

        bool CheckTree(IBidirectionalGraph<CustomVertex, CustomEdge> tree)
        {
            if (rootVertex == null)
                return false;

            bool state = true;
            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(tree);
            bfs.NonTreeEdge += u => state = false;
            bfs.Compute(rootVertex);
            return state;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
