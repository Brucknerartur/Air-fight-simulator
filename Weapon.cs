using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_fight_simulator
{
    internal class Weapon
    {
        public string Name { get; }
        public string Type { get; private set; }
        public int Ammo { get; private set; }
        public int  Accuracy { get; private set; }
        public int ATK { get; private set; }
        public int DMG {  get; private set; }
        public Weapon(string name,string type, int ammo, int accuracy, int atk, int dmg) 
        {
            Name = name;
            Type = type;
            Ammo = ammo;
            Accuracy = accuracy;
            ATK = atk;
            DMG = dmg;
        }
    }
}
