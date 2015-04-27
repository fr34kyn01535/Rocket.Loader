using Rocket.Rcon;
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
            RocketRconServer.Listen(8080);
        }
    }
}
