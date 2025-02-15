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
        public List<int> PossibleMoveX { get; private set; } = new List<int>();
        public List<int> PossibleMoveY { get; private set; } = new List<int>();
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
            return $"Name: {Name},X: {X + 1},Y: {Y + 1},HP: {CurrrentHP},Speed {Speed},";
        }

        public void CalcutePossibleMoves()
        {
            PossibleMoveX.Clear();
            PossibleMoveY.Clear();
            for (int i = 0; i < Speed * 2 + 1; i++)
            {
                for (global::System.Int32 j = 0; j < Speed * 2 + 1; j++)
                {
                    if (!PossibleMoveX.Contains(j + X - Speed))
                    {
                        PossibleMoveX.Add(j + X - Speed);
                    }
                    if (!PossibleMoveY.Contains(i + Y - Speed))
                    {
                        PossibleMoveY.Add(i + Y - Speed);
                    }
                }
            }
            PossibleMoveX.Sort();
            PossibleMoveY.Sort();
        }
    }
}
