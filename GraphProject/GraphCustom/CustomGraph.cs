using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace GraphProject
{
    public class CustomGraph : BidirectionalGraph<CustomVertex, CustomEdge>
    {
        public CustomGraph() { }
        public CustomGraph(bool allowParallelEdges) : base(allowParallelEdges) { }
        public CustomGraph(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }
    }
}
