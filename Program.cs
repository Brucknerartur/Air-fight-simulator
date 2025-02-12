namespace Air_fight_simulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DisplayBattlefield(20, 20);
        }
        
        static void DisplayBattlefield(int width, int height)
        {
            string[] abc = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - width * 3 / 2, i + 3);
                Console.Write(abc[i]);
                for (global::System.Int32 j = 0; j < height; j++)
                {
                    Console.Write("[ ]");
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
