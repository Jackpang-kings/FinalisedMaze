    using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MazeGame{
    class Player : GameObject {
        protected List<object> inventory;
        protected object itemHeld;
        public Player(){
            x = 0;
            y = 0;
            name = "Player";
            itemHeld = null!;
            inventory = new List<object>();
        }
        public Player(int a,int b,string n){
            x = a;
            y = b;
            name = n;
            label = '8';
            itemHeld = null!;
            inventory = new List<object>();
        }
        public object GetInventory(int a){
            if (a<inventory.Count&&a>=0){
                return inventory[a--];
            }
            return null!;
        }
        public void AddItem(object i){
            inventory.Add(i);
        }
        public void Remove(int i){
            inventory.RemoveAt(i);
        }
        public object GetItemHeld(){
            if (itemHeld!=null){
                return itemHeld;
            }
            return null!;
        }
        public void Hold(int i){
            itemHeld = GetInventory(i);
        }
        public string DisplayInventory(){
            string message = "";
            int i = 1;
            foreach(object item in inventory){
                if (item is Navigator){
                    Navigator obj = (Navigator) item;
                    message+=$"{i}: {obj.GetName()},{obj.GetType()}\n";
                }
                i++;
            }
            return message;
        }
        public string CheckHold(int i){
            i--;
            if (i>=0&&i<inventory.Count){
                Hold(i);
            }
            if (itemHeld!=null){
                return itemHeld.ToString()+" is held";
            }
            return "Nothing";
        }
    }
}