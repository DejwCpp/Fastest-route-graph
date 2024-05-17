using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastest_route_graph.Resources.Class
{
    internal class Graph
    {
        public List<List<int>> matrix = new List<List<int>>();
        public List<Node> Q = new List<Node>();
        private List<Node> S = new List<Node>();
        private int numOfNodes;

        public void FindFastestRoute()
        {
            Q[0].Distance = 0;

            numOfNodes = Q.Count;

            while (Q.Count > 0)
            {
                Node actualNode = FindNodeIdForLowestDistance(Q);

                DeleteNodeWithId(actualNode.Id, Q);
                S.Add(actualNode);

                // find connections between nodes according to matrix
                for (int i = 0; i < numOfNodes; i++)
                {
                    if (matrix[actualNode.Id][i] != 0 && FindNodeWithId(i, Q))
                    {
                        Node node = GetNodeWithId(i, Q);

                        if (node.Distance > actualNode.Distance + matrix[actualNode.Id][i])
                        {
                            node.Distance = actualNode.Distance + matrix[actualNode.Id][i];
                            node.Path = actualNode.Id;
                        }
                    }
                }
            }
        }

        private Node FindNodeIdForLowestDistance(List<Node> nodes)
        {
            int lowestDistance = int.MaxValue;
            Node result = new Node();

            foreach (Node node in nodes)
            {
                if (node.Distance < lowestDistance)
                {
                    lowestDistance = node.Distance;
                    result = node;
                }
            }
            return result;
        }

        private Node GetNodeWithId(int id, List<Node> nodes)
        {
            return nodes.Find(n => n.Id == id);
        }

        private bool FindNodeWithId(int id, List<Node> nodes)
        {
            return GetNodeWithId(id, nodes) != null;
        }

        private void DeleteNodeWithId(int id, List<Node> nodes)
        {
            Node nodeToRemove = GetNodeWithId(id, nodes);

            if (nodeToRemove != null)
            {
                nodes.Remove(nodeToRemove);
            }
        }

        private List<Node> SetNodeWithId(int id, List<Node> nodes, Node toPut)
        {
            Node nodeToSet = GetNodeWithId(id, nodes);

            nodes[nodeToSet.Id] = toPut;

            return nodes;
        }
    }
}
