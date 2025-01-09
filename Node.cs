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
        public void SetEdge(Edge e){
            edges.Add(e);
            BubbleSort();
        }
        public override string ToString()
        {
            string node2str = "";
            node2str += $"NodeID:{nodeid}\n";
            node2str += $"X:{X()}Y:{Y()}\n";
            return node2str;
        }
        public void BubbleSort(){
            bool swapped = true;
            Edge temp;
            int i = 0;
            int l = edges.Count-1;
            while (swapped == true){
                swapped = false;
                for (int j = 0; j < l-i;j++){
                    if (edges[j].GetDistance()>edges[j+1].GetDistance()){
                        temp = edges[j+1];
                        edges[j+1] = edges[j];
                        edges[j] = temp;
                        swapped = true;
                    }
                }
                i++;
            }
        }
    }

}