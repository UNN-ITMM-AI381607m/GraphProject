using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject.PrufCode
{
    class PrufCodes
    {
        public CustomGraph CodeToGraph (List<int> code)
        {
            CustomGraph NewGraph = new CustomGraph();
            List<CustomVertex> Vertices = new List<CustomVertex>();
            List<int> used = new List<int>();
            for (int i = 0; i < code.Count()+2; i++)
            {
                used.Add(i + 1);
                Vertices.Add(new CustomVertex(i + 1));
            }
            NewGraph.AddVertexRange(Vertices);
            while (code.Count()!=0)
                {
                    int use = 0;
                    while (code.Contains(used[use]))
                        use++;
                    CustomEdge Edgee = new CustomEdge("0", Vertices[code[0] - 1], Vertices[used[use] - 1]);
                    NewGraph.AddEdge(Edgee);
                    used.RemoveAt(use);
                    code.RemoveAt(0);
                }
            CustomEdge E = new CustomEdge("0", Vertices[used[0]-1], Vertices[used[1]-1]);
            NewGraph.AddEdge(E);
            return NewGraph;
        }

        public List<int> GraphToCode(CustomGraph Graph)
        {
            List<CustomVertex> Verticies = Graph.Vertices.ToList();
            List<CustomEdge> Edges = Graph.Edges.ToList();
            List<List<int>> Vertis = new List<List<int>>();
            for (int i = 0; i < Edges.Count(); i++)
                Vertis.Add(new List<int>() { Edges[i].Source.ID, Edges[i].Target.ID });
            List<int> Answer = new List<int>();

            while (Verticies.Count()>2)
            {
                int min = 99999;
                int min_ref = 99999;
                int for_remove_vert = -1;
                int for_remove_edge = -1;
                for (int k = 0; k < Verticies.Count(); k++)
                {
                    int maybe_ref = 0;
                    int counter = 0;
                    int id = Verticies[k].ID;
                    int may_rem = -1;
                    for (int i = 0; i < Vertis.Count(); i++)
                        for (int j = 0; j < Vertis[i].Count; j++)
                            if (id == Vertis[i][j])
                            {
                                counter++;
                                maybe_ref = Vertis[i][(j+1) % 2];
                                may_rem = i;
                            }
                    if ((counter == 1) && (min > id))
                    {
                        min = id;
                        min_ref = maybe_ref;
                        for_remove_vert = k;
                        for_remove_edge = may_rem;
                    }

                }
                Answer.Add(min_ref);
                Verticies.Remove(Verticies[for_remove_vert]);
                Vertis.Remove(Vertis[for_remove_edge]);
            }

            return Answer;
        }
    }
}
