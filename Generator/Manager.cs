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
                    Aes myAes = Aes.Create();
                    key = Convert.ToBase64String(myAes.Key);
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
            input();

        }

        //vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=


        void input()
        {
            Console.Write("What do you want to do? (? for help)\n>");

            switch (Console.ReadLine())
            {
                case "add":
                    {
                        List<string> inputs = getPasswordInput();
                        if (inputs != null)
                        {
                            addPasswordList(inputs[0], inputs[1], inputs[2]);
                            input();
                        }
                        input();
                        break;
                    }
                case "list":
                    {
                        List<Password> passwords = readPasswordList();

                        if (passwords == null)
                            Console.WriteLine("null");

                        for (int i = 0; i < passwords.Count; i++)
                        {
                            Password password = passwords[i];
                            Console.WriteLine("[{0}] {1}", i, password.applicationName);
                        }
                        input();
                        break;
                    }
                case "exit":
                    {
                        return;
                    }
                case "get":
                    {
                        List<Password> inputs = readPasswordList();

                        Console.Write("input name (website/app/etc.) of the password you want to get\n>");

                        string input = Console.ReadLine();

                        foreach (Password password in inputs)
                        {
                            if (password.applicationName == input)
                            {
                                Console.WriteLine("\n*********************\n" +
                                    "application name: {0}\n" +
                                    "username: {1}\n" +
                                    "password: {2}\n" +
                                    "*********************\n", password.applicationName, password.username, password.password);
                            }
                        }
                        break;
                    }
            }



            input();
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
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };


            byte[] iv = password.IV;
            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");

            Cryptographic cryptographic = new Cryptographic(iv, key);

            string plainText = cryptographic.Decrypt(password.password);

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


            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");

            Cryptographic cryptographic = new Cryptographic(key);



            string cipher = cryptographic.Encrypt(passwordInput);


            Password password = new Password()
            {
                IV = cryptographic.getIV(),
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
                Cryptographic cryptographic = new Cryptographic(iv, key);
                string cipher = password.password;

                if (cipher == null)
                {

                    Console.WriteLine("gg: {0}", password.username);
                }

                string plainText = cryptographic.Decrypt(cipher);


                Password password1 = new Password()
                {
                    IV = iv,
                    password = plainText,
                    username = password.username,
                    applicationName = password.applicationName,
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
