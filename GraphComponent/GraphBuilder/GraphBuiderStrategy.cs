using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphComponent.GraphBuilder
{
    static public class GraphBuilderStrategy
    {
        //static public CustomGraph CodeToGraph1(string code)
        //{
        //    List<int> code_list = code.Split(' ').Select(int.Parse).ToList();
        //    return CodeToGraph(code_list);
        //}

        static public CustomGraph CodeToGraph (List<int> code)
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

        static public List<int> GraphToCode(CustomGraph Graph)
        {
            List<CustomVertex> Verticies = Graph.Vertices.ToList();
            List<CustomEdge> Edges = Graph.Edges.ToList();
            List<int[]> ListEdges = new List<int[]>();
            for (int i = 0; i < Edges.Count(); i++)
            {
                int[] current_edge = new int[2] { Edges[i].Source.ID, Edges[i].Target.ID };
                ListEdges.Add(current_edge);
        }
            List<int> Answer = new List<int>();

            while (Verticies.Count()>2)
            {
                int min = int.MaxValue;
                int min_ref = int.MaxValue;
                int for_remove_vert = int.MaxValue;
                int for_remove_edge = int.MaxValue;
                for (int k = 0; k < Verticies.Count(); k++)
                {
                    int maybe_ref = int.MaxValue;
                    int counter = 0;
                    int id = Verticies[k].ID;
                    int may_rem = int.MaxValue;
                    for (int i = 0; i < ListEdges.Count(); i++)
                        for (int j = 0; j < 2; j++)
                            if (id == ListEdges[i][j])
                            {
                                counter++;
                                maybe_ref = ListEdges[i][(j+1) % 2];
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
                ListEdges.RemoveAt(for_remove_edge);
            }

            return Answer;
        }
    }
}
