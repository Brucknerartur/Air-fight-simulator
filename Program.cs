using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.Arm;

namespace Air_fight_simulator
{
    // ← → ↑ ↓
    internal class Program
    {
        static string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        static List<Plane> blueplanes = [];
        static List<Plane> redplanes = [];
        static List<AntiAir> blueAntiAirs = [];
        static List<Weapon> weapons = [];
        static List<Weapon> antiAirWeapons = [];
        static List<string> menuOptions = ["Surrender", "Move", "Shoot with Plane", "Shoot with Anti-Air"];
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
        static int activeIndex = -1;
        static int answer = -1;
        static string blank = new string(' ', Console.WindowWidth - 20);
        static string message = "";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.White;

            CreateWeapons();
            CreatePlanes();
            CreateAntiAirs();

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
                        endTurn = true;
                        Move();
                        break;
                    case 2:
                        endTurn = true;
                        ShootPlane();
                        break;
                    case 3:
                        endTurn = true;
                        ShootAntiAir();
                        answer = -1;
                        break;
                    case -1: Console.WriteLine("HIBA"); break;
                }
                activeIndex = -1;
                if (endTurn)
                {
                    if (turn == "Blue")
                    {
                        turn = "Red";
                        endTurn = false;
                    }
                    else { turn = "Blue"; endTurn = false; }
                }
            }
        }
        static void ShootAntiAir()
        { 
            if(turn == "Blue")
            {
                List<string> antiAirData = new List<string>();
                for (int i = 0; i < blueAntiAirs.Count; i++)
                {
                    string[] data = blueAntiAirs[i].ToString().Split(",");
                    antiAirData.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(antiAirData);
                activeIndex = index;
                ShootWithAntiAir(index, blueAntiAirs[activeIndex].X, blueAntiAirs[activeIndex].Y);
            }
            else
            {
                answer = -1;
                Console.WriteLine("Sorry you have no Anti-Air");
                endTurn = false;
            }
        }
        static void ShootWithAntiAir(int index, int x, int y)
        {
            int hitCount = 0;
            if (turn == "Blue")
            {
                Weapon chosenWeapon = ChooseWeaponForAntiAir(blueAntiAirs[index]);
                blueAntiAirs[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMovesForAntiAir(blueAntiAirs[index]);
                GetATKParamsForAntiAir(blueAntiAirs[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < redplanes.Count; i++)
                        {
                            if (redplanes[i].X == newX && redplanes[i].Y == newY)
                            {
                                redplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (redplanes[i].X == -100 && redplanes[i].Y == -100)
                                {
                                    redplanes.Remove(redplanes[i]);
                                    if (redplanes.Count == 0)
                                    {
                                        VictoryScreen("Blue");
                                    }
                                }
                            }
                        }
                    }
                }
                message = $"Hit {hitCount} times";
                WriteMessage();
                newX = -1;
                newY = -1;
            }
        }
        static void ShootPlane()
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
                activeIndex = index;
                ShootWithPlane(index, blueplanes[activeIndex].X, blueplanes[activeIndex].Y);
            }
            if (turn == "Red")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < redplanes.Count; i++)
                {
                    string[] data = redplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                activeIndex = index;
                ShootWithPlane(index, redplanes[activeIndex].X, redplanes[activeIndex].Y);
            }
        }
        static void VictoryScreen(string winner)
        {
            Console.Clear();
            if (winner == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                WriteCentered($"Blue is the winner", Console.WindowHeight / 2);
            }
            if (winner == "Red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCentered($"Red is the winner", Console.WindowHeight / 2);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            answer = 0;
        }
        static bool Hit(Weapon weapon)
        {
            Random random = new Random();
            if (random.Next(100) < weapon.Accuracy)
            {
                return true;
            }
            return false;
        }
        static void ShootWithPlane(int index, int x, int y)
        {
            int hitCount = 0;
            if (turn == "Blue")
            {
                Weapon chosenWeapon = ChooseWeapon(blueplanes[index]);
                blueplanes[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMoves(blueplanes[index]);
                GetATKParams(blueplanes[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < redplanes.Count; i++)
                        {
                            if (redplanes[i].X == newX && redplanes[i].Y == newY)
                            {
                                redplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (redplanes[i].X == -100 && redplanes[i].Y == -100)
                                {
                                    redplanes.Remove(redplanes[i]);
                                    if (redplanes.Count == 0)
                                    {
                                        VictoryScreen("Blue");
                                    }
                                }
                            }
                        }
                    }
                }
                message = $"Hit {hitCount} times";
                WriteMessage();
                newX = -1;
                newY = -1;
            }
            if (turn == "Red")
            {
                Weapon chosenWeapon = ChooseWeapon(redplanes[index]);
                redplanes[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMoves(redplanes[index]);
                GetATKParams(redplanes[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < blueplanes.Count; i++)
                        {
                            if (blueplanes[i].X == newX && blueplanes[i].Y == newY)
                            {
                                blueplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (blueplanes[i].X == -100 && blueplanes[i].Y == -100)
                                {
                                    blueplanes.Remove(blueplanes[i]);
                                    if (blueplanes.Count == 0)
                                    {
                                        VictoryScreen("Red");
                                    }
                                }
                            }
                        }
                        message = $"Hit {hitCount} times";
                        WriteMessage();
                        newX = -1;
                        newY = -1;
                    }
                }
            }

        }
        static void GetATKParams(Plane plane)
        {
            RefreshScreen();
            Console.SetCursorPosition(0, height + 5);
            try
            {
                if (newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target row: ", height + 5);
                    newY = int.Parse(Console.ReadLine());
                    newY--;
                }
                if (!plane.PossibleMoveY.Contains(newY))
                {
                    newY = -1;
                    GetATKParams(plane);
                }

                if (newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target column: ", height + 5);
                    newX = int.Parse(Console.ReadLine());
                    newX--;

                }
                if (!plane.PossibleMoveX.Contains(newX))
                {
                    newX = -1;
                    GetATKParams(plane);
                }
            }
            catch
            {
                GetATKParams(plane);
            }
        }
        static void GetATKParamsForAntiAir(AntiAir antiAir)
        {
            RefreshScreen();
            Console.SetCursorPosition(0, height + 5);
            try
            {
                if (newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target row: ", height + 5);
                    newY = int.Parse(Console.ReadLine());
                    newY--;
                }
                if (!antiAir.PossibleMoveY.Contains(newY))
                {
                    newY = -1;
                    GetATKParamsForAntiAir(antiAir);
                }

                if (newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target column: ", height + 5);
                    newX = int.Parse(Console.ReadLine());
                    newX--;

                }
                if (!antiAir.PossibleMoveX.Contains(newX))
                {
                    newX = -1;
                    GetATKParamsForAntiAir(antiAir);
                }
            }
            catch
            {
                GetATKParamsForAntiAir(antiAir);
            }
        }
        static Weapon ChooseWeapon(Plane plane)
        {
            List<string> asd = [];
            foreach (var item in plane.Weapons)
            {
                asd.Add(item.ToString());
            }
            RefreshScreen();
            int answer = DisplayMenu(asd);
            return plane.Weapons[answer];
        }
        static Weapon ChooseWeaponForAntiAir(AntiAir antiair)
        {
            List<string> asd = [];
            foreach (var item in antiair.Weapons)
            {
                asd.Add(item.ToString());
            }
            RefreshScreen();
            int answer = DisplayMenu(asd);
            return antiair.Weapons[answer];
        }
        static void RefreshScreen()
        {
            Console.Clear();
            WrtieTurn();
            DisplayBattlefield(width, height);
            WriteInformation();
            WriteMessage();
            if (activeIndex != -1)
            {
                if (answer == 1 || answer == 2)
                {
                    if (turn == "Blue") ColorPossibleMoves(blueplanes[activeIndex]);
                    if (turn == "Red") ColorPossibleMoves(redplanes[activeIndex]);
                }
                if (answer == 3)
                {
                    if (turn == "Blue") ColorPossibleMovesForAntiAir(blueAntiAirs[activeIndex]);
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
                            DrawTile(j, i, false);
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
        static void ColorPossibleMovesForAntiAir(AntiAir antiAir)
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
                    if (antiAir.PossibleMoveY.Contains(i))
                    {
                        if (antiAir.PossibleMoveX.Contains(j))
                        {
                            DrawTile(j, i, false);
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
                activeIndex = index;
                blueplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(blueplanes[index]);
                GetMoveParams(blueplanes[index]);
                blueplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
                activeIndex = -1;
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
                activeIndex = index;
                redplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(redplanes[index]);
                GetMoveParams(redplanes[index]);
                redplanes[index].Move(newX, newY, newRotation);
                newX = -1;
                newY = -1;
                newRotation = string.Empty;
                activeIndex = -1;
            }
            endTurn = true;
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
            WriteCentered(blank, height + options.Count + 4);
            WriteCentered("Answer:  ", height + options.Count + 4);
            try
            {
                int answer = int.Parse(Console.ReadLine());
                if (answer + 1 > options.Count)
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
            for (int i = 0; i < blueAntiAirs.Count; i++)
            {
                if (i + 1 <= blueAntiAirs.Count) if (blueAntiAirs[i].X == x && blueAntiAirs[i].Y == y)
                    {
                        DrawBlueAntiAir(i, x, y);
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
        static void DrawBlueAntiAir(int index, int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[X]");
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
            weapons.Add(new Weapon("COW 37 mm gun ", "gun", 100, 20, 4, 1, 7)); // 0
            weapons.Add(new Weapon("Bordkanone BK 7,5 cannon", "gun", 30, 50, 1, 2, 9)); //1
            weapons.Add(new Weapon("M2 Browning machine gun", "gun", 100, 25, 6, 1, 5)); //2
            weapons.Add(new Weapon("AGM-142 Raptor", "rocket", 8, 50, 2, 3, 11)); //3
            weapons.Add(new Weapon("AS.34 Kormoran ", "rocket", 6, 60, 2, 4, 13));//4
            weapons.Add(new Weapon("Anti-air gun type 1", "anti-air", 50, 50, 5, 1, 7));//5
            weapons.Add(new Weapon("Anti-air gun type 2", "anti-air", 50, 40, 5, 1, 9));//6
        }
        static void CreatePlanes()
        {
            List<Weapon> weaponsForPlanes = new List<Weapon>();
            weaponsForPlanes.Add(weapons[0]);
            weaponsForPlanes.Add(weapons[1]);
            weaponsForPlanes.Add(weapons[2]);
            weaponsForPlanes.Add(weapons[3]);
            weaponsForPlanes.Add(weapons[4]);
            blueplanes.Add(new Plane("A-10C Thunderbolt II", 14, 7, "bottom", 2, 7, weaponsForPlanes));

            blueplanes.Add(new Plane("F-117 Nighthawk", 12, 7, "bottom", 2, 7, weaponsForPlanes));

            redplanes.Add(new Plane("C-130H Hercules", 12, 20, "top", 4, 5, weaponsForPlanes));

            redplanes.Add(new Plane("F-15C/D Eagle", 13, 20, "top", 4, 5, weaponsForPlanes));

            redplanes.Add(new Plane("F-35A Lightning II", 14, 20, "top", 3, 6, weaponsForPlanes));

        }
        static void CreateAntiAirs()
        {
            List<Weapon> weaponsForAntiAirs = new List<Weapon>();
            weaponsForAntiAirs.Add(weapons[5]);
            blueAntiAirs.Add(new AntiAir("Oerlikon 35 mm twin cannon", 11, 10, weaponsForAntiAirs));
            blueAntiAirs.Add(new AntiAir("Oerlikon 35 mm twin cannon", 15, 10, weaponsForAntiAirs));

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
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            WriteCentered($"{turn}'s turn", 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void WriteMessage()
        {
            WriteCentered(blank, 2);
            WriteCentered(message, 2);
        }
    }
}
