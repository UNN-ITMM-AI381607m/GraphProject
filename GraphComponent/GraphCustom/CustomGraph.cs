using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace GraphComponent
{
    public class CustomGraph : BidirectionalGraph<CustomVertex, CustomEdge>
    {
        public CustomGraph(bool allowParallelEdges = false) : base(allowParallelEdges) { }
        public CustomGraph(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }

        public override bool AddEdge(CustomEdge e)
        {
            if (!AllowParallelEdges && Edges.Any(x => x.Source == e.Target && x.Target == e.Source))
                return false;
            return base.AddEdge(e);
        }
    }
}
