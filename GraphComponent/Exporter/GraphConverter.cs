using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphComponent.GraphConverter
{
    public static class GraphConverter
    {
        public enum CodingFormat
        {
            PrufferCode,
            CSR
        }

        public static Dictionary<CodingFormat, String> SupportedFilters = new Dictionary<CodingFormat, string>()
        {
            { CodingFormat.PrufferCode, "Text Files (*.txt)| *.txt" },
            { CodingFormat.CSR, "CSR Format (*.graph)| *.graph" }
        };


        public static string CodeGraphToString(Tree tree, CodingFormat format)
        {
            switch (format)
            {
                case CodingFormat.PrufferCode:
                    return string.Join(" ", GraphBuilder.GraphBuilderStrategy.GraphToCode(tree).ToArray());
                case CodingFormat.CSR:
                    return CodeToCSR(tree);
                default:
                    throw new Exception("Unknown format");
            }
        }

        // CRS code
        static public string CodeToCSR(Tree tree)
        {
            List<string> lines = new List<string>();
            int verices = tree.VertexCount;
            int edges = tree.EdgeCount;
            lines.Add(verices.ToString() + " " + edges.ToString());

            var sortedVertices = tree.Vertices.OrderBy(x => x.ID);
            foreach (var vertex in sortedVertices)
            {
                IEnumerable<CustomEdge> outEdges;
                tree.TryGetOutEdges(vertex, out outEdges);
                IEnumerable<CustomEdge> inEdges;
                tree.TryGetInEdges(vertex, out inEdges);

                var neighboursIds = outEdges.Select(x => x.Target.ID).Concat(inEdges.Select(x => x.Source.ID));

                string line = string.Join(" ", neighboursIds);
                if (line.Length != 0)
                    lines.Add(line);
            }
            return lines.Aggregate((current, next) => current + System.Environment.NewLine + next);
        }
    }
}
