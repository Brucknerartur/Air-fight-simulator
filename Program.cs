using System.Diagnostics.CodeAnalysis;

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
        static List<string> menuOptions = new List<string>() {"Surrender", "Move"};
        static int width = 30;
        static int height = 30;
        static string fasz = new string(' ', Console.WindowWidth / 2);
        static string turn = "blue";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.White;


            blueplanes.Add(new Plane("KékRepulo", 0, 0, "bottom", 10, 5));

            blueplanes.Add(new Plane("KékRepulo2", 1, 0, "bottom", 10, 5));

            redplanes.Add(new Plane("PirosRepulo", 20, 20, "top", 10, 5));


            int answer = -1;
            while (0 > answer || menuOptions.Count <= answer)
            {
                RefreshScreen();
                answer = DisplayMenu(menuOptions);
            }
            switch (answer)
            {
                case 0: return;
                case 1:
                    
                    SelectPlane();
                    break ;
            }
            if (turn == "blue")
            {
                turn = "red";
            }
            else { turn = "blue"; }
        }

        static void RefreshScreen()
        {
            Console.Clear();
            DisplayBattlefield(width, height);
            WriteInformation();
        }

        static int SelectPlane()
        {
            if(turn == "blue")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < blueplanes.Count; i++)
                {
                    string[] data = blueplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " +data[2]);
                }
                RefreshScreen();
                return DisplayMenu(planeDatas);
            }
            else
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < redplanes.Count; i++)
                {
                    string[] data = redplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                return DisplayMenu(planeDatas);
            }
        }

        static void MovePlane(Plane plane, int newX, int newY, string rotaion)
        {
            plane.Move(newX, newY, rotaion);
            RefreshScreen();
        }

        static int DisplayMenu(List<string> options)
        {
            Console.WriteLine();
            Console.SetCursorPosition(0, height + 2);
            for (int i = 0; i < options.Count; i++)
            {
                WriteCentered($"{i} - {options[i]}");
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
                    return;
                }

            }
            for (int i = 0; i < redplanes.Count; i++)
            {
                if (i + 1 <= redplanes.Count) if (redplanes[i].X == x && redplanes[i].Y == y)
                {
                    RedPlaneRotation(i);
                    return;
                }
            }

             Console.Write("[ ]");

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
                string[] data = blueplanes[i].ToString().Split(",");
                for (global::System.Int32 j = 0; j < data.Length; j++)
                {
                    Console.SetCursorPosition(10, 5 + j + 1 + i * data.Length + 1);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < redplanes.Count; i++)
            {
                string[] data = redplanes[i].ToString().Split(",");
                for (global::System.Int32 j = 0; j < data.Length; j++)
                {
                    Console.SetCursorPosition(170, 5 + j + 1 + i * data.Length + 1);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }
        }

    }
}
