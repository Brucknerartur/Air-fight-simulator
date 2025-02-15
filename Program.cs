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
        static List<string> menuOptions = new List<string>() { "Surrender", "Move", "Shoot" };
        static int width = 26;
        static int height = 26;
        static string turn = "Blue";
        static int[] selectedTile = [0, 0];
        static bool endTurn = false;
        static int newX = -1;
        static int newY = -1;
        static string newRotation = string.Empty;
        static int battlefieldLeftCoursorPos = Console.WindowWidth / 2 - width * 3 / 2;
        static int battlefieldTopCoursorPos = 3;
        static int activePlaneIndex = -1;
        static int answer = -1;
        static string blank = new string(' ', Console.WindowWidth - 20);
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.White;


            blueplanes.Add(new Plane("KékRepulo", 20, 10, "bottom", 10, 5));

            redplanes.Add(new Plane("PirosRepulo", 20, 20, "top", 10, 5));


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
                    case -1: Console.WriteLine("HIBA"); break;
                }
                if (endTurn)
                {
                    if (turn == "Blue")
                    {
                        turn = "Red";
                    }
                    else { turn = "Blue"; }
                }
            }
        }
        static void RefreshScreen()
        {
            Console.Clear();
            WrtieTurn();
            DisplayBattlefield(width, height);
            WriteInformation();
            if (activePlaneIndex != -1)
            {
                if (answer == 1)
                {
                    ColorPossibleMoves(blueplanes[activePlaneIndex]);
                }
            }
        }

        static void ColorPossibleMoves(Plane plane)
        {
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(battlefieldLeftCoursorPos, battlefieldTopCoursorPos + i);
                if (10 > i + 1)
                {
                    Console.Write($"{i + 1} ");
                }
                else
                {
                    Console.Write(i + 1);
                }
                for (int j = 0; j < height; j++)
                {
                    if (plane.PossibleMoveY.Contains(i))
                    {
                        if (plane.PossibleMoveX.Contains(j))
                        {
                            DrawTile(j, i,false);
                        }
                        else
                        {
                            DrawTile(j, i, true);
                        }
                    }
                    else
                    {
                        DrawTile(j, i, true);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        static void GetMoveParams(Plane plane)
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
                    newY--;
                }
                if (!plane.PossibleMoveY.Contains(newY))
                {
                    newY = -1;
                    GetMoveParams(plane);
                }

                if (newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Column: ", height + 5);
                    newX = int.Parse(Console.ReadLine());
                    newX--;

                }
                if (!plane.PossibleMoveX.Contains(newX))
                {
                    newX = -1;
                    GetMoveParams(plane);
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
                    GetMoveParams(plane);
                }
            }
            catch
            {
                GetMoveParams(plane);
            }
        }
        static void Move()
        {
            if (turn == "Blue")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < blueplanes.Count; i++)
                {
                    string[] data = blueplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                activePlaneIndex = index;
                blueplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(blueplanes[index]);
                GetMoveParams(blueplanes[index]);
                blueplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
                activePlaneIndex = -1;
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
                activePlaneIndex = index;
                redplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(redplanes[index]);
                GetMoveParams(redplanes[index]);
                redplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
                activePlaneIndex = -1;
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
                if (answer > options.Count)
                {
                    DisplayMenu(options);
                }
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
        static void DrawTile(int x, int y, bool drawEmptyTile)
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
            if (!drawEmptyTile) Console.ForegroundColor = ConsoleColor.Green;
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
                Console.SetCursorPosition(battlefieldLeftCoursorPos, battlefieldTopCoursorPos + i);
                if(10 > i + 1)
                { 
                    Console.Write($"{i + 1} ");
                }
                else 
                {
                    Console.Write(i + 1);
                }
                for (int j = 0; j < height; j++)
                {
                    DrawTile(j, i, true);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - width * 3 / 2, height + 3);
            Console.Write("  ");
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
        static void WrtieTurn()
        {
            if (turn == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }else Console.ForegroundColor = ConsoleColor.Red;
            WriteCentered($"{turn}'s turn", 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void WriteMessage(string message)
        {
            WriteCentered(blank, 2);
            WriteCentered(message, 2);
        }
    }
}
