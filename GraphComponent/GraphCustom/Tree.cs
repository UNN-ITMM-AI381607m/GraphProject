using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using System.ComponentModel;
using System.Collections;

namespace GraphComponent
{
    public class Tree : BidirectionalGraph<CustomVertex, CustomEdge>, INotifyPropertyChanged
    {
        public Tree(bool allowParallelEdges = false) : base(allowParallelEdges) { }
        public Tree(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }

        public CustomVertex Root;

        public void ReconstructTree()
        {
            List<CustomVertex> visited = new List<CustomVertex>();
            Visit(Root, visited);
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
            if (ViewModel.Mode == ViewModel.TreeMode.DIRECTED && Edges.Any(x => x.Source == e.Target && x.Target == e.Source))
                return false;

            return base.AddEdge(e);
        }

        public override bool RemoveVertex(CustomVertex v)
        {
            v.Selected = false;
            return base.RemoveVertex(v);
        }

        public CustomVertex FindRoot()
        {
            return TryFindRoot();
        }

        CustomVertex TryFindRoot()
        {
            int rootCounter = 0;
            CustomVertex root = null;
            foreach (var vertex in Vertices)
            {
                IEnumerable<CustomEdge> edges;
                if (TryGetInEdges(vertex, out edges) && edges.Count() == 0)
                {
                    rootCounter++;
                    root = vertex;
                }
            }
            if (rootCounter == 1)
                return root;
            return null;
        }

        public int GetLength(Dictionary<CustomVertex, int> mapId = null)
        {
            int length = 0;

            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(this);
            CustomVertex firstVertex = null;
            bfs.ExamineVertex += u => firstVertex = u;
            bfs.DiscoverVertex += u =>
            {
                if (firstVertex != null)
                {
                    int diff = 0;
                    if (mapId == null)
                        diff = Math.Abs(firstVertex.ID - u.ID);
                    else
                        diff = Math.Abs(mapId[firstVertex] - mapId[u]);

                    length += diff;
                }
            };
            if (Root != null)
                bfs.Compute(Root);
            else
                bfs.Compute(TryFindRoot());

            return length;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
