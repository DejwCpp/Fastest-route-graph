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
        public int Distance { get; set; }
        public int Path {  get; set; }
        public System.Drawing.PointF Position { get; set; }

        public Node()
        {
            Id = 0;
            Distance = int.MaxValue;
            Path = -1;
            Position = new System.Drawing.PointF();
        }

        public Node GetNodeFromThisLocation(System.Drawing.PointF point, List<Node> nodes)
        {
            return nodes.Find(n => n.Position.X == point.X && n.Position.Y == point.Y);
        }
    }
}
