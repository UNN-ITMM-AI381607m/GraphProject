using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using GraphSharp.Controls;
using System.Windows.Input;

namespace GraphComponent
{
    public class CustomGraphLayout : GraphLayout<CustomVertex, CustomEdge, Tree> 
    {
        public CustomGraphLayout() : base()
        {
            DestructionTransition = null;
            CreationTransition = null;
        }
    }
}
