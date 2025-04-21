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
            foreach (Cell c in _maze.GetGrid()){
                if (c.IsNewNode()){
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
        }        
        public (int,Cell) DFSCalculateDistance(Cell c){
            List<Cell> cellsToTraverse = new List<Cell>([c]);
            int d= 1;
            bool found = false;
            while (cellsToTraverse.Count>0&&!found){
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
                    c = cellsToTraverse[0];
                    c.Visited(true);
                }
            }
            return (d,c);
        }
        (Cell,Cell,int) FindTheNearestNode(Cell currentCell){
            List<(int,Cell,Cell)> list = new List<(int, Cell,Cell)>();
            currentCell.Visited(true);
            for(int k = 0;k<currentCell.connectedCells.Count;k++){
                var edge = DFSCalculateDistance(currentCell.connectedCells[k]);
                list.Add((edge.Item1,edge.Item2,currentCell.connectedCells[k]));
            }
            _maze.SetAllCellNotVisited();
            Cell newCell;
            int d;
            int index;
            if (list[0].Item1>list[1].Item1){
                index = 1;
                list[0].Item3.Visited(true);
            }else{
                index = 0;
                list[1].Item3.Visited(true);
            }
            currentCell = list[index].Item3;
            newCell = list[index].Item2;
            d = list[index].Item1;
            return (currentCell,newCell,d);
        }
        List<object> PathToNearestNode(Cell currentCell,Cell newCell){
            List<object> objs = new List<object>([new GameObject(newCell.X(),newCell.Y(),"Mark2Node",'#')]);
            List<object> fill = DFSBuildBridgeBetweenNodes(currentCell,cellNodeMap[newCell],'#');
            if (fill!=null){
                foreach(object o in fill){
                    objs.Add(o);
                }
            }
            _maze.SetAllCellNotVisited();
            return objs;
        }
        public List<object> ReturnPathToNearestNode(Cell currentCell){
            var cells = FindTheNearestNode(currentCell);
            return PathToNearestNode(cells.Item1,cells.Item2);
        }
        List<object> DFSBuildBridgeBetweenNodes(Cell c, Node endNode,char label){
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
                if (c.IsNewNode()){
                    found = true;
                }else{
                    c = cellsToTraverse[0];
                    cellsToTraverse.RemoveAt(0);
                    objs.Add(new GameObject(c.X(),c.Y(),"Mark",label)); 
                }
                c.Visited(true);
                foreach (Cell cell in c.connectedCells){
                    if (!cell.IsVisited()){
                        cellsToTraverse.Add(cell);
                    }
                }
            }
            return objs;
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
                foreach(object obj in DFSBuildBridgeBetweenNodes(_maze.GetMazeCell(startX,startY),nodes[i+1],label)){
                    list.Add(obj);
                }
                startX = nodes[i+1].X();
                startY = nodes[i+1].Y();
                _maze.SetAllCellNotVisited();
            }
            list.Add(new GameObject(endCell.X(),endCell.Y(),"Last Mark",label));
            return list;
        }
        public List<object> ReturnPath(Cell start, Cell end,int d,string method,char label){
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
            return d;
        }
    }
}