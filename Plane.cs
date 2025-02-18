using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class Plane
    {
        public List<Weapon> Weapons { get; private set; } = [];
        string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        public int HP { get; private set; }
        public string Name { get; private set; }

        private string _rotation = "top";
        public int X { get; private set; }
        public int Y { get; private set; }
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
        public Plane(string name, int x, int y, string rotation, int hp, int speed, List<Weapon> weapons)
        {
            Name = name;
            X = x;
            Y = y;
            Rotation = rotation;
            Speed = speed;
            HP = hp;
            Weapons = weapons;
        }
        
        public void GetDamaged(int amount)
        {
            HP -= amount;
            if (HP < 1)
            {
                X = -100;
                Y = -100;
            }
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
            return $"Name: {Name},X: {X + 1},Y: {Y + 1},HP: {HP},Speed {Speed},";
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

        public void CalculatePossibleATK(int range)
        {
            PossibleMoveX.Clear();
            PossibleMoveY.Clear();
            for (int i = 0; i < range; i++)
            {
                switch (Rotation)
                {
                    case "top":
                        getCords(0, -1, i);
                        break;
                    case "right":
                        getCords(1, 0, i);
                        break;
                    case "left":
                        getCords(-1, 0, i);
                        break;
                    case "bottom":
                        getCords(0, 1, i);
                        break;
                }
            }
        }
        private void getCords(int xChange,  int yChange, int distance)
        {
            if (xChange != 0)
            {
                PossibleMoveX.Add(X + xChange * distance);
                for (global::System.Int32 i = 1; i <= distance + 2; i++)
                {
                    PossibleMoveY.Add(Y - distance / 2 + i );                    
                }
            }
            if (yChange != 0)
            {
                PossibleMoveY.Add(Y + yChange * distance);
                for (global::System.Int32 i = 0; i <= distance + 2; i++)
                {
                    PossibleMoveX.Add(X - distance / 2 + i - 1);
                }
            }
        }
    }
}
