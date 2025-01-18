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
        public GameObject gameObject;
        bool visited, goal;
    
        public Cell(int X,int Y,bool go = false,GameObject g = null){
            x = X;
            y = Y;
            goal = go;
            visited = false;
            neighbourCells = new List<Cell>();
            connectedCells = new List<Cell>();
            gameObject = g;
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
        public bool IsNode(int w,int h){
            if((X()==0&&Y()==0)|(X()==w-1&&Y()==h-1)){
                return true;
            }else if (FrontWall&&BackWall&&!RightWall&&!LeftWall){
                return false;
            }else if (!FrontWall&&!BackWall&&RightWall&&LeftWall){
                return false;
            }else{
                return true;
            }
        }
        public void SetGameObject(ref GameObject g){
            gameObject = g;
        }       
        public GameObject GetGameObject(){
            return gameObject;
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
            if (gameObject!=null){
                message += $"{gameObject.ToString()}";
            }
            return message;
        }
    }
}