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
        static List<string> menuOptions = new List<string>() { "Surrender", "Move" };
        static int width = 26;
        static int height = 26;
        static string fasz = new string(' ', Console.WindowWidth / 2);
        static string turn = "blue";
        static int[] selectedTile = [0, 0];
        static bool endTurn = false;
        static int newX = -1;
        static int newY = -1;
        static string newRotation = string.Empty;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.White;


            blueplanes.Add(new Plane("KékRepulo", 0, 0, "bottom", 10, 5));

            blueplanes.Add(new Plane("KékRepulo2", 1, 0, "right", 10, 5));

            redplanes.Add(new Plane("PirosRepulo", 20, 20, "top", 10, 5));


            int answer = -1;
            while (answer != 0)
            {
                answer = -1;
                while (0 > answer || menuOptions.Count <= answer)
                {
                    RefreshScreen();
                    answer = DisplayMenu(menuOptions);
                }
                switch (answer)
                {
                    case -2: break;
                    case 0: return;
                    case 1:

                        Move();
                        break;
                    case -1: Console.WriteLine("HIBA"); break;//TODO HIBÁRA
                }
                if (endTurn)
                {
                    if (turn == "blue")
                    {
                        turn = "red";
                    }
                    else { turn = "blue"; }
                }
            }
        }
        static void RefreshScreen()
        {
            Console.Clear();
            DisplayBattlefield(width, height);
            WriteInformation();
        }

        static void GetMoveParams()
        {
            RefreshScreen();
            Console.SetCursorPosition(0, height + 5);
            try
            {
                if (newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Row: ", height + 5);
                    newY = int.Parse(Console.ReadLine());
                }

                if (newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Column: ", height + 5);
                    newX = int.Parse(Console.ReadLine());
                }

                if (newRotation == string.Empty)
                {
                    RefreshScreen();
                    WriteCentered("Rotation (top, bottom, left, right): ", height + 5);
                    newRotation = Console.ReadLine();
                }
                string[] asd = ["top", "bottom", "left", "right"];
                if (!asd.Contains(newRotation))
                {
                    newRotation = string.Empty;
                    GetMoveParams();
                }
            }
            catch
            {
                GetMoveParams();
            }
        }
        static void Move()
        {
            if (turn == "blue")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < blueplanes.Count; i++)
                {
                    string[] data = blueplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                GetMoveParams();
                blueplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
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
                int index = DisplayMenu(planeDatas);
                GetMoveParams();
                redplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
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
                WriteCentered($"{i} - {options[i]}", height + i + 4);
            }
            Console.WriteLine();
            WriteCentered("Answer: ", height + options.Count + 4);
            try
            {
                int answer = int.Parse(Console.ReadLine());
                return answer;
            }
            catch
            {
                return -1;
            }

        }
        private static void WriteCentered(string text, int line)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2, line);
            Console.Write(text);
        }
        static void DrawTile(int x, int y)
        {
            for (int i = 0; i < blueplanes.Count; i++)
            {
                if (i + 1 <= blueplanes.Count) if (blueplanes[i].X == x && blueplanes[i].Y == y)
                    {
                        DrawBluePlane(i, x, y);
                        return;
                    }

            }
            for (int i = 0; i < redplanes.Count; i++)
            {
                if (i + 1 <= redplanes.Count) if (redplanes[i].X == x && redplanes[i].Y == y)
                    {
                        DrawRedPlane(i, x, y);
                        return;
                    }
            }
            Console.Write("[ ]");
        }
        static void DrawBluePlane(int index, int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (blueplanes[index].Rotation == "top")
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
            else if (blueplanes[index].Rotation == "left")
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
        static void DrawRedPlane(int index, int x, int y)
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
            else if (redplanes[index].Rotation == "left")
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
                    Console.SetCursorPosition(10, 1 + j + 1 + i * data.Length + 1);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < redplanes.Count; i++)
            {
                string[] data = redplanes[i].ToString().Split(",");
                for (global::System.Int32 j = 0; j < data.Length; j++)
                {
                    Console.SetCursorPosition(170, 1 + j + 1 + i * data.Length + 1);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }
        }

    }
}
