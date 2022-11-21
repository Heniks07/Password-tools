using System;
using System.IO;
using System.Text.Json;

namespace Generator
{

    class Program
    {
        static string path = @".\config.txt";
        static void Main(string[] args)
        {        
            

            Run();
            Console.WriteLine("Press anny butn to exit");
            Console.ReadKey();
        }

        static void Input()
        {
            Generation generation = new Generation();
            
            string input = Console.ReadLine();

            if (input == "?")
            {
                Console.WriteLine("___Help___\n\n" +
                    " gen\tonly generate a password and don't store it\n" +
                    " manager\topens the password manager where you can generate and securely store passwords\t\n" +
                    " clear\tclears the console\n");
                return;
            }

            if (input == "gen")
            {
                generation.Run();
            }
            if (input == "manager")
            {
                Manager manager = new Manager();
                manager.Run();
            }
            if(input == "clear")
            {
                Console.Clear();
            }
        }
        static void Run()
        {
            //first setup
            string fileString = File.ReadAllText(path);
            Configuration configuration = JsonSerializer.Deserialize<Configuration>(fileString);
            if (string.IsNullOrEmpty(configuration.Generator))
            {
                Console.Write("Please input the path where your generated passwords should be stored\n>");
                string input= Console.ReadLine();
                if (!Directory.Exists(input))
                {
                    Console.WriteLine("This is not a valid Path!");
                    Run();
                }
                configuration.Generator = input + "\\pswds.txt";
                File.Create(configuration.Generator).Close();

            }
            if (string.IsNullOrEmpty(configuration.Manager))
            {
                Console.Write("Please input the path where your generated passwords should be stored\n>");
                string input = Console.ReadLine();
                if (!Directory.Exists(input))
                {
                    Console.WriteLine("This is not a valid Path!");
                    Run();
                }

                configuration.Manager = input + "\\passwords.dat";

                File.Create(configuration.Manager).Close();
                File.WriteAllText(configuration.Manager, "[]");
            }
               
            File.WriteAllText(path, JsonSerializer.Serialize(configuration));



            Console.Title = "Main menue";
            Console.Write("What do you want to do? (? for help)\n>");
            Input();
            Run();
        }

        public void returning()
        {
            Run();
        }
    }
    public class Configuration
    {
        public string Manager { get; set; }
        public string Generator { get; set; }
    }
}
