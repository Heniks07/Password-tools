using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Generator
{

    class Program
    {
        static void Main(string[] args)
        {          
            Run();
            /*
             * Test Cryptograpics
            Cryptographic cryptographic = new Cryptographic();

            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");
            byte[] iv = Convert.FromBase64String("4KABv4INAf/CZpL3o9CANQ==");

            string cipher = Encoding.Unicode.GetString(cryptographic.EncryptStringToBytes_Aes("Hello World", key,iv));
            
            File.WriteAllText(filepath, cipher);

            string fileCipher = File.ReadAllText(filepath);

            string plainText = cryptographic.DecryptStringFromBytes_Aes(Encoding.Unicode.GetBytes(fileCipher),key,iv);

            Console.WriteLine("cipher {0}\nplain text {1}" , cipher,plainText);

            */

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
                    " clear\tclears the console\n" +
                    " exit\tcloses the proram\n");
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

            
            return;
        }
        static void Run()
        {
            Console.Title = "Main menue";
            Console.Write("What do you want to do? (? for help)\n>");
            Input();
            return;
        }

        public void returning()
        {
            Run();
        }
        
    }

}
