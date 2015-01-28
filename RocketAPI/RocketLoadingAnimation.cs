using System;
using System.Reflection;
using System.Threading;

namespace Rocket
{
    internal class RocketLoadingAnimation
    {
        private static Random rand = new Random();
        private static bool running = false;
        private static Thread t;

        private static char AsciiCharacter
        {
            get
            {
                int t = rand.Next(10);
                if (t <= 2)
                    // returns a number
                    return (char)('0' + rand.Next(10));
                else if (t <= 4)
                    // small letter
                    return (char)('a' + rand.Next(27));
                else if (t <= 6)
                    // capital letter
                    return (char)('A' + rand.Next(27));
                else
                    // any ascii character
                    return (char)(rand.Next(32, 255));
            }
        }

        internal static void Load()
        {
            Console.Clear();
            running = true;
            t = new Thread(Start);
            t.Start();
        }

        private static void Start()
        {
            Console.CursorVisible = false;

            int width, height;
            int[] y;
            Initialize(out width, out height, out y);
            while (running)
            {
                System.Threading.Thread.Sleep(10);
                UpdateAllColumns(width, height, y);
            }
            Console.Clear();
        }

        internal static void Stop()
        {
            running = false;
            t.Join();
        }

        private static void UpdateAllColumns(int width, int height, int[] y)
        {
            int x;
            for (x = 0; x < width; ++x)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(x, y[x]);
                Console.Write(AsciiCharacter);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                int temp = y[x] - 2;
                Console.SetCursorPosition(x, inScreenYPosition(temp, height));
                Console.Write(AsciiCharacter);
                int temp1 = y[x] - 20;
                Console.SetCursorPosition(x, inScreenYPosition(temp1, height));
                Console.Write(' ');
                y[x] = inScreenYPosition(y[x] + 1, height);

                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.SetCursorPosition(0, 0); Console.WriteLine(@"Loading Rocket version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "...");

                Console.SetCursorPosition(16, 10); Console.WriteLine(@"  ______  _____  _______ _     _ _______ _______ "); //cyberlarge
                Console.SetCursorPosition(16, 11); Console.WriteLine(@" |_____/ |     | |       |____/  |______    |    ");
                Console.SetCursorPosition(16, 12); Console.WriteLine(@" |    \_ |_____| |_____  |    \_ |______    |    ");
                Console.SetCursorPosition(16, 13); Console.WriteLine(@"                                                 ");
            }
        }

        public static int inScreenYPosition(int yPosition, int height)
        {
            if (yPosition < 0)
                return yPosition + height;
            else if (yPosition < height)
                return yPosition;
            else
                return 0;
        }

        private static void Initialize(out int width, out int height, out int[] y)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth - 1;
            y = new int[width];
            for (int x = 0; x < width; ++x)
            {
                y[x] = rand.Next(height);
            }
        }
    }
}