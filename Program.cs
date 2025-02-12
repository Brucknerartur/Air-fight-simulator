namespace Air_fight_simulator
{
    // ← → ↑ ↓
    internal class Program
    {
        static List<Plane> planes = new List<Plane>();

        static void Main(string[] args)
        {
            Plane tesztRepülő = new Plane(10, 10, "top");
            planes.Add(tesztRepülő);
            DisplayBattlefield(30, 30);
        }

        static void DrawTile(int x, int y)
        {
            for (int i = 0; i < planes.Count; i++)
            {
                if (planes[i].XCordinate == x && planes[i].YCordinate == y)
                {
                    PlaneRotaion(i);
                }
                else
                {
                    Console.Write("[ ]");
                }
            }
        }

        static void PlaneRotaion(int index)
        {

            if(planes[index].Rotation == "top")
            {
                Console.Write("[↑]");
            }
            else if (planes[index].Rotation == "right")
            {
                Console.Write("[→]");
            }
            else if (planes[index].Rotation == "bottom")
            {
                Console.Write("[↓]");
            }
            else if (planes[index].Rotation == "right")
            {
                Console.Write("[←]");
            }
            else
            {
                planes[index].ResetRotaion();
                Console.Write("[↑]");
            }
        }

        static void DisplayBattlefield(int width, int height)
        {
            string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
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
                for (global::System.Int32 j = 0; j < height; j++)
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
    }
}
