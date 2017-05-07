using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphComponent
{
    public class Numerator
    {
        public static Tree Renumber(Tree tree)
        {
            List<MyEdge> new_graph = ConvertGraph(tree);
            InitGraphRecurs(InitGraph(new_graph, tree.VertexCount), null, tree.VertexCount);
            Numeration(new_graph, 1);
            return BuildNewTree(new_graph, tree);
        }

        static Tree BuildNewTree(List<MyEdge> newGraph, Tree tree)
        {
            Dictionary<CustomVertex, int> idMap = new Dictionary<CustomVertex, int>();
            foreach (var edge in newGraph)
            {
                idMap[tree.Vertices.First(x => x.ID == edge.Preview.Id)] = edge.Preview.Number;
                idMap[tree.Vertices.First(x => x.ID == edge.Next.Id)] = edge.Next.Number;
            }

            foreach (var pair in idMap)
            {
                pair.Key.ID = pair.Value;
            }

            return tree;
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
        static MyEdge InitGraph(List<MyEdge> graph, int kolvo)
        {
            foreach (MyEdge edge in graph)
            {
                if (edge.Next.List_of_edge.Count == 1)
                {
                    edge.Number_vertex_next = 1;
                    edge.Number_vertex_preview = kolvo - 1;
                }
                if (edge.Preview.List_of_edge.Count == 1)
                {
                    edge.Number_vertex_preview = 1;
                    edge.Number_vertex_next = kolvo - 1;
                }
            }
            foreach (MyEdge edge in graph)
                if (edge.Number_vertex_next == 0) return edge;
            return null;
        }
        static void InitGraphRecurs(MyEdge current, MyEdge preview, int kolvo)
        {
            if (current != null)
            {
                foreach (MyEdge edge in current.Next.List_of_edge)
                    if (edge != current && edge != preview && edge.Number_vertex_next == 0) InitGraphRecurs(edge, current, kolvo);
                foreach (MyEdge edge in current.Next.List_of_edge)
                    if (edge.Number_vertex_next != 0 && edge != current)
                        if (current.Next == edge.Next) current.Number_vertex_next += edge.Number_vertex_preview;
                        else current.Number_vertex_next += edge.Number_vertex_next;
                current.Number_vertex_next++;
                current.Number_vertex_preview = kolvo - current.Number_vertex_next;
            }
        }
        static MyVertex SearchNextVertex(MyVertex current, MyEdge preview)
        {
            int max = int.MinValue;
            MyVertex next = null;
            foreach (MyEdge edge in current.List_of_edge)
                if (edge != preview)
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
            return next;
        }
        static MyVertex SearchFirstVertex(MyVertex current, MyEdge preview)
        {
            MyVertex next = SearchNextVertex(current, preview);
            while (next.List_of_edge.Count != 1)
            {
                foreach (MyEdge edge in next.List_of_edge)
                    if (edge.Next == next && edge.Preview == current || edge.Next == current && edge.Preview == next)
                    {
                        preview = edge;
                        break;
                    }
                current = next;
                next = SearchNextVertex(current, preview);
            }
            return next;
        }
        static int Numeration(List<MyEdge> graph, int first)
        {
            MyVertex preview_vertex = SearchFirstVertex(graph[0].Next, null), current_vertex = null, next_vertex = null;
            preview_vertex.Number = first++;
            MyEdge preview_edge = preview_vertex.List_of_edge[0], next_edge = null;
            current_vertex = SearchNextVertex(preview_vertex, null);
            while (true)
            {
                next_vertex = SearchNextVertex(current_vertex, preview_edge);
                if (next_vertex != null)
                {
                    foreach (MyEdge edge in current_vertex.List_of_edge)
                        if (edge.Next == next_vertex && edge.Preview == current_vertex || edge.Next == current_vertex && edge.Preview == next_vertex)
                        {
                            next_edge = edge;
                            break;
                        }
                    List<MyEdge> new_graph = new List<MyEdge>();
                    foreach (MyEdge edge in current_vertex.List_of_edge)
                        if (edge != preview_edge && edge != next_edge) new_graph.Add(edge);
                    if (new_graph.Count != 0)
                    {
                        BuildGraph(new_graph);
                        InitGraphRecurs(InitGraph(new_graph, new_graph.Count + 1), null, new_graph.Count + 1);
                        first = Numeration(new_graph, first);
                    }
                    else current_vertex.Number = first++;
                    current_vertex = next_vertex;
                    preview_edge = next_edge;
                }
                else
                {
                    current_vertex.Number = first++;
                    break;
                }
            }
            return first;
        }
        static void BuildGraph(List<MyEdge> graph)
        {
            foreach (MyEdge edge in graph)
            {
                for (int i = 0; i < edge.Next.List_of_edge.Count; i++)
                    if (graph.IndexOf(edge.Next.List_of_edge[i]) == -1)
                    {
                        edge.Next.List_of_edge.RemoveAt(i);
                        i--;
                    }
                for (int i = 0; i < edge.Preview.List_of_edge.Count; i++)
                    if (graph.IndexOf(edge.Preview.List_of_edge[i]) == -1)
                    {
                        edge.Preview.List_of_edge.RemoveAt(i);
                        i--;
                    }
            }
        }
    }
    class MyVertex
    {
        List<MyEdge> list_of_edge;
        int id, number;
        public MyVertex(int pid)
        {
            id = pid;
            number = 0;
            list_of_edge = new List<MyEdge>();
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
    }
    class MyEdge
    {
        MyVertex preview, next;
        int number_vertex_preview, number_vertex_next;
        public MyEdge(MyVertex vert1, MyVertex vert2)
        {
            preview = vert1;
            next = vert2;
            number_vertex_preview = 0;
            number_vertex_next = 0;
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
    }
}
