using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject
{
    public class CustomVertex
    {
        public int ID { get; private set; }

        public CustomVertex() { }

        public CustomVertex(int id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return string.Format("{0}", ID);
        }
    }
}
