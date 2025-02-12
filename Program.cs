namespace Air_fight_simulator
{
    // ← → ↑ ↓
    internal class Program
    {
        static string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        static List<Plane> blueplanes = new List<Plane>();
        static List<Plane> redplanes = new List<Plane>();
        static List<AntiAir> antiAirs = new List<AntiAir>();
        static List<Weapon> weapons = new List<Weapon>(); 
        static List<string> menuOptions = new List<string>() {"Exit", "Move"};
        static int width = 30;
        static int height = 30;
        static string fasz = new string(' ', Console.WindowWidth / 2);

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Plane tesztRepülő = new Plane("Repulo", 10, 5, "bottom", 10, 5);
            blueplanes.Add(tesztRepülő);
            blueplanes.Add(new Plane("Repulo2", 10, 5, "bottom", 10, 5));

            Plane tesztRepülő2 = new Plane("Repulo", 20, 20, "top", 10, 5);
            redplanes.Add(tesztRepülő2);
            int answer = -1;
            while (0 > answer || menuOptions.Count <= answer)
            {
                Console.Clear();
                DisplayBattlefield(width, height);
                WriteInformation();
                answer = DisplayMenu();
            }
            switch (answer)
            {
                case 0: return;
                case 1: break;
            }
        }

        static int DisplayMenu()
        {
            Console.WriteLine();
            Console.SetCursorPosition(0, height + 2);
            for (int i = 0; i < menuOptions.Count; i++)
            {
                WriteCentered($"{i} - {menuOptions[i]}");
            }
            WriteCentered("Answer: ");
            try
            {
                int answer = int.Parse(Console.ReadLine());;
                return answer;
            }
            catch
            {
                return -1;
            }

        }

        private static void WriteCentered(string text)
        {
            Console.WriteLine(fasz + text);
        }

        static void DrawTile(int x, int y)
        {
            for (int i = 0; i < blueplanes.Count; i++)
            {
                if (i + 1 <= blueplanes.Count ) if( blueplanes[i].X == x && blueplanes[i].Y == y)
                {
                    BluePlaneRotaion(i);
                }
                else if (i + 1 <= redplanes.Count )
                {
                    if (redplanes[i].X == x && redplanes[i].Y == y)
                    { 
                        RedPlaneRotation(i);
                    }
                }
                else
                {
                    Console.Write("[ ]");
                }
            }
        }

        static void BluePlaneRotaion(int index)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if(blueplanes[index].Rotation == "top")
            {
                Console.Write("[↑]");
            }
            else if (blueplanes[index].Rotation == "right")
            {
                Console.Write("[→]");
            }
            else if (blueplanes[index].Rotation == "bottom")
            {
                Console.Write("[↓]");
            }
            else if (blueplanes[index].Rotation == "right")
            {
                Console.Write("[←]");
            }
            else
            {
                blueplanes[index].ResetRotaion();
                Console.Write("[↑]");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void RedPlaneRotation(int index)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (redplanes[index].Rotation == "top")
            {
                Console.Write("[↑]");
            }
            else if (redplanes[index].Rotation == "right")
            {
                Console.Write("[→]");
            }
            else if (redplanes[index].Rotation == "bottom")
            {
                Console.Write("[↓]");
            }
            else if (redplanes[index].Rotation == "right")
            {
                Console.Write("[←]");
            }
            else
            {
                redplanes[index].ResetRotaion();
                Console.Write("[↑]");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void DisplayBattlefield(int width, int height)
        {
            if (width > abc.Length)
            {
                width = abc.Length;
            }
            if (height > abc.Length)
            {
                height = abc.Length;
            }
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - width * 3 / 2, i + 3);
                Console.Write(abc[i]);
                for (int j = 0; j < height; j++)
                {
                    DrawTile(j, i);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - width * 3 / 2, height + 3);
            Console.Write(" ");
            for (int i = 0; i < width; i++)
            {
                if (i < 10)
                {
                    Console.Write($" {i + 1} ");
                }
                else
                {
                    Console.Write($"{i + 1} ");
                }
            }
        }
        static void CreateWeapons()
        {
            weapons.Add(new Weapon("Minigun", "gun", 100, 20, 10, 1));
            weapons.Add(new Weapon("Heat-seeking Rockets", "rocket", 8, 40, 2, 2));

        }
        static void WriteInformation()
        {
            
            for (int i = 0; i < blueplanes.Count; i++)
            {
                for (global::System.Int32 j = 0; j < blueplanes[i].ToString().Split(",").Length; j++)
                {
                    Console.SetCursorPosition(1, 10);
                    Console.Write(blueplanes[i].ToString().Split(",")[j]);
                }
            }
        }

    }
}
