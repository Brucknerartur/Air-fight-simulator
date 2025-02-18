using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class AntiAir
    {
        public List<Weapon> Weapons { get; private set; } = [];
        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<int> PossibleMoveX { get; private set; } = new List<int>();
        public List<int> PossibleMoveY { get; private set; } = new List<int>();

        public AntiAir(string name, int x, int y, List<Weapon> weapons) 
        {
            Name = name;
            X = x;
            Y = y;
            Weapons = weapons;
        }
        public override string ToString()
        {
            return $"Name: {Name}, X: {X + 1}, Y: {Y + 1}";
        }
        public void CalculatePossibleATK(int range)
        {
            PossibleMoveX.Clear();
            PossibleMoveY.Clear();
            for (int i = 0; i < range * 2 + 1; i++)
            {
                for (global::System.Int32 j = 0; j < range * 2 + 1; j++)
                {
                    if (!PossibleMoveX.Contains(j + X - range))
                    {
                        PossibleMoveX.Add(j + X - range);
                    }
                    if (!PossibleMoveY.Contains(i + Y - range))
                    {
                        PossibleMoveY.Add(i + Y - range);
                    }
                }
            }
        }
    }
}
