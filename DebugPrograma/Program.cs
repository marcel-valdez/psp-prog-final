using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Programa;
using Programa.ConsoleService;
namespace DebugPrograma
{
    class Program
    {
        static void Main(string[] args)
        {
            // testcase #1
            MainProgram.Main(new string[] { "test1.data", "386" });
            Console.ReadKey(true);
        }
    }
}
