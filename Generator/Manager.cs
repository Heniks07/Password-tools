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
        private static string keyString;
        private static bool keyset = false;
        static string filepath = "C:\\Users\\Hendr\\Desktop\\passwords.dat";

        public void Run()
        {
            Configuration configuration = new Configuration();
            configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(@".\config.txt"));

            filepath = configuration.Manager;

            Console.Title = "Password manager";
            if (!keyset)
            {
                Console.Write("Enter your Key (don't have one? inputStr --gen):\n>");
                keyString = Console.ReadLine();
                if (keyString == "--gen")
                {
                    Cryptographic cryptographic = new Cryptographic();
                    keyString = Convert.ToBase64String(cryptographic.getKey());
                    keyset = true;
                    Console.WriteLine("your key is\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(keyString);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n!!!DON'T SHARE IT WITH ANYONE AND KEEP IT SECURED!WITOUT IT YOU DO NOT HAVE ACCES TO YOUR PASSWORDS!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                keyset = true;
                Console.WriteLine("Key set successfully!");

            }
            input();

        }


        void input()
        {
            Console.Write("What do you want to do? (? for help)\n>");

            switch (Console.ReadLine())
            {
                case "?":
                    {
                        Console.WriteLine("\n___HELP___\n" +
                            " add\tadd a password\n" +
                            " list\tshows a list of passwords\n" +
                            " get\tget all information about one password\n" +
                            " rm\tremove a password (not undoable!)\n" +
                            " exit\tgo back to the main menue\n" +
                            " clear\tclear the console\n" +
                            " key\tshows you the key\n" +
                            " setDir\tchange the direction where your database is\n");
                        break;
                    }
                case "add":
                    {
                        List<string> inputs = getPasswordInput();
                        if (inputs != null)
                        {
                            addPasswordList(inputs[0], inputs[1], inputs[2]);
                            Console.ForegroundColor= ConsoleColor.Green;
                            Console.WriteLine("Succesfully added a password for {0}!", inputs[0]);
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
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

                        string inputStr = Console.ReadLine();

                        bool found = false;

                        foreach (Password password in inputs)
                        {
                            if (password.applicationName == inputStr)
                            {
                                Console.WriteLine("\n*********************\n" +
                                    "name:\t\t{0}\n" +
                                    "username:\t{1}\n" +
                                    "password:\t{2}\n" +
                                    "*********************\n", password.applicationName, password.username, password.password);
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No Password for application \"{0}\" exists (list for a lsit of all passords)", inputStr);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "rm":
                    {
                        string file = File.ReadAllText(filepath);
                        List<Password> inputs = JsonSerializer.Deserialize<List<Password>>(file);
                        Console.Write("inputStr name (website/app/etc.) of the password you want to delete\n>");
                        string inputStr = Console.ReadLine();
                        bool found = false;

                        foreach (Password password in inputs)
                        {
                            if (password.applicationName == inputStr)
                            {
                                found = true;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("Do you really wan't to delete the password of the application {0}? You can not undo this! If you wan't to delete it nevertheless type in: {0}\n>", password.applicationName);
                                Console.ForegroundColor = ConsoleColor.White;

                                if (Console.ReadLine() == password.applicationName)
                                {
                                    deletePassword(password, inputs);
                                    Console.WriteLine("password of application {0} succesully deleted", password.applicationName);
                                }
                                break;
                            }
                        }
                        if (!found)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No Password for application \"{0}\" exists (list for a lsit of all passords)", inputStr);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }  
                case "clear":
                    {
                        Console.Clear();
                        break;
                    }
                case "key":
                    {
                        Console.WriteLine("Your key is {0}", keyString);
                        break;
                    }
                case "setDir":
                    {
                        Configuration configuration= new Configuration();

                        string original = filepath;
                        Console.Write("Please input the path where your generated passwords should be stored\n>");

                        configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(@".\config.txt"));
                        string inputStr = Console.ReadLine();
                        configuration.Manager = inputStr + "\\passwords.dat";


                        filepath= configuration.Manager;

                        if (!Directory.Exists(inputStr))
                        {
                            Console.WriteLine("This is not a valid Path!");
                            break;
                        }
                        else
                        {
                            File.WriteAllText(@".\config.txt", JsonSerializer.Serialize(configuration));
                            File.Move(original, filepath);

                            Console.ForegroundColor= ConsoleColor.Green;
                            Console.WriteLine("Database Succesfully moved to {0} !", filepath);
                            Console.ForegroundColor= ConsoleColor.White;
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
                    Console.WriteLine("only inputStr numbers between 8 and 1000");
                    return null;
                }

                Generation generation = new Generation();
                password = generation.trueRandom(length);
            }
            else
            {
                Console.Write("Are you sure your password is \"{0}\"? If you are sure type yes else just press Enter.\n>", password);
                if (Console.ReadLine() != "yes") return null;

            }

            

            List<string> output = new List<string>();

            output.Add(applicationName);
            output.Add(username);
            output.Add(password);

            return output;

        }

        void deletePassword(Password ps, List<Password> passwords)
        {
            List<Password> output = new List<Password>();
            foreach (Password pw in passwords)
            {
                if (ps.applicationName != pw.applicationName)
                {
                    output.Add(pw);
                }
            }

            string json = JsonSerializer.Serialize(output);
            File.WriteAllText(filepath, json);
        }

        string readPassword(Password password)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };


            byte[] iv = password.IV;
            byte[] key = Convert.FromBase64String(keyString);

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


            byte[] key = Convert.FromBase64String(keyString);

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
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                pswds.Add(psw);
            }


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
                byte[] key = Convert.FromBase64String(keyString);
                Cryptographic cryptographic = new Cryptographic(iv, key);
                string cipher = password.password;


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
