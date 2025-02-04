using System;
using System.Collections.Generic;

namespace MazeGame{
    public class GameObject{
        public int x{ get; set; }
        public int y{ get; set; }
        public string name;
        public char label;
        public bool interaction;
        public bool movable;
        public GameObject(){
            x=0;
            y=0;
            name="";
            label=' ';
            interaction = false;
            movable = false;
        }
        public GameObject(int a,int b,string n, char t,bool i, bool m){
            x = a;
            y = b;
            name = n;
            interaction = i;
            movable = m;
            label = t;
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
        public char GetLabel(){
            return label;
        }
    }
    class Player : GameObject {
        protected List<Item> inventory;
        protected int health;
        public Player(int a,int b,string n, List<Item> inv){
            x = a;
            y = b;
            name = n;
            health = 5;
            interaction = true;
            movable = true;
            label = '8';
            inventory = inv;
        }
        
    }
    class Item:GameObject {
        
    }
    class Mob : GameObject {
        protected int damage;    
        protected int range;

        
    }
}