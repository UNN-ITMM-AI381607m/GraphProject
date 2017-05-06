using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace GraphComponent
{
    public class Tree : BidirectionalGraph<CustomVertex, CustomEdge>
    {
        public Tree(bool allowParallelEdges = false) : base(allowParallelEdges) { }
        public Tree(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }

        public CustomVertex Root { get; private set; }

        public override bool AddEdge(CustomEdge e)
        {
            if (!AllowParallelEdges && Edges.Any(x => x.Source == e.Target && x.Target == e.Source))
                return false;

            var newTree = Clone();
            newTree.AddEdge(e);

            SureRoot();
            if (!CheckTree(newTree))
                return false;

            base.AddEdge(e);
            return true;
        }

        void SureRoot()
        {
            if (Root != null && TryGetInEdges(Root, out IEnumerable<CustomEdge> inRootEdges) && inRootEdges.Count() == 0)
                return;

            foreach (var v in Vertices)
            {
                if (TryGetInEdges(v, out IEnumerable<CustomEdge> inEdges) && inEdges.Count() == 0)
                {
                    Root = v;
                    return;
                }
            }
            Root = null;
        }

        bool CheckTree(IBidirectionalGraph<CustomVertex, CustomEdge> tree)
        {
            if (Root == null)
                return false;

            bool state = true;
            BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge> bfs = new BreadthFirstSearchAlgorithm<CustomVertex, CustomEdge>(tree);
            bfs.NonTreeEdge += u => state = false;
            bfs.Compute(Root);
            return state;
        }
    }
}
