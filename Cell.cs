using System;
using System.Collections.Generic;

namespace MazeGame{
    class Cell{
        int x,y;
        public bool LeftWall { get; set; } = true;
        public bool RightWall { get; set; } = true;
        public bool FrontWall { get; set; } = true;
        public bool BackWall { get; set; } = true;

        public List<Cell> neighbourCells;
        public List<Cell> connectedCells;
        bool visited, goal;
    
        public Cell(int X,int Y,bool go = false){
            x = X;
            y = Y;
            goal = go;
            visited = false;
            neighbourCells = new List<Cell>();
            connectedCells = new List<Cell>();
        }
        public int X(){
            return x;
        }
        public int Y(){
            return y;
        }
        public bool IsVisited(){
            return visited;
        }
        public void Visited(bool b){
            visited = b;
        }
        public bool IsGoal(){
            return goal;
        }
        public void Goal(bool g){
            goal = g;
        }
        public void AddConnectedCell(Cell cell){
            connectedCells.Add(cell);
        }
        public List<Cell> GetConnectedCells(){
            return connectedCells;
        }
        public bool IsNode(){
            if((x==0&&y==0)|(goal)){
                return true;
            }else if (FrontWall&&BackWall&&!RightWall&&!LeftWall){
                return false;
            }else if (!FrontWall&&!BackWall&&RightWall&&LeftWall){
                return false;
            }else{
                return true;
            }
        }
        
        public override string ToString()
        {
            string message = "";
            message += $"x location: "+x.ToString()+"\n";
            message += $"y location: "+y.ToString()+"\n";
            message += $"leftWall: "+LeftWall.ToString()+"\n";
            message += $"rightWall: "+RightWall.ToString()+"\n";
            message += $"frontWall: "+FrontWall.ToString()+"\n";
            message += $"backWall: "+BackWall.ToString()+"\n";
            message += $"visited: "+visited.ToString()+"\n";
            message += "goal: "+goal.ToString()+"\n";
            return message;
        }
    }
}