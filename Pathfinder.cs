using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using Microsoft.CSharp;
namespace MazeGame{
    class PathFinder {
        public Graph graph;
        public MazeGrid _maze;
        public Dictionary<Cell, Node> cellNodeMap;
        public PathFinder(MazeGrid m,Graph g) {
            _maze = m;
            graph = g;
            cellNodeMap = new Dictionary<Cell, Node>();
            int nodecounter = 0;
            
            for(int i = 0;i<_maze.Width();i++){
                for(int j = 0;j<_maze.Height();j++){
                    Cell c = _maze.GetMazeCell(i,j);
                    if (c.IsNewNode()){
                        Node newNode = new Node(nodecounter, c.X(), c.Y(),new List<Edge>());
                        nodecounter++;
                        graph.AddNode(newNode);
                        cellNodeMap[c] = newNode;
                    }
                } 
            }
            //LinkNodeRelationships();
            Link();
        }
        void Link(){
            foreach (Node n in graph.GetNodes()){
                Cell c = _maze.GetMazeCell(n.X(),n.Y());
                Node nodeToConnect;
                c.Visited(true);
                for(int i = 0;i<c.connectedCells.Count;i++){
                    var edge = DFSCalculateDistance(c.connectedCells[i]);
                    int d = edge.Item1;
                    nodeToConnect = cellNodeMap[edge.Item2];
                    SetNodeRelationship(cellNodeMap[c],nodeToConnect,c.connectedCells[i],d);
                }
                _maze.SetAllCellNotVisited();
            }
        }        
        (int,Cell) DFSCalculateDistance(Cell c){
            List<Cell> cellsToTraverse = new List<Cell>([c]);
            int d= 1;
            bool found = false;
            while (!found){
                c.Visited(true);
                c = cellsToTraverse[0];
                cellsToTraverse.RemoveAt(0);
                if (c.IsNewNode()){
                    found = true;
                }else{
                    d++;
                    foreach (Cell cell in c.connectedCells){
                        if (!cell.IsVisited()){
                            cellsToTraverse.Add(cell);
                        }
                    }
                }
            }
            return (d,c);
        }
        (Cell,Cell,int) FindTheNearestNode(Cell currentCell){
            List<(int distance,Cell nextNode,Cell viaRoute)> list = new List<(int, Cell,Cell)>();
            currentCell.Visited(true);
            for(int k = 0;k<currentCell.connectedCells.Count;k++){
                var edge = DFSCalculateDistance(currentCell.connectedCells[k]);
                list.Add((edge.Item1,edge.Item2,currentCell.connectedCells[k]));
            }
            _maze.SetAllCellNotVisited();
            Cell newCell;
            Node endNode = cellNodeMap[_maze.GetMazeCell(_maze.GetEndX(),_maze.GetEndY())];
            int d;
            int index;
            if (list.Count>1){
                if (graph.DijkstraAlgorithm(cellNodeMap[list[0].nextNode],endNode).Item2>graph.DijkstraAlgorithm(cellNodeMap[list[1].nextNode],endNode).Item2){
                    index = 1;
                }else{
                    index = 0;
                }
            }else{
                index = 0;
            }
            
            currentCell = list[index].viaRoute;
            newCell = list[index].nextNode;
            d = list[index].distance;
            return (currentCell,newCell,d);
        }
        List<object> PathToNearestNode(Cell currentCell,Cell newCell){
            List<object> objs = new List<object>();
            currentCell.Visited(true);
            List<object> fill = BuildBridgeBetweenNodes(currentCell,cellNodeMap[newCell],'#');
            if (fill!=null){
                foreach(object o in fill){
                    objs.Add(o);
                }
            }
            objs.Add(new GameObject(newCell.X(),newCell.Y(),"Mark2Node",'#'));
            _maze.SetAllCellNotVisited();
            return objs;
        }
        List<object> BuildBridgeBetweenNodes(Cell c, Node endNode,char label){
            Node startNode;
            if (cellNodeMap.ContainsKey(c)){
                startNode = cellNodeMap[c];
                foreach (Edge e in startNode.GetEdges()){
                    if (e.GetNode()==endNode){
                        c = e.GetViaRoute();
                    }
                }
            }
            List<Cell> cellsToTraverse = new List<Cell>([c]);
            List<object> objs = new List<object>();
            bool found = false;
            while (cellsToTraverse.Count>0&&!found){
                c.Visited(true);
                if (c.IsNewNode()){
                    found = true;
                }else{
                    objs.Add(new GameObject(c.X(),c.Y(),"Mark",label));
                    cellsToTraverse.RemoveAt(0); 
                    foreach (Cell cell in c.connectedCells){
                        if (!cell.IsVisited()){
                            cellsToTraverse.Add(cell);
                        }
                    }
                    c = cellsToTraverse[0];
                    
                }  
            }
            return objs;
        }
        public List<object> ReturnPathToNearestNode(Cell currentCell){
            if (!currentCell.IsNewNode()){
                var cells = FindTheNearestNode(currentCell);
                currentCell.Visited(true);
                return PathToNearestNode(cells.Item1,cells.Item2);
            }else{
                return null!;
            }
            
        }
        void SetNodeRelationship(Node newNode, Node prevNode,Cell c,int d){
            Edge e = new Edge(prevNode,d,c);
            newNode.AddEdge(e);
        }
        public Node Cell2Node(Cell c){
            if (cellNodeMap.ContainsKey(c)){
                return cellNodeMap[c];
            }else{
                return null!;
            }
        }
        public List<object> FillGaps(List<Node> nodes,char label){
            Cell currCell = _maze.GetMazeCell(nodes[0].X(),nodes[0].Y());
            Cell endCell = _maze.GetMazeCell(nodes[nodes.Count-1].X(),nodes[nodes.Count-1].Y());
            int startX = currCell.X();
            int startY = currCell.Y();
            List<object> list=new List<object>();
            for (int i = 0; i < nodes.Count-1;i++){
                list.Add(new GameObject(startX,startY,"Mark",label));
                _maze.GetMazeCell(startX,startY).Visited(true);
                foreach(object obj in BuildBridgeBetweenNodes(_maze.GetMazeCell(startX,startY),nodes[i+1],label)){
                    list.Add(obj);
                }
                startX = nodes[i+1].X();
                startY = nodes[i+1].Y();
            }
            list.Add(new GameObject(endCell.X(),endCell.Y(),"Last Mark",label));
            list.RemoveAt(0);
            _maze.SetAllCellNotVisited();
            return list;
        }
        public List<object> ReturnPath(Cell start, Cell end,int d,string method,char label){
            if (!cellNodeMap.ContainsKey(start)){
                start = FindTheNearestNode(start).Item2;
            }
            Node startNode = Cell2Node(start);
            Node endNode = Cell2Node(end);
            List<object> list = new List<object>();
            if (method.ToUpper()=="B"){
                list = FillGaps(graph.BreadthFirstTraversal(startNode,endNode).Item1,label);
            }else{
                list = FillGaps(graph.DijkstraAlgorithm(startNode,endNode).Item1,label);
            }
            if (list.Count<d|d==int.MaxValue){
                return list;
            }else{
                return list.GetRange(0,d);
            }   
        }
        public int FindDistanceUntilNode(Cell currentCell){
            Node currentNode;
            Node endNode = cellNodeMap[_maze.GetMazeCell(_maze.GetEndX(),_maze.GetEndY())];
            int d = 0;
            if (!currentCell.IsNewNode()){
                (Cell one ,Cell two,int d) cells = FindTheNearestNode(currentCell);
                currentNode = cellNodeMap[cells.two];
                d = cells.d;
            }else{
                currentNode = cellNodeMap[currentCell];
            }
            d += graph.DijkstraAlgorithm(currentNode,endNode).Item2;
            _maze.SetAllCellNotVisited();
            return d;
        }
    }
}