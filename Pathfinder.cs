using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.CSharp;
namespace MazeGame{
    class PathFinder {
        public Graph graph;
        public MazeGrid _maze;
        Dictionary<Cell, Node> cellNodeMap;
        public PathFinder(MazeGrid m,Graph g) {
            _maze = m;
            graph = g;
            cellNodeMap = new Dictionary<Cell, Node>();
            int nodecounter = 0;
            for(int i = 0;i<_maze.Width();i++){
                for(int j = 0;j<_maze.Height();j++){
                    Cell c = _maze._mazeGrid[i,j];
                    if (IsNode(c)){
                        Node newNode = new Node(nodecounter, c.X(), c.Y(),new List<Edge>());
                        nodecounter++;
                        graph.AddNode(newNode);
                        cellNodeMap[c] = newNode;
                    }
                } 
            }
        }
        public void LinkNodeRelationships(){
            _maze.SetAllCellNotVisited();
            foreach (Cell c in _maze._mazeGrid){
                if (IsNode(c)){
                    CheckNodesRelationship(c);
                }
            }
        }
        public bool IsNode(Cell cell){
            if(cell.X()==0&&cell.Y()==0){
                return true;
            }else if (cell.X()==_maze.Width()-1&&cell.Y()==_maze.Height()-1){
                return true;
            }
            else if (cell.FrontWall&&cell.BackWall){
                if (cell.RightWall^cell.LeftWall){
                    return true;
                }
            }else if (cell.RightWall&&cell.LeftWall){
                if (cell.FrontWall^cell.BackWall){
                    return true;
                }
            }else if (!((cell.FrontWall&&cell.BackWall)|(cell.LeftWall&&cell.RightWall))){
                return true;
            }
            return false;
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
                    if (IsNode(_maze._mazeGrid[x,c.Y()])&&_maze._mazeGrid[x,c.Y()]!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[x,c.Y()]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }
                }
                found=false;
            }
            if (!c.RightWall){
                while(!found&&x<_maze.Width()-1){
                    x++;
                    if (IsNode(_maze._mazeGrid[x,c.Y()])&&_maze._mazeGrid[x,c.Y()]!=c){
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
                    if (IsNode(_maze._mazeGrid[c.X(),y])&&_maze._mazeGrid[c.X(),y]!=c){
                        prevNode = cellNodeMap[_maze._mazeGrid[c.X(),y]];
                        SetGraphRelationship(newNode,prevNode);
                        found=true;
                    }
                }
                found=false;
            }
            if (!c.BackWall){
                while(!found&&y<_maze.Height()-1){
                    y++;
                    if (IsNode(_maze._mazeGrid[c.X(),y])&&_maze._mazeGrid[c.X(),y]!=c){
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
            newNode.SetEdge(e);
        }
        public Node Cell2Node(Cell c){
            if (cellNodeMap.ContainsKey(c)){
                return cellNodeMap[c];
            }else{
                return null;
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
            if (IsNode(cell)){
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