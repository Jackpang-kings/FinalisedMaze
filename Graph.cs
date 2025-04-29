using System;
using System.Formats.Asn1;
namespace MazeGame{
    class Graph{
        
        List<Node> nodes;
        
        public Graph(){
            nodes = new List<Node>();
        }
        public Graph(List<Node> n){
            nodes = n;
        }
        public void SetNewNodeList(){
            nodes = new List<Node>();
        }
        void SetAllNodeNotVisited(){
            foreach (var node in nodes){
                node.Visited(false);
            }
        }
        
        public List<Node> GetNodes(){
            return nodes;
        }
        
        public Node GetNode(int i){
            return nodes[i];
        }
        
        public void AddNode(Node n) {
            nodes.Add(n); 
        }
        
        public (List<Node>,int,int) BreadthFirstTraversal(Node startNode, Node endNode){
            // A list of distance from the startNode to every other node
            int [] distance = new int[nodes.Count];

            // Set all nodes as not visited
            SetAllNodeNotVisited();

            // Queue for storing neighbours, solution to return the path
            Queue<Node> queue = new Queue<Node>();
            List<Node> solution = new List<Node>();
            // Parent dictionary for reconstructing the path
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            //Declare start distance
            int distFromStartN;

            // Setup the node
            queue.Enqueue(startNode);
            startNode.Visited(true);
            while (queue.Count > 0 && !solution.Contains(endNode)) {
                Node currentNode = queue.Dequeue();
                solution.Add(currentNode);
                distFromStartN = distance[currentNode.getNodeID()];
                // Enqueue unvisited connected neighbors
                foreach (var edge in currentNode.GetEdges()){
                    Node n = edge.GetNode();
                    if (!n.IsVisited()) {
                        queue.Enqueue(n);
                        parent[n] = currentNode;
                        int d = edge.GetDistance();
                        distance[n.getNodeID()] = distFromStartN+d;
                        n.Visited(true);
                    }
                }
            }

            // Reconstruct the path
            List<Node> path = new List<Node>();
            Node current = endNode;
            while (current != null && parent.ContainsKey(current)) {
                path.Insert(0, current);
                current = parent[current];
            }

            if (current == startNode) {
                path.Insert(0, startNode); // Include the start node
            }
            int count= 0;
            foreach(int i in distance){
                if (i!=int.MaxValue){
                    count++;
                }
            }
            return (path,distance[endNode.getNodeID()],count);
        }

        public (List<Node>,int,int) DijkstraAlgorithm(Node startNode, Node endNode) {
            // Set all nodes as not visited
            SetAllNodeNotVisited();

            // A list of nodes stores the shortest path
            List<Node> solution  = new List<Node>();

            // A list of distance from the startNode to every other node
            int [] distance = new int[nodes.Count];
            int distTemp;
            int distFromStartN;
            // Set all integer into largest value possible
            foreach (Node n in GetNodes()){
                distance[n.getNodeID()] = int.MaxValue;
            }

            // Parent dictionary for reconstructing the shortest path
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            // Priority queue declare, enQuqueue startNode
            DistanceQueue q = new DistanceQueue();
            q.Enqueue(startNode,0);

            // Update queue to make startNode starts first
            q.UpdateQueue(startNode,0);
            distance[startNode.getNodeID()] = 0;

            // Loop starts until
            while (!q.IsEmpty()&&!endNode.IsVisited()){
                // Take this smallest distance node
                Node currentNode = q.Dequeue();

                // Give a list edges from smallest distance node
                List<Edge> edges = currentNode.GetEdges();

                distFromStartN = distance[currentNode.getNodeID()];
                foreach (Edge e in edges){
                    Node n = e.GetNode();
                    if (!n.IsVisited()){
                        // Get distance from graph.NodeList
                        int d = e.GetDistance();
                        distTemp = distFromStartN+d;
                        // if a shorter path is found
                        if (distTemp < distance[n.getNodeID()]){
                            //Update distance array
                            distance[n.getNodeID()] = distTemp;
                            
                            //Update Queue
                            if (!q.CheckNodeInQueue(n)){ 
                                q.Enqueue(n,distTemp);
                            }else{
                                q.UpdateQueue(n,distance[n.getNodeID()]);
                            }

                            // Update parent for path reconstruction
                            parent[n] = currentNode;
                        }
                    }
                }
                solution.Add(currentNode);
                currentNode.Visited(true);
            }

            // Reconstruct the shortest path
            List<Node> shortestPath = new List<Node>();
            Node current = endNode;
            while (current != null && parent.ContainsKey(current)) {
                shortestPath.Insert(0, current);
                current = parent[current];
            }

            if (current == startNode) {
                shortestPath.Insert(0, startNode); // Include the start node
            }
            int count = 0;
            foreach(int i in distance){
                if (i!=int.MaxValue){
                    count++;
                }
            }
            return (shortestPath,distance[endNode.getNodeID()],count);
        }
        
        public int GetDistanceBetweenNodes(Node node1,Node node2){
            int distance = -1;
            foreach (Edge e in node1.GetEdges()){
                if (e.GetNode()==node2){
                    distance = e.GetDistance();
                }
            }
            return distance;
        }
        
}
}