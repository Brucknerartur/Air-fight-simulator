using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class Plane
    {
        string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        public int CurrrentHP { get; private set; }
        public string Name { get; private set; }
        private string _rotation;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int MaxHP { get; private set; }
        public int Speed { get; private set; }
        public string Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                if (value != "top" && value != "bottom" && value != "left" && value != "right")
                {
                    throw new Exception("incorrect rotation");
                }
                _rotation = value;
            } 
        }
        public Plane(string name,int x, int y, string rotation, int hp, int speed)
        {
            Name = name;
            X = x;
            Y = y;
            Rotation = rotation;
            MaxHP = hp;
            Speed = speed;
            CurrrentHP = hp;
        }

        public void ResetRotaion()
        {
            Rotation = "top";
        }

        public void Move(int x, int y, string newRotation)
        {
            Rotation = newRotation;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Name: {Name},X: {X},Y: {abc[Y]},HP: {CurrrentHP},Speed {Speed},";
        }
    }
}
