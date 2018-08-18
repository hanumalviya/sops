using InstallTool.Code;
using System;
using System.Linq;

namespace InstallTool
{
    class Program
    {       
        static void Main(string[] args)
        {
            Menu();
        }

        private static void Menu()
        {
            Console.WriteLine("Narzędzie instalacyjne systemu SOPS\n\n");
            Console.WriteLine("Naciśnij [ENTER] by rozpocząć proces instalacyjny.\n(UWAGA proces automatycznie zmodyfikuje dane w bazie)\n");

            Console.WriteLine("Rozpocznij [ENTER]");
            Console.ReadLine();

            Generator generator = new Generator();
            generator.Generate();
        }
    }
}
