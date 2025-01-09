using System;
using System.Formats.Asn1;
namespace MazeGame{
    class OldGraph{
        public List<Node> nodes;
        public int[,] NodeMap;
        public Dictionary<int,List<(int id,int dist)>> NodeList;
        public int size;

        public OldGraph(int n){
            nodes = new List<Node>(n);
            NodeMap = new int[n,n];
            NodeList = new Dictionary<int, List<(int, int)>>();
            size = n;
        }
        public Node GetNode(int i){
            return nodes[i];
        }
        public void AddNode(Node n) {
            nodes.Add(n); 
        }
        public int GetCostBetweenNodes(int nodeID1,int nodeID2) {
            int cost;
            cost = NodeMap[nodeID1,nodeID2];
            return cost;
        }
        public void AddNodesInNodeMap(int n1,int n2,int val){
            NodeMap[n1,n2]=val;
        }
        public void AddNodesInNodeList(int n1,List<(int,int)> values){
            NodeList.Add(n1,values);
        }
        public List<(int,int)> RetrieveNeighbours(Node n){
            return NodeList[n.getNodeID()];
        }
        public List<(int,int)> BubbleSort(List<(int id,int dist)> list2sort){
            bool swapped = true;
            (int a,int b) temp;
            int i = 0;
            int l = list2sort.ToArray().Length-1;
            while (swapped == true){
                swapped = false;
                for (int j = 0; j < l-i;j++){
                    if (list2sort[j].dist>list2sort[j+1].dist){
                        temp = list2sort[j+1];
                        list2sort[j+1] = list2sort[j];
                        list2sort[j] = temp;
                        swapped = true;
                    }
                }
                i++;
            }
            return list2sort;
            
        }
        public string PrintNodeMap(){
            string message="";
            for (int i = 0;i < NodeMap.GetLength(0);i++){
                for (int j = 0;j < NodeMap.GetLength(1);j++){
                    if (NodeMap[i,j]!=0){
                        message+=$"({nodes[i].X()},{nodes[i].Y()})".PadRight(9);
                        message+=$"({nodes[j].X()},{nodes[j].Y()})".PadRight(9);
                        message+=$"{NodeMap[i,j]}";
                        message+="\n";
                    }
                }
            }
            return message;
        }
        public string PrintNMap(){
            int [,]matrix = NodeMap;
            int size = matrix.GetLength(0);
            string result = "";
            // Add the header row
            result += "    |";
            for (int i = 0; i < size; i++){
                result += $"{i,3}";
            }
            result += "\n";
            // Add a separator line
            result += "----+" + new string('-', size * 4 - 1) + "\n";
            // Add the matrix rows with row indices
            for (int i = 0; i < size; i++){
                result += $"{i,3} |";
                for (int j = 0; j < size; j++){
                    result += $"{matrix[i, j],3}";
                }
                result += "\n";
            }
            return result;
        }
        public string PrintNodeList(){
            string message="";
            foreach(var nodepair in NodeList){
                int n = nodepair.Key;
                List<(int n,int val)> nodeNcosts = nodepair.Value;
                message+=$"({nodes[n].X()},{nodes[n].Y()})".PadRight(8);
                message+=":";
                foreach(var nNc in nodeNcosts){
                    message+=$"({nodes[nNc.n].X()},{nodes[nNc.n].Y()}),{nNc.val}|".PadLeft(11);
                }
                message+="\n";
            }
            return message;
        }
        public int GetDistanceBetweenNodes(Node node1,Node node2){
            int distance;
            if (node1.X()==node2.X()){// in the same row
                //find the distance vertically
                distance = Math.Abs(node1.Y()-node2.Y());
            }else if (node1.Y()==node2.Y()){
                distance = Math.Abs(node1.X()-node2.X());
            }else{
                return -1;
            }
            return distance;
        }
        public List<Node> GetNodesInNodeList(List<(int id,int dist)> l){
            List<Node> list = new List<Node>();
            foreach (var nodeedge in l){
                list.Add(nodes[nodeedge.id]);
            }
            return list;
        }
        public int GetDistanceInNodeList(Node n1,Node n2){
            var nodeedges = NodeList[n1.getNodeID()];
            foreach ((int id, int dist) n in nodeedges){
                if (n.id == n2.getNodeID()){
                    return n.dist;
                }
            }
            return 0;
        }
        public void SetAllNodeNotVisited(){
            foreach (var node in nodes){
                node.Visited(false);
            }
        }
}
}
