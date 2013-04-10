using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace AOA
{
    class Base
    {
        private int health;
        private int damage;
        private double height;
        private double width;
        private char direction;
        private Texture2D texture;

        public Base(int hp, char direct, double hei, double wid, int dmg, Texture2D img)
        {
            health = hp;
            direction = direct;
            height = hei;
            width = wid;
            damage = dmg;
        }

        public Base(Texture2D img)
        {
            texture = img;
        }

        public int Health
        {
            get { return health; }
        }

        public char Direction
        {
            get { return direction; }
        }

        public double Height
        {
            get { return height; }
        }

        public double Width
        {
            get { return width; }
        }

        public int Damage
        {
            get { return damage; }
        }
    }
}