using Air_fight_simulator;

internal static class ProgramHelpers
{
    public static string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
    public static List<Plane> blueplanes = [];
    public static List<Plane> redplanes = [];
    public static List<AntiAir> blueAntiAirs = [];
    public static List<Weapon> weapons = [];
    public static List<Weapon> antiAirWeapons = [];
    public static List<string> menuOptions = ["Surrender", "Move", "Shoot with Plane", "Shoot with Anti-Air", "Tutorial"];
    public static int width = 26;
    public static int height = 26;
    public static string turn = "Blue";
    public static int[] selectedTile = [0, 0];
    public static bool endTurn = false;
    public static int newX = -1;
    public static int newY = -1;
    public static string newRotation = string.Empty;
    public static int battlefieldLeftCoursorPos = Console.WindowWidth / 2 - width * 3 / 2;
    public static int battlefieldTopCoursorPos = 3;
    public static int activeIndex = -1;
    public static int answer = -1;
    public static string blank = new string(' ', Console.WindowWidth - 20);
    public static string message = "";
    public static void CreateAntiAirs()
    {
        List<Weapon> weaponsForAntiAirs = new List<Weapon>();
        weaponsForAntiAirs.Add(weapons[5]);
        blueAntiAirs.Add(new AntiAir("Oerlikon 35 mm twin cannon", 11, 10, weaponsForAntiAirs));
        blueAntiAirs.Add(new AntiAir("Oerlikon 35 mm twin cannon", 15, 10, weaponsForAntiAirs));

    }
    public static void CreatePlanes()
    {
        List<Weapon> weaponsForPlanes = new List<Weapon>();
        blueplanes.Add(new Plane("A-10C Thunderbolt II", 14, 7, "down", 2, 7, new List<Weapon>() { new Weapon(weapons[0]), new Weapon(weapons[3]) }));

        blueplanes.Add(new Plane("F-117 Nighthawk", 12, 7, "down", 2, 7, new List<Weapon>() { new Weapon(weapons[0]) }));

        redplanes.Add(new Plane("C-130H Hercules", 12, 20, "up", 4, 5, new List<Weapon>() { new Weapon(weapons[1]), new Weapon(weapons[4]) }));

        redplanes.Add(new Plane("F-15C/D Eagle", 13, 20, "up", 4, 5, new List<Weapon>() { new Weapon(weapons[2]), new Weapon(weapons[4]) }));

        redplanes.Add(new Plane("F-35A Lightning II", 14, 20, "up", 3, 6, new List<Weapon>() { new Weapon(weapons[0]), new Weapon(weapons[4]), new Weapon(weapons[3]) }));

    }
    public static void CreateWeapons()
    {
        weapons.Add(new Weapon("COW 37 mm gun ", "gun", 100, 20, 4, 1, 7)); // 0
        weapons.Add(new Weapon("Bordkanone BK 7,5 cannon", "gun", 30, 50, 1, 2, 9)); //1
        weapons.Add(new Weapon("M2 Browning machine gun", "gun", 100, 25, 6, 1, 5)); //2
        weapons.Add(new Weapon("AGM-142 Raptor", "rocket", 8, 50, 2, 3, 11)); //3
        weapons.Add(new Weapon("AS.34 Kormoran ", "rocket", 6, 60, 2, 4, 13));//4
        weapons.Add(new Weapon("Anti-air gun type 1", "anti-air", 50, 50, 5, 1, 7));//5
        weapons.Add(new Weapon("Anti-air gun type 2", "anti-air", 50, 40, 5, 1, 9));//6
    }
}