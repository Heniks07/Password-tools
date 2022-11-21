using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using System.Text.Encodings.Web;
using System.Text.Json;

namespace Generator
{
    internal class Manager
    {
        static SymmetricAlgorithm aes = Aes.Create();
        private static string key;
        private static bool keyset = true;
        static string filepath = "C:\\Users\\Hendr\\Desktop\\passwords.dat";

        public void Run()
        {
            Console.Title = "Password manager";
            if (!keyset)
            {
                Console.Write("Enter your Key (don't have one? input --gen):\n>");
                key = Console.ReadLine();
                if (key == "--gen")
                {
                    Cryptographic cryptographic = new Cryptographic();
                    key = cryptographic.GenerateKey();
                    keyset = true;
                    Console.WriteLine("your key is\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(key);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n!!!DON'T SHARE IT WITH ANYONE AND KEEP IT SECURED!WITOUT IT YOU DO NOT HAVE ACCES TO YOUR PASSWORDS!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                keyset = true;
                Console.WriteLine("Key set successfully!");

            }
            run();

        }

        //vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=

        void run()
        {

            input();

            List<Password> passwords = readPasswordList();
            foreach (Password password in passwords)
            {
                Console.WriteLine("username : {0}\npassword : {1}\n", password.username, password.password);
            }


        }

        void input()
        {
            Console.Write(">");
            List<string> input = getPasswordInput();
            if (input != null)
            {
                addPasswordList(input[0], input[1], input[2]);
                return;
            }
            return;
        }

        List<string> getPasswordInput()
        {
            Console.Write("Please input the name of the website/app/etc.\n>");
            string applicationName = Console.ReadLine();
            Console.Write("Please input your Username/Email address\n>");
            string username = Console.ReadLine();
            Console.Write("Please input your password (don't have one? type in --gen to generate one)\n>");
            string password = Console.ReadLine();

            if (password == "--gen")
            {
                Console.Write("Input your dessired password length\n>");
                int length;
                if (int.TryParse(Console.ReadLine(), out length))
                {
                    if (length > 1000)
                    {
                        Console.WriteLine("password to long (maximum is 1000)");
                        return null;
                    }
                    if (length < 8)
                    {
                        Console.WriteLine("password to short(minimum is 8)");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("only input numbers between 8 and 1000");
                    return null;
                }

                Generation generation = new Generation();
                password = generation.trueRandom(length);
            }

            List<string> output = new List<string>();

            output.Add(applicationName);
            output.Add(username);
            output.Add(password);

            return output;

        }

        string readPassword(Password password)
        {
            Cryptographic cryptographic = new Cryptographic();
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };


            byte[] iv = password.IV;
            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");

            string cipher = password.password;

            string plainText = cryptographic.DecryptStringFromBytes_Aes(Convert.FromBase64String(cipher), key, iv);

            List<string> output = new List<string>();

            return plainText;
        }

        void addPasswordList(string applicationName, string username, string passwordInput)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            Cryptographic cryptographic = new Cryptographic();

            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");
            byte[] iv = Convert.FromBase64String(cryptographic.GenerateIV());

            string cipher = Convert.ToBase64String(cryptographic.EncryptStringToBytes_Aes(passwordInput, key, iv));


            Password password = new Password()
            {
                IV = iv,
                password = cipher,
                username = username,
                applicationName = applicationName,
            };

            if (string.IsNullOrEmpty(readPassword(password)))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error!!!");
                Console.ForegroundColor = ConsoleColor.White;
                addPasswordList(applicationName, username, passwordInput);
                return;
            }

            List<Password> pswds = new List<Password>();

            string fileJson = File.ReadAllText(filepath);

            List<Password> filePswds = JsonSerializer.Deserialize<List<Password>>(fileJson);



            foreach (Password psw in filePswds)
            {
                if (psw.password == null)
                {
                    Console.WriteLine("noo");
                }
                pswds.Add(psw);
            }

            Console.WriteLine(readPassword(password));

            pswds.Add(password);


            string jsonString = JsonSerializer.Serialize(pswds);

            File.WriteAllText(filepath, jsonString);


        }

        List<Password> readPasswordList()
        {
            Cryptographic cryptographic = new Cryptographic();
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };




            string fileJson = File.ReadAllText(filepath);

            List<Password> fileCipher = JsonSerializer.Deserialize<List<Password>>(fileJson);

            List<Password> pswds = new List<Password>();

            foreach (Password password in fileCipher)
            {
                byte[] iv = password.IV;
                byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");
                string cipher = password.password;

                if (cipher == null)
                {

                    Console.WriteLine("gg: {0}", password.username);
                }

                string plainText = cryptographic.DecryptStringFromBytes_Aes(Convert.FromBase64String(cipher), key, iv);


                Password password1 = new Password()
                {
                    IV = iv,
                    password = plainText,
                    username = password.username
                };

                pswds.Add(password1);
            }

            return pswds;
        }


        class Password
        {
            public byte[] IV { get; set; }
            public string password { get; set; }
            public string username { get; set; }
            public string applicationName { get; set; }
        }

    }
}
