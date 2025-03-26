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
                    if (c.IsNode()){
                        Node newNode = new Node(nodecounter, c.X(), c.Y(),new List<Edge>());
                        nodecounter++;
                        graph.AddNode(newNode);
                        cellNodeMap[c] = newNode;
                    }
                } 
            }
            LinkNodeRelationships();
        }
        public void LinkNodeRelationships(){
            foreach (Cell c in _maze._mazeGrid){
                if (c.IsNode()){
                    CheckNodesRelationship(c);
                }
            }
        }
        public void CheckNodesRelationship(Cell c){
            Node prevNode;
            Node newNode = cellNodeMap[c];
            bool found = false;
            int x = c.X();
            int y = c.Y();
            if (!c.LeftWall){
                while(!found&&x>=1){
                    x--;
                    if (_maze.GetMazeCell(x,c.Y()).IsNode()&&_maze.GetMazeCell(x,c.Y())!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[x,c.Y()]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }
                }
                found=false;
            }
            x = c.X();
            if (!c.RightWall){
                while(!found&&x<_maze.Width()-1){
                    x++;
                    if (_maze.GetMazeCell(x,c.Y()).IsNode()&&_maze.GetMazeCell(x,c.Y())!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[x,c.Y()]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }  
                }
                found=false;
            }
            if (!c.FrontWall){
                while(!found&&y>=1){
                    y--;
                    if (_maze.GetMazeCell(c.X(),y).IsNode()&&_maze.GetMazeCell(c.X(),y)!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[c.X(),y]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }
                }
                found=false;
            }
            y = c.Y();
            if (!c.BackWall){
                while(!found&&y<_maze.Height()-1){
                    y++;
                    if (_maze.GetMazeCell(c.X(),y).IsNode()&&_maze.GetMazeCell(c.X(),y)!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[c.X(),y]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }
                }
            }
        }
        public void SetGraphRelationship(Node newNode, Node prevNode){
            int d = graph.GetDistanceBetweenNodes(newNode,prevNode);
            Edge e = new Edge(prevNode,d);
            newNode.AddEdge(e);
        }
        public Node Cell2Node(Cell c){
            if (cellNodeMap.ContainsKey(c)){
                return cellNodeMap[c];
            }else{
                return null!;
            }
        }
        public List<object> ReturnPath(Cell currCell, Cell endCell, int d,char label){
            // Step 1: Declare variables
            List<object> list = new List<object>();

            // Step 2: Perform Dijkstra's Algorithm between two nodes
            Node startNode = Cell2Node(currCell);  // Starting cell
            Node endNode = Cell2Node(endCell);    // Goal cell
            List<Node> solution = new List<Node>();
            int[] distance;
            (solution,distance) = graph.DijkstraAlgorithm(startNode,endNode);

            int startX = currCell.X();
            int startY = currCell.Y();
            // Step 3: Output the solution
            List<Node> nodes = solution;
            for (int i = 0; i < nodes.Count-1;i++){
                int nx = nodes[i+1].X();
                int ny = nodes[i+1].Y(); 
                Cell nextCell = _maze.GetMazeCell(nx,ny);
                list.Add(new GameObject(startX,startY,"Mark "+i,label,false));
                //nextCell.SetGameObject(new GameObject(x,y,"Mark "+0,'#',false,false));
                int count = graph.GetDistanceBetweenNodes(nodes[i],nodes[i+1]);
            
            // Repeat n times in specific direction by comparing two nodes
                for(int j = 1; j < count;j++){
                    int a = startX;
                    int b = startY;
                    if (currCell.X()>nextCell.X()){
                        a = startX-j;
                    }else if (currCell.X()<nextCell.X()){
                        a = startX+j;
                    }else if (currCell.Y()>nextCell.Y()){
                        b = startY-j;
                    }else if (currCell.Y()<nextCell.Y()){
                        b=startY+j;
                    }
                    // Sets object marks
                    //nextCell = _maze._mazeGrid[a,b];
                    list.Add(new GameObject(a,b,"Mark "+i,label,false));
                }
                currCell = nextCell;
                startX = nx;
                startY = ny;
            }
            list.Add(new GameObject(endCell.X(),endCell.Y(),"Last Mark",label,false));
            if (list.Count<d|d==int.MaxValue){
                return list;
            }else{
                return list.GetRange(0,d);
            }   
        }
        public string PrintCellFrontWall(Cell[] cells){
            string message = "";
            foreach (Cell cell in cells){
                if (cell.FrontWall){
                message += "+---+";
                }else{
                message += "+   +";
                }
            }
            message +="\n";
            return message;
        }
        public string PrintCellLeftRightWall(Cell[] cells,List<Node> solution){ 
            string message = "";
            foreach (Cell cell in cells){
                if (cell.LeftWall){
                    message += "| ";
                }else{
                    message += "  ";
                }
                if (cell.IsNode()){
                    if (solution.Contains(cellNodeMap[cell])){
                        message += "#";
                    }else{
                        message += " ";
                    }
                }else{
                    message += " ";
                }
                if (cell.RightWall){
                    message += " |";
                }else{
                    message += "  ";
                }
            }
            message+="\n";
            return message;
        }
        public string PrintCellBackWall(Cell[] cells){
            string message = "";
            foreach (Cell cell in cells){
                if (cell.BackWall){
                    message += "+---+";
                }else{
                    message += "+   +";
                }
            }
            message+="\n";
            return message;       
        }
        public string PrintMazeSolution(List<Node> solution) {
        string mazeprintmessage ="   ";
        for (int i = 0;i<_maze.Width();i++){
            mazeprintmessage += $"{Convert.ToString(i),3}  ";
        }
        mazeprintmessage += "\n".PadRight(4);
        mazeprintmessage += PrintCellFrontWall(_maze.GetMazeRows(0));
        for (int j = 0;j<_maze.Height();j++){
            Cell[] rowOfCells = _maze.GetMazeRows(j);
            mazeprintmessage += $"{j,3}";
            mazeprintmessage += PrintCellLeftRightWall(rowOfCells,solution);
            mazeprintmessage += $"".PadLeft(3);
            mazeprintmessage += PrintCellBackWall(rowOfCells);
        }
        return mazeprintmessage;
    }
    }
}