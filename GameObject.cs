using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MazeGame{
    public class GameObject{
        public int x{ get; set; }
        public int y{ get; set; }
        public string name;
        public char label;
        public GameObject(){
            x=0;
            y=0;
            name="";
            label=' ';
            
        }
        public GameObject(int a,int b,string n, char t, bool m){
            x = a;
            y = b;
            name = n;
            label = t;
        }
        public int X(){
            return x;
        }
        public int Y(){
            return y;
        }
        public string GetName(){
            return name;
        }

        public void Move(int a, int b){
            x=a;
            y=b;
    }
        public override string ToString()
        {
            return $"GameObject(Name: {name}, Position: ({x}, {y}))";
        }
        public char GetLabel(){
            return label;
        }
    }

    class Navigator : GameObject {
        public PathFinder pathFinder;
        protected int distance;
        public Navigator(PathFinder p){
            x=0;
            y=0;
            name = "Navigator";
            pathFinder = p;
        }
        public Navigator(int a, int b,string n,PathFinder cF,int d){
            x = a;
            y = b;
            name = n;
            label = 'N';
            pathFinder = cF;
            distance = d;
        }
        public int GetDistance(){
            return distance;
        }
    }
    class Mob : GameObject {
        protected int damage;
        protected int range;
        protected int health;
        protected bool following;
        protected List<object> objectToFollow;
        public PathFinder pathFinder;
        public Mob(int a,int b,string n,int c, int d, int h,PathFinder p){
            x = a;
            y = b;
            name = n;
            health = h;
            damage = c;
            range = d;
            label = 'M';
            pathFinder = p;
            following = false;
            objectToFollow = new List<object>();
        }
        public bool GetFollowing(){
            return following;
        }
        public List<object> GetobjectsToFollow(){
            return objectToFollow;
        }
        public GameObject PopObjToFollow(){
            if (objectToFollow.Count>0){
                GameObject obj = (GameObject)objectToFollow[0];
                objectToFollow.RemoveAt(0);
                return obj;
            }
            return null!;
        }
        public void SetObjectToFollow(List<object> objs){
            objectToFollow = objs;
        }       
        public void Tick(Cell currentCell){
            CheckIfPlayerIsWithinRange(currentCell);
            if (objectToFollow!=null&&following==true){
                try{
                    GameObject holder = (GameObject)objectToFollow[0];
                    x = holder.x;
                    y = holder.y;
                    objectToFollow.RemoveAt(0);
                }catch{
                    objectToFollow = pathFinder.ReturnPath(pathFinder._maze.GetMazeCell(x,y),currentCell,range,'#');
                }           
            }else{
                Cell nextCell = pathFinder._maze.NeighbourCell(pathFinder._maze.GetMazeCell(x,y).connectedCells,false);
                Move(nextCell.X(),nextCell.Y());
            }
        }
        public void CheckIfPlayerIsWithinRange(Cell c){
            Node startNode = pathFinder.Cell2Node(pathFinder._maze.GetMazeCell(x,y));  // Starting cell
            Node endNode = pathFinder.Cell2Node(pathFinder._maze.GetMazeCell(c.X(),c.Y()));    // Goal cell
            // if node is null
            if (startNode==null|endNode==null){
                following = false;
            }else{
                var solution = pathFinder.graph.DijkstraAlgorithm(startNode!,endNode!);
                if (solution.Item2[endNode!.getNodeID()]<=range){
                    following = true;
                }
            }
        }
    }
    class Tool:GameObject {
        protected bool breakWall;
        protected bool openWall;
        public Tool(int a, int b, string n,bool br, bool op){
            x = a;
            y = b;
            name = n;
            label = 'T';
            breakWall = br;
            openWall = op;
        }
        public void BreakWall(MazeGrid maze){

        }
        public void OpenWall(MazeGrid maze,string direction){
            Cell currentCell = maze.GetMazeCell(x,y);
            if(currentCell != null){
                int x = currentCell.X();
                int y = currentCell.Y();
                if (direction=="UP"){
                    if (!currentCell.FrontWall&&y-1>=0){
                        
                    }
                }else if (direction=="DOWN"){
                    if (y+1<maze.Height()&&!currentCell.BackWall){
                        
                    }
                }else if (direction=="LEFT"){
                    if (!currentCell.LeftWall&&x-1>=0){
                        
                    }
                }else if (direction=="RIGHT"){
                    if (!currentCell.RightWall&&x+1<maze.Width()){
                        
                    }
                }
            }
        }
    }
    class Weapon:GameObject {
        protected int damage;
        public Weapon(int a, int b, string n, int d){
            x = a;
            y = b;
            name = n;
            label = 'W';
            damage = d;
        }
    }
    class Food:GameObject {
        protected int heal;
        public Food(int a, int b, string n, int h){
            x = a;
            y = b;
            name = n;
            label = 'F';
            heal = h;
        }
        public int GetHeal(){
            return heal;
        }
    }
}