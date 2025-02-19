using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.Arm;

namespace Air_fight_simulator
{
    // ← → ↑ ↓
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.White;
            ProgramHelpers.CreateWeapons();
            ProgramHelpers.CreatePlanes();
            ProgramHelpers.CreateAntiAirs();

            while (ProgramHelpers.answer != 0)
            {
                ProgramHelpers.answer = -1;
                while (0 > ProgramHelpers.answer || ProgramHelpers.menuOptions.Count <= ProgramHelpers.answer)
                {
                    RefreshScreen();
                    ProgramHelpers.answer = DisplayMenu(ProgramHelpers.menuOptions);
                }
                switch (ProgramHelpers.answer)
                {
                    case -2: break;
                    case 0: return;
                    case 1:
                        ProgramHelpers.endTurn = true;
                        Move();
                        break;
                    case 2:
                        ProgramHelpers.endTurn = true;
                        ShootPlane();
                        break;
                    case 3:
                        ProgramHelpers.endTurn = true;
                        ShootAntiAir();
                        ProgramHelpers.answer = -1;
                        break;
                    case 4:
                        ProgramHelpers.answer = -1;
                        ShowTutorial();
                        break;
                    case -1: Console.WriteLine("HIBA"); break;
                }

                ProgramHelpers.activeIndex = -1;
                if (ProgramHelpers.endTurn)
                {
                    if (ProgramHelpers.turn == "Blue")
                    {
                        ProgramHelpers.turn = "Red";
                        ProgramHelpers.endTurn = false;
                    }
                    else { ProgramHelpers.turn = "Blue"; ProgramHelpers.endTurn = false; }
                }
            }
        }
        static void ShootAntiAir()
        { 
            if(ProgramHelpers.turn == "Blue")
            {
                List<string> antiAirData = new List<string>();
                for (int i = 0; i < ProgramHelpers.blueAntiAirs.Count; i++)
                {
                    string[] data = ProgramHelpers.blueAntiAirs[i].ToString().Split(",");
                    antiAirData.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(antiAirData);
                ProgramHelpers.activeIndex = index;
                ShootWithAntiAir(index, ProgramHelpers.blueAntiAirs[ProgramHelpers.activeIndex].X, ProgramHelpers.blueAntiAirs[ProgramHelpers.activeIndex].Y);
            }
            else
            {
                ProgramHelpers.answer = -1;
                Console.WriteLine("Sorry you have no Anti-Air");
                ProgramHelpers.endTurn = false;
            }
        }
        static void ShootWithAntiAir(int index, int x, int y)
        {
            int hitCount = 0;
            if (ProgramHelpers.turn == "Blue")
            {
                Weapon chosenWeapon = ChooseWeaponForAntiAir(ProgramHelpers.blueAntiAirs[index]);
                ProgramHelpers.blueAntiAirs[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMovesForAntiAir(ProgramHelpers.blueAntiAirs[index]);
                GetATKParamsForAntiAir(ProgramHelpers.blueAntiAirs[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < ProgramHelpers.redplanes.Count; i++)
                        {
                            if (ProgramHelpers.redplanes[i].X == ProgramHelpers.newX && ProgramHelpers.redplanes[i].Y == ProgramHelpers.newY)
                            {
                                ProgramHelpers.redplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (ProgramHelpers.redplanes[i].X == -100 && ProgramHelpers.redplanes[i].Y == -100)
                                {
                                    ProgramHelpers.redplanes.Remove(ProgramHelpers.redplanes[i]);
                                    if (ProgramHelpers.redplanes.Count == 0)
                                    {
                                        VictoryScreen("Blue");
                                    }
                                }
                            }
                        }
                    }
                }

                ProgramHelpers.message = $"Hit {hitCount} times";
                WriteMessage();
                ProgramHelpers.newX = -1;
                ProgramHelpers.newY = -1;
            }
        }
        static void ShootPlane()
        {
            if (ProgramHelpers.turn == "Blue")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < ProgramHelpers.blueplanes.Count; i++)
                {
                    string[] data = ProgramHelpers.blueplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                ProgramHelpers.activeIndex = index;
                ShootWithPlane(index, ProgramHelpers.blueplanes[ProgramHelpers.activeIndex].X, ProgramHelpers.blueplanes[ProgramHelpers.activeIndex].Y);
            }
            if (ProgramHelpers.turn == "Red")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < ProgramHelpers.redplanes.Count; i++)
                {
                    string[] data = ProgramHelpers.redplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                ProgramHelpers.activeIndex = index;
                ShootWithPlane(index, ProgramHelpers.redplanes[ProgramHelpers.activeIndex].X, ProgramHelpers.redplanes[ProgramHelpers.activeIndex].Y);
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
            ProgramHelpers.answer = 0;
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
            if (ProgramHelpers.turn == "Blue")
            {
                Weapon chosenWeapon = ChooseWeapon(ProgramHelpers.blueplanes[index]);
                ProgramHelpers.blueplanes[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMoves(ProgramHelpers.blueplanes[index]);
                GetATKParams(ProgramHelpers.blueplanes[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < ProgramHelpers.redplanes.Count; i++)
                        {
                            if (ProgramHelpers.redplanes[i].X == ProgramHelpers.newX && ProgramHelpers.redplanes[i].Y == ProgramHelpers.newY)
                            {
                                ProgramHelpers.redplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (ProgramHelpers.redplanes[i].X == -100 && ProgramHelpers.redplanes[i].Y == -100)
                                {
                                    ProgramHelpers.redplanes.Remove(ProgramHelpers.redplanes[i]);
                                    if (ProgramHelpers.redplanes.Count == 0)
                                    {
                                        VictoryScreen("Blue");
                                    }
                                }
                            }
                        }
                    }
                }

                ProgramHelpers.message = $"Hit {hitCount} times";
                WriteMessage();
                ProgramHelpers.newX = -1;
                ProgramHelpers.newY = -1;
            }
            if (ProgramHelpers.turn == "Red")
            {
                Weapon chosenWeapon = ChooseWeapon(ProgramHelpers.redplanes[index]);
                ProgramHelpers.redplanes[index].CalculatePossibleATK(chosenWeapon.Range);
                ColorPossibleMoves(ProgramHelpers.redplanes[index]);
                GetATKParams(ProgramHelpers.redplanes[index]);
                for (global::System.Int32 j = 0; j < chosenWeapon.ATK; j++)
                {
                    chosenWeapon.UseAmmo(1);
                    if (Hit(chosenWeapon))
                    {
                        hitCount++;
                        for (global::System.Int32 i = 0; i < ProgramHelpers.blueplanes.Count; i++)
                        {
                            if (ProgramHelpers.blueplanes[i].X == ProgramHelpers.newX && ProgramHelpers.blueplanes[i].Y == ProgramHelpers.newY)
                            {
                                ProgramHelpers.blueplanes[i].GetDamaged(chosenWeapon.DMG);
                                if (ProgramHelpers.blueplanes[i].X == -100 && ProgramHelpers.blueplanes[i].Y == -100)
                                {
                                    ProgramHelpers.blueplanes.Remove(ProgramHelpers.blueplanes[i]);
                                    if (ProgramHelpers.blueplanes.Count == 0)
                                    {
                                        VictoryScreen("Red");
                                    }
                                }
                            }
                        }

                        ProgramHelpers.message = $"Hit {hitCount} times";
                        WriteMessage();
                        ProgramHelpers.newX = -1;
                        ProgramHelpers.newY = -1;
                    }
                }
            }

        }
        static void GetATKParams(Plane plane)
        {
            RefreshScreen();
            Console.SetCursorPosition(0, ProgramHelpers.height + 5);
            try
            {
                if (ProgramHelpers.newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target row: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newY = int.Parse(Console.ReadLine());
                    ProgramHelpers.newY--;
                }
                if (!plane.PossibleMoveY.Contains(ProgramHelpers.newY))
                {
                    ProgramHelpers.newY = -1;
                    GetATKParams(plane);
                }

                if (ProgramHelpers.newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target column: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newX = int.Parse(Console.ReadLine());
                    ProgramHelpers.newX--;

                }
                if (!plane.PossibleMoveX.Contains(ProgramHelpers.newX))
                {
                    ProgramHelpers.newX = -1;
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
            Console.SetCursorPosition(0, ProgramHelpers.height + 5);
            try
            {
                if (ProgramHelpers.newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target row: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newY = int.Parse(Console.ReadLine());
                    ProgramHelpers.newY--;
                }
                if (!antiAir.PossibleMoveY.Contains(ProgramHelpers.newY))
                {
                    ProgramHelpers.newY = -1;
                    GetATKParamsForAntiAir(antiAir);
                }

                if (ProgramHelpers.newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Target column: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newX = int.Parse(Console.ReadLine());
                    ProgramHelpers.newX--;

                }
                if (!antiAir.PossibleMoveX.Contains(ProgramHelpers.newX))
                {
                    ProgramHelpers.newX = -1;
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
            DisplayBattlefield(ProgramHelpers.width, ProgramHelpers.height);
            WriteInformation();
            WriteMessage();
            if (ProgramHelpers.activeIndex != -1)
            {
                if (ProgramHelpers.answer == 1 || ProgramHelpers.answer == 2)
                {
                    if (ProgramHelpers.turn == "Blue") ColorPossibleMoves(ProgramHelpers.blueplanes[ProgramHelpers.activeIndex]);
                    if (ProgramHelpers.turn == "Red") ColorPossibleMoves(ProgramHelpers.redplanes[ProgramHelpers.activeIndex]);
                }
                if (ProgramHelpers.answer == 3)
                {
                    if (ProgramHelpers.turn == "Blue") ColorPossibleMovesForAntiAir(ProgramHelpers.blueAntiAirs[ProgramHelpers.activeIndex]);
                }
            }
        }
        static void ColorPossibleMoves(Plane plane)
        {
            for (int i = 0; i < ProgramHelpers.width; i++)
            {
                Console.SetCursorPosition(ProgramHelpers.battlefieldLeftCoursorPos, ProgramHelpers.battlefieldTopCoursorPos + i);
                if (10 > i + 1)
                {
                    Console.Write($"{i + 1} ");
                }
                else
                {
                    Console.Write(i + 1);
                }
                for (int j = 0; j < ProgramHelpers.height; j++)
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
            for (int i = 0; i < ProgramHelpers.width; i++)
            {
                Console.SetCursorPosition(ProgramHelpers.battlefieldLeftCoursorPos, ProgramHelpers.battlefieldTopCoursorPos + i);
                if (10 > i + 1)
                {
                    Console.Write($"{i + 1} ");
                }
                else
                {
                    Console.Write(i + 1);
                }
                for (int j = 0; j < ProgramHelpers.height; j++)
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
            Console.SetCursorPosition(0, ProgramHelpers.height + 5);
            try
            {
                if (ProgramHelpers.newY == -1)
                {
                    RefreshScreen();
                    WriteCentered("Row: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newY = int.Parse(Console.ReadLine());
                    ProgramHelpers.newY--;
                }
                if (!plane.PossibleMoveY.Contains(ProgramHelpers.newY))
                {
                    ProgramHelpers.newY = -1;
                    GetMoveParams(plane);
                }

                if (ProgramHelpers.newX == -1)
                {
                    RefreshScreen();
                    WriteCentered("Column: ", ProgramHelpers.height + 5);
                    ProgramHelpers.newX = int.Parse(Console.ReadLine());
                    ProgramHelpers.newX--;

                }
                if (!plane.PossibleMoveX.Contains(ProgramHelpers.newX))
                {
                    ProgramHelpers.newX = -1;
                    GetMoveParams(plane);
                }

                if (ProgramHelpers.newRotation == string.Empty)
                {
                    RefreshScreen();
                    WriteCentered("Rotation (up, down, left, right): ", ProgramHelpers.height + 5);
                    ProgramHelpers.newRotation = Console.ReadLine();
                }
                string[] asd = ["up", "down", "left", "right"];
                if (!asd.Contains(ProgramHelpers.newRotation))
                {
                    ProgramHelpers.newRotation = string.Empty;
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
            if (ProgramHelpers.turn == "Blue")
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < ProgramHelpers.blueplanes.Count; i++)
                {
                    string[] data = ProgramHelpers.blueplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                ProgramHelpers.activeIndex = index;
                ProgramHelpers.blueplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(ProgramHelpers.blueplanes[index]);
                GetMoveParams(ProgramHelpers.blueplanes[index]);
                ProgramHelpers.blueplanes[index].Move(ProgramHelpers.newX, ProgramHelpers.newY, ProgramHelpers.newRotation);
                ProgramHelpers.newX = -1;
                ProgramHelpers.newY = -1;
                ProgramHelpers.newRotation = string.Empty;
                ProgramHelpers.activeIndex = -1;
            }
            else
            {
                List<string> planeDatas = new List<string>();
                for (int i = 0; i < ProgramHelpers.redplanes.Count; i++)
                {
                    string[] data = ProgramHelpers.redplanes[i].ToString().Split(",");
                    planeDatas.Add(data[0] + " " + data[1] + " " + data[2]);
                }
                RefreshScreen();
                int index = DisplayMenu(planeDatas);
                ProgramHelpers.activeIndex = index;
                ProgramHelpers.redplanes[index].CalcutePossibleMoves();
                ColorPossibleMoves(ProgramHelpers.redplanes[index]);
                GetMoveParams(ProgramHelpers.redplanes[index]);
                ProgramHelpers.redplanes[index].Move(ProgramHelpers.newX, ProgramHelpers.newY, ProgramHelpers.newRotation);
                ProgramHelpers.newX = -1;
                ProgramHelpers.newY = -1;
                ProgramHelpers.newRotation = string.Empty;
                ProgramHelpers.activeIndex = -1;
            }

            ProgramHelpers.endTurn = true;
        }
        static void MovePlane(Plane plane, int newX, int newY, string rotaion)
        {
            plane.Move(newX, newY, rotaion);
            RefreshScreen();
        }
        static int DisplayMenu(List<string> options)
        {
            Console.WriteLine();
            Console.SetCursorPosition(0, ProgramHelpers.height + 2);
            for (int i = 0; i < options.Count; i++)
            {
                WriteCentered($"{i} - {options[i]}", ProgramHelpers.height + i + 4);
            }
            Console.WriteLine();
            WriteCentered(ProgramHelpers.blank, ProgramHelpers.height + options.Count + 4);
            WriteCentered("Answer:  ", ProgramHelpers.height + options.Count + 4);
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
            for (int i = 0; i < ProgramHelpers.blueplanes.Count; i++)
            {
                if (i + 1 <= ProgramHelpers.blueplanes.Count) if (ProgramHelpers.blueplanes[i].X == x && ProgramHelpers.blueplanes[i].Y == y)
                    {
                        DrawBluePlane(i, x, y);
                        return;
                    }

            }
            for (int i = 0; i < ProgramHelpers.redplanes.Count; i++)
            {
                if (i + 1 <= ProgramHelpers.redplanes.Count) if (ProgramHelpers.redplanes[i].X == x && ProgramHelpers.redplanes[i].Y == y)
                    {
                        DrawRedPlane(i, x, y);
                        return;
                    }
            }
            for (int i = 0; i < ProgramHelpers.blueAntiAirs.Count; i++)
            {
                if (i + 1 <= ProgramHelpers.blueAntiAirs.Count) if (ProgramHelpers.blueAntiAirs[i].X == x && ProgramHelpers.blueAntiAirs[i].Y == y)
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
            if (ProgramHelpers.blueplanes[index].Rotation == "up")
            {
                Console.Write("[↑]");
            }
            else if (ProgramHelpers.blueplanes[index].Rotation == "right")
            {
                Console.Write("[→]");
            }
            else if (ProgramHelpers.blueplanes[index].Rotation == "down")
            {
                Console.Write("[↓]");
            }
            else if (ProgramHelpers.blueplanes[index].Rotation == "left")
            {
                Console.Write("[←]");
            }
            else
            {
                ProgramHelpers.blueplanes[index].ResetRotaion();
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
            if (ProgramHelpers.redplanes[index].Rotation == "up")
            {
                Console.Write("[↑]");
            }
            else if (ProgramHelpers.redplanes[index].Rotation == "right")
            {
                Console.Write("[→]");
            }
            else if (ProgramHelpers.redplanes[index].Rotation == "down")
            {
                Console.Write("[↓]");

            }
            else if (ProgramHelpers.redplanes[index].Rotation == "left")
            {
                Console.Write("[←]");
            }
            else
            {
                ProgramHelpers.redplanes[index].ResetRotaion();
                Console.Write("[↑]");

            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void DisplayBattlefield(int width, int height)
        {
            if (width > ProgramHelpers.abc.Length)
            {
                width = ProgramHelpers.abc.Length;
            }
            if (height > ProgramHelpers.abc.Length)
            {
                height = ProgramHelpers.abc.Length;
            }
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(ProgramHelpers.battlefieldLeftCoursorPos, ProgramHelpers.battlefieldTopCoursorPos + i);
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
        static void WriteInformation()
        {

            for (int i = 0; i < ProgramHelpers.blueplanes.Count; i++)
            {
                Console.SetCursorPosition(18, 3);
                Console.WriteLine("Blue Planes");
                string[] data = ProgramHelpers.blueplanes[i].ToString().Split(",");
                for (global::System.Int32 j = 0; j < data.Length; j++)
                {
                    Console.SetCursorPosition(10, 5 + j + i * data.Length);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < ProgramHelpers.redplanes.Count; i++)
            {
                Console.SetCursorPosition(178, 3);
                Console.WriteLine("Red Planes");
                string[] data = ProgramHelpers.redplanes[i].ToString().Split(",");
                for (global::System.Int32 j = 0; j < data.Length; j++)
                {
                    Console.SetCursorPosition(170, 5 + j + i * data.Length);
                    Console.Write(data[j]);
                    Console.WriteLine();
                }
            }
        }
        static void WrtieTurn()
        {
            if (ProgramHelpers.turn == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            WriteCentered($"{ProgramHelpers.turn}'s turn", 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void WriteMessage()
        {
            WriteCentered(ProgramHelpers.blank, 2);
            WriteCentered(ProgramHelpers.message, 2);
        }
        static void ShowTutorial()
        {
            Console.Clear();
            WriteCentered("Tutorial", 2);
            WriteCentered("Red is the attacker, Blue is the defender it has less powerful planes, but it has Anti-Air.", 4);
            WriteCentered("This is your plane: [↓]. Pay attention for your rotation it will influence your shooting direction.", 6);
            WriteCentered("This is an Anti-Air: [X]. Only the blue player has this. These are indestructible.", 8);
            WriteCentered("The green tiles show where you can move or shoot", 10);
            WriteCentered("The player who has no planes loses, Anti-Airs do not count", 12);
            WriteCentered("Press a key to exit", 14);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
