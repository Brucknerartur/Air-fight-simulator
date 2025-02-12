using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class AntiAir
    {
        public string Name { get; }
        public int Ammo { get; private set; }
        public int Accuracy { get; private set; }
        public string Type { get; private set; }

        public AntiAir(string name, string type, int ammo, int accuracy) 
        {
            Name = name;
            Ammo = ammo;
            Accuracy = accuracy;
            Type = type;
        }
    }
}
