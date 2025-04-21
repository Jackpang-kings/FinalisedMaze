    using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MazeGame{
    class Player : GameObject {
        public Player(){
            x = 0;
            y = 0;
            name = "Player";
        }
        public Player(int a,int b,string n){
            x = a;
            y = b;
            name = n;
            label = '8';
        }
    }
}