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

            return base.AddEdge(e);
        }

        public override bool RemoveVertex(CustomVertex v)
        {
            v.Selected = false;
            return base.RemoveVertex(v);
        }

        public bool FindRoot()
        {
            var root = TryFindRoot();

            if (root == null)
                return false;

            Root = root;
            return true;
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

        IEnumerable<CustomEdge> FindNonTreeEdges(CustomVertex root)
        {
            List<CustomEdge> nonTreeEdges = new List<CustomEdge>();

            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(this);
            bfs.NonTreeEdge += u => nonTreeEdges.Add(u);
            bfs.Compute(root);
            
            return nonTreeEdges;
        }

        IDictionary<CustomVertex, GraphColor> GetBFSColors(CustomVertex root)
        {
            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(this);
            bfs.Compute(root);
            return bfs.VertexColors;
        }

        public bool IsTree()
        {
            CustomVertex root = TryFindRoot();

            if (root == null)
                return false;

            var nonTreeEdges = FindNonTreeEdges(root);

            bool notAllVisited = GetBFSColors(root).Any(x => x.Value == GraphColor.White);

            if (nonTreeEdges.Count() > 0 || notAllVisited)
                return false;

            return true;
        }

        public int GetLength()
        {
            int length = 0;

            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(this);
            CustomVertex firstVertex = null;
            bfs.ExamineVertex += u => firstVertex = u;
            bfs.DiscoverVertex += u =>
            {
                if (firstVertex != null)
                {
                    int diff = Math.Abs(firstVertex.ID - u.ID);
                    length += diff;
                }
            };
            if (rootVertex != null)
                bfs.Compute(rootVertex);
            else
                bfs.Compute(TryFindRoot());

            return length;
        }

        private void Bfs_DiscoverVertex(CustomVertex vertex)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
