using System;
using System.Formats.Asn1;
namespace MazeGame{
    class Edge{
        Node node;
        int distance;
        public Edge(Node n, int d){
            node = n;
            distance = d;
        }
        public Node GetNode() {
            return node;
        }
        public int GetDistance() {
            return distance;
        }
        
    }
}