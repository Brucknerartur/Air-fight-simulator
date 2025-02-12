using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class Plane
    {
        public int XCordinate { get; }
        public int YCordinate { get; }
        public string Rotation
        {
            get;
            set
            {
                if (value != "top" && value != "bottom" && value != "left" && value != "right")
                {
                    Rotation = "top";
                }
                Rotation = value;
            } 
        }
        public Plane(int x, int y, string rotation)
        {
            XCordinate = x;
            YCordinate = y;
            Rotation = rotation;
        }

        public void ResetRotaion()
        {
            Rotation = "top";
        }

        public void Move(int x, int y, string newRotation)
        {
            Rotation = newRotation;
        }
    }
}
