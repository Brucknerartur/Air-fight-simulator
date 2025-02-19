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
        public int Range { get; private set; }
        public Weapon(string name, string type, int ammo, int accuracy, int atk, int dmg, int range) 
        {
            Name = name;
            Type = type;
            Ammo = ammo;
            Accuracy = accuracy;
            ATK = atk;
            DMG = dmg;
            Range = range;
        }

        public Weapon(Weapon weaponTemplate) 
        {
            Name = weaponTemplate.Name;
            Type = weaponTemplate.Type;
            Ammo = weaponTemplate.Ammo;
            Accuracy = weaponTemplate.Accuracy;
            ATK = weaponTemplate.ATK;
            DMG = weaponTemplate.DMG;
            Range = weaponTemplate.Range;
        }
         
        public void UseAmmo(int amount)
        {
            Ammo -= amount;
        }
        public override string ToString()
        {
            return $"Name: {Name}, Ammo: {Ammo}, Range: {Range}, Accuracy: {Accuracy}, ATK: {ATK}, DMG: {DMG}";
        }
    }
}
