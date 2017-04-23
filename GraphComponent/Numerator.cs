using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphComponent
{
    public class Numerator
    {

        public static void Renumber(CustomGraph graph)
        {
            List<MyEdge> new_graph = ConvertGraph(graph);
            InitGraph(new_graph);
        }
        static List<MyEdge> ConvertGraph(CustomGraph graph)
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
        static void InitGraph(List<MyEdge> graph)
        {
            foreach (MyEdge edge in graph)
            {
                if (edge.Preview.List_of_edge.Count == 1)
                {
                    MyVertex tmp_vert = edge.Next;
                    edge.Next = edge.Preview;
                    edge.Preview = tmp_vert;
                    Recurs(edge, edge.Next, graph.Count);
                    break;
                }
                if (edge.Next.List_of_edge.Count == 1)
                {
                    Recurs(edge, edge.Next, graph.Count);
                    break;
                }
            }            
        }
        static int Recurs(MyEdge edge,MyVertex vert,int kolvo)
        {
            if (edge.Number_vertex_preview == 0) 
            {
                if (edge.Next != vert)
                {
                    MyVertex tmp_vert = edge.Next;
                    edge.Next = edge.Preview;
                    edge.Preview = tmp_vert;
                }
                if (edge.Preview.List_of_edge.Count != 1)
                {
                    foreach (MyEdge tmp in edge.Preview.List_of_edge)
                    {
                        if (tmp != edge) edge.Number_vertex_preview += Recurs(tmp, edge.Next, kolvo);
                    }
                    edge.Number_vertex_preview++;
                    edge.Number_vertex_next = kolvo - edge.Number_vertex_preview + 1;
                }
                else
                {
                    edge.Number_vertex_preview = 1;
                    edge.Number_vertex_next = kolvo;
                }                   
            }
            return edge.Number_vertex_preview;
        }
        static void Numeration(List<MyEdge> graph,int first,int last)
        {
            MyVertex current_vertex = graph[0].Next, next_vertex = null;
            int max;
            while (true)
            {
                max = int.MinValue;
                foreach (MyEdge edge in current_vertex.List_of_edge)
                    if (current_vertex == edge.Next && !edge.Next.IsWatch)
                    {
                        if (edge.Number_vertex_preview > max)
                        {
                            max = edge.Number_vertex_preview;
                            next_vertex = edge.Preview;
                        }
                    }
                    else
                    {
                        if (edge.Number_vertex_next > max)
                        {
                            max = edge.Number_vertex_next;
                            next_vertex = edge.Next;
                        }
                    }
                current_vertex.IsWatch = true;
                current_vertex = next_vertex;
                if (current_vertex.List_of_edge.Count == 1) break;
            }
            foreach (MyEdge edge in graph)
            {
                edge.Next.IsWatch = false;
                edge.Preview.IsWatch = false;
            }
            current_vertex.Number = first;
            first++;
            List<MyEdge> new_graph = new List<MyEdge>();
            MyEdge tmp = null;
            while (true)
            {
                max = int.MinValue;
                foreach (MyEdge edge in current_vertex.List_of_edge)
                    if (current_vertex == edge.Next && !edge.Next.IsWatch)
                    {
                        if (edge.Number_vertex_preview > max)
                        {
                            max = edge.Number_vertex_preview;
                            next_vertex = edge.Preview;
                            tmp = edge;
                        }
                    }
                    else
                    {
                        if (edge.Number_vertex_next > max)
                        {
                            max = edge.Number_vertex_next;
                            next_vertex = edge.Next;
                            tmp = edge;
                        }
                    }
                foreach (MyEdge edge in current_vertex.List_of_edge)
                    if (edge != tmp) new_graph.Add(edge);
                if (new_graph.Count!=0)
                {
                    //ClearGraph(new_graph);
                    InitGraph(new_graph);

                    //Numeration(new_graph,first,)
                }
                current_vertex.IsWatch = true;
                current_vertex = next_vertex;
                if (current_vertex.List_of_edge.Count == 1) break;
            }
        }
    }
    class MyVertex
    {
        public MyVertex(int pid)
        {
            id = pid;
            number = 0;
            list_of_edge = new List<MyEdge>();
            IsWatch = false;
        }
        List<MyEdge> list_of_edge;
        int id,number;
        bool is_watch;
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
        public bool IsWatch
        {
            get
            {
                return is_watch;
            }
            set
            {
                is_watch = value;
            }
        }
    }
    class MyEdge
    {
        MyVertex preview, next;
        int number_vertex_preview, number_vertex_next;
        public MyEdge(MyVertex vert1,MyVertex vert2)
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
