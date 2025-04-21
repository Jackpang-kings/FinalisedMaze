using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MazeGame{
    class GameObject{
        protected int x;
        protected int y;
        public string name;
        public char label;
        public GameObject(){
            x=0;
            y=0;
            name="";
            label=' ';
            
        }
        public GameObject(int a,int b,string n, char t){
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

    class Mob : GameObject {
        protected List<object> objectToFollow;
        public Mob(int a,int b,string n){
            x = a;
            y = b;
            name = n;
            label = 'M';
            objectToFollow = new List<object>();
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
    }
}