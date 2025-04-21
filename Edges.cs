using System;
using System.Formats.Asn1;
namespace MazeGame{
    class Edge{
        Node node;
        Cell viaRoute;
        int distance;
        public Edge(Node n, int d,Cell c){
            node = n;
            distance = d;
            viaRoute = c;
        }
        public Node GetNode() {
            return node;
        }
        public int GetDistance() {
            return distance;
        }
        public Cell GetViaRoute() {
            return viaRoute;
        }
        
    }
}