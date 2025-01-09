using System;
using System.Collections.Generic;

namespace MazeGame{
    class GameObject{
        public int x{ get; set; }
        public int y{ get; set; }
        string name;
        bool interaction;
        bool movable;
        public GameObject(int a,int b,string n,bool i, bool m){
            x = a;
            y = b;
            name = n;
            interaction = i;
            movable = m;
        }
        public string GetName(){
            return name;
        }
        public bool IsInteracting(){
            return interaction;
        }
        public void Interaction(bool val){
            interaction = val;
        }
        public bool IsMovable(){
            return movable;
        }
        public void Movable(bool val){
            movable = val;
        }
        public override string ToString()
        {
            return $"GameObject(Name: {name}, Position: ({x}, {y}), Interacting: {interaction}, Movable: {movable})";
        }
    }
    //class Player : GameObject {
        
        
    //}
    //class Item : GameObject {

        
    //}
    //class Mob : GameObject {
        
        
    //}
}