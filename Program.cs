using System;
using System.Collections.Generic;

namespace Flexing
{
    class Program
    {
        private static void Main()
        {
            var input = Console.ReadLine();
            var tokens = input.Split(" ");
            var command = new Command();
            command.Build(tokens);
            Console.WriteLine(command);
        }
    }
}
