using Rocket.RCON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCONTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MinimalRocketRconServer.Listen(1234);
            while (Console.ReadLine() != "exit") { }
        }
    }
}
