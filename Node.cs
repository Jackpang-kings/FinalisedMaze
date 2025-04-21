using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.CSharp;
namespace MazeGame{
    class Node{
        int nodeid;
        int x,y;
        bool visited;
        List<Edge> edges;
        public Node(int id,int a,int b,List<Edge> e){
            nodeid = id;
            x = a;
            y = b;
            visited = false;
            edges = e;
        }
        public int X() {
            return x;
        }
        public int Y() {
            return y;
        }
        public int getNodeID(){
            return nodeid;
        }
        public bool IsVisited(){
            return visited;
        }
        public void Visited(bool b){
            visited = b;
        }
        public List<Edge> GetEdges(){
            return edges;
        }
        public Edge GetEdge(int i){
            return edges[i];
        }
        public void AddEdge(Edge e){
            edges.Add(e);
        }
        public override string ToString()
        {
            string node2str = "";
            node2str += $"NodeID:{nodeid}\n";
            node2str += $"X:{X()}Y:{Y()}\n";
            return node2str;
        }
    }

}