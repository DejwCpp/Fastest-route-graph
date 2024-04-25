using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastest_route_graph.Resources.Class
{
    internal class Node
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public int Distance { get; set; }
        public int Path {  get; set; }

        public Node()
        {
            Id = 0;
            IsDone = false;
            Distance = int.MaxValue;
            Path = -1;
        }

        public List<Node> GetNodesId(int size)
        {
            List<Node> arr = new List<Node>();

            for (int i = 0; i < size; i++)
            {
                Node node = new Node();

                node.Id = i;

                arr.Add(node);
            }
            return arr;
        }
    }
}
