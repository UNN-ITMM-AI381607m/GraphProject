using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphComponent
{
    public class Numerator
    {
        public static Dictionary<CustomVertex, int> GetIDMap(Tree tree, bool fullNum = false)
        {
            List<MyEdge> new_graph = ConvertGraph(tree);
            InitGraph(new_graph, tree.VertexCount);
            Numeration(new_graph, 1, fullNum);
            return CreateIDMap(new_graph, tree);
        }

        static Dictionary<CustomVertex, int> CreateIDMap(List<MyEdge> newGraph, Tree tree)
        {
            Dictionary<CustomVertex, int> idMap = new Dictionary<CustomVertex, int>();
            foreach (var edge in newGraph)
            {
                idMap[tree.Vertices.First(x => x.ID == edge.Preview.Id)] = edge.Preview.Number;
                idMap[tree.Vertices.First(x => x.ID == edge.Next.Id)] = edge.Next.Number;
            }

            return idMap;
        }

        static List<MyEdge> ConvertGraph(Tree graph)
        {
            List<MyEdge> result = new List<MyEdge>();
            List<MyVertex> vertices = new List<MyVertex>();
            foreach (CustomEdge edge in graph.Edges)
            {
                MyVertex tmp1, tmp2;
                MyEdge tmp_edge;
                if ((tmp1 = vertices.Find(x => x.Id == edge.Source.ID)) == null)
                {
                    tmp1 = new MyVertex(edge.Source.ID);
                    vertices.Add(tmp1);
                }
                if ((tmp2 = vertices.Find(x => x.Id == edge.Target.ID)) == null)
                {
                    tmp2 = new MyVertex(edge.Target.ID);
                    vertices.Add(tmp2);
                }
                tmp_edge = new MyEdge(tmp1, tmp2);
                result.Add(tmp_edge);
                tmp1.List_of_edge.Add(tmp_edge);
                tmp2.List_of_edge.Add(tmp_edge);
            }
            return result;
        }
        static void InitGraph(List<MyEdge> graph, int count)
        {
            List<MyEdge> current_list = InitGraphBegin(graph, count);
            List<MyEdge> tmp_list, next_list;
            int k;
            MyEdge next = null;
            while (current_list.Count != 0)
            {
                next_list = new List<MyEdge>();
                foreach (MyEdge edge in current_list)
                {
                    k = 0;
                    if (edge.Next.Mark) tmp_list = edge.Next.List_of_edge;
                    else tmp_list = edge.Preview.List_of_edge;
                    foreach (MyEdge tmp in tmp_list)
                        if (tmp.Number_vertex_next == 0)
                        {
                            k++;
                            next = tmp;
                        }
                    if (k == 1)
                    {
                        foreach (MyEdge tmp in tmp_list)
                        {
                            if (tmp == next) continue;
                            next.Number_vertex_next += tmp.Next == next.Next ? tmp.Number_vertex_preview : tmp.Number_vertex_next;
                        }
                        next.Number_vertex_next++;
                        next.Number_vertex_preview = count - next.Number_vertex_next;

                        next.Preview.Mark = next.Next.Mark;
                        next.Next.Mark = !next.Next.Mark;

                        next_list.Add(next);
                    }
                    if (k > 1) next_list.Add(edge);
                }
                current_list = next_list;
            }
        }
        static List<MyEdge> InitGraphBegin(List<MyEdge> graph, int count)
        {
            List<MyEdge> result = new List<MyEdge>();
            foreach (MyEdge edge in graph)
            {
                if (edge.Next.List_of_edge.Count == 1)
                {
                    edge.Number_vertex_next = 1;
                    edge.Number_vertex_preview = count - 1;
                    edge.Preview.Mark = true;
                    result.Add(edge);
                }
                if (edge.Preview.List_of_edge.Count == 1)
                {
                    edge.Number_vertex_preview = 1;
                    edge.Number_vertex_next = count - 1;
                    edge.Next.Mark = true;
                    result.Add(edge);
                }
            }
            return result;
        }
        static MyVertex SearchNextVertex(MyVertex current)
        {
            int max = int.MinValue;
            MyVertex next = null;
            foreach (MyEdge edge in current.List_of_edge)
            {
                if (edge.Mark) continue;
                if (edge.Next == current)
                {
                    if (edge.Number_vertex_preview > max)
                    {
                        max = edge.Number_vertex_preview;
                        next = edge.Preview;
                    }
                }
                else
                {
                    if (edge.Number_vertex_next > max)
                    {
                        max = edge.Number_vertex_next;
                        next = edge.Next;
                    }
                }
            }
            return next;
        }
        static MyVertex SearchFirstVertex(List<MyEdge> graph)
        {
            List<MyEdge> markEdges = new List<MyEdge>();
            MyVertex next = SearchNextVertex(graph[0].Next), current = graph[0].Next;
            while (next.List_of_edge.Count != 1)
            {
                foreach (MyEdge edge in next.List_of_edge)
                    if (edge.Next == next && edge.Preview == current || edge.Next == current && edge.Preview == next)
                    {
                        edge.Mark = true;
                        markEdges.Add(edge);
                        break;
                    }
                current = next;
                next = SearchNextVertex(current);
            }
            markEdges.ForEach((e) => { e.Mark = false; });
            return next;
        }
        static int Numeration(List<MyEdge> graph, int first, bool fullNum)
        {
            MyVertex current_vertex = SearchFirstVertex(graph),
                next_vertex = SearchNextVertex(current_vertex);
            while (next_vertex != null)
            {
                foreach (MyEdge edge in current_vertex.List_of_edge)
                {
                    if (edge.Next == next_vertex && edge.Preview == current_vertex || edge.Next == current_vertex && edge.Preview == next_vertex)
                    {
                        edge.Mark = true;
                        break;
                    }
                }
                var new_graph = new List<MyEdge>();
                current_vertex.List_of_edge.ForEach((e) => { if (!e.Mark) new_graph.Add(e); });
                if (new_graph.Count != 0)
                {
                    new_graph = BuildGraph(new_graph);
                    if (fullNum) InitGraph(new_graph, new_graph.Count + 1);
                    first = Numeration(new_graph, first, fullNum);
                }
                else
                {
                    current_vertex.Number = first++;
                }
                current_vertex = next_vertex;
                next_vertex = SearchNextVertex(current_vertex);
            }
            current_vertex.Number = first++;

            return first;
        }
        static List<MyEdge> BuildGraph(List<MyEdge> graph)
        {
            graph.ForEach((edge) =>
            {
                edge.Next.List_of_edge.RemoveAll(x => x.Mark);
                edge.Preview.List_of_edge.RemoveAll(x => x.Mark);
            });
            List<MyEdge> new_graph = new List<MyEdge>();
            foreach (MyEdge edge in graph)
            {
                foreach (MyEdge next_edge in edge.Next.List_of_edge)
                    if (new_graph.IndexOf(next_edge) == -1) new_graph.Add(next_edge);
                foreach (MyEdge preview_edge in edge.Preview.List_of_edge)
                    if (new_graph.IndexOf(preview_edge) == -1) new_graph.Add(preview_edge);
            }
            return new_graph;
        }
    }
    class MyVertex
    {
        List<MyEdge> list_of_edge;
        int id, number;
        bool mark;
        public MyVertex(int pid)
        {
            id = pid;
            number = 0;
            list_of_edge = new List<MyEdge>();
            mark = false;
        }
        public List<MyEdge> List_of_edge
        {
            get
            {
                return list_of_edge;
            }
            set
            {
                list_of_edge = value;
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }
        public bool Mark
        {
            get
            {
                return mark;
            }
            set
            {
                mark = value;
            }
        }
    }
    class MyEdge
    {
        MyVertex preview, next;
        int number_vertex_preview, number_vertex_next;
        bool mark;
        public MyEdge(MyVertex vert1, MyVertex vert2)
        {
            preview = vert1;
            next = vert2;
            number_vertex_preview = 0;
            number_vertex_next = 0;
            mark = false;
        }
        public MyVertex Preview
        {
            get
            {
                return preview;
            }
            set
            {
                preview = value;
            }
        }
        public MyVertex Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }
        public int Number_vertex_preview
        {
            get
            {
                return number_vertex_preview;
            }
            set
            {
                number_vertex_preview = value;
            }
        }
        public int Number_vertex_next
        {
            get
            {
                return number_vertex_next;
            }
            set
            {
                number_vertex_next = value;
            }
        }
        public bool Mark
        {
            get
            {
                return mark;
            }
            set
            {
                mark = value;
            }
        }
    }
}
