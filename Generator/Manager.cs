using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Generator
{
    internal class Manager
    {
        static SymmetricAlgorithm aes = Aes.Create();
        private static string key;
        private static bool keyset = false;
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
            Cryptographic cryptographic = new Cryptographic();
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            addPassword();

            


            string fileJson = File.ReadAllText(filepath);

            Password fileCipher = JsonSerializer.Deserialize<Password>(fileJson);

            byte[] iv = fileCipher.IV;
            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");

            string cipher = fileCipher.password;

            string plainText = cryptographic.DecryptStringFromBytes_Aes(Encoding.Unicode.GetBytes(cipher), key, iv);

            Console.WriteLine("cipher: {0}\nplain text: {1}", cipher, plainText);



        }

        void addPassword()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            Cryptographic cryptographic = new Cryptographic();

            byte[] key = Convert.FromBase64String("vFuHlYO6OV70TtF+io1K9oBbpqHvJX3hXx7vNPZIqss=");
            byte[] iv = Convert.FromBase64String(cryptographic.GenerateIV());

            string cipher = Encoding.Unicode.GetString(cryptographic.EncryptStringToBytes_Aes("Hello World!", key, iv));


            Password password = new Password()
            {
                IV = iv,
                password = cipher,
                username = "henikx"
            };

            string jsonString = JsonSerializer.Serialize(password);

            File.WriteAllText(filepath, jsonString);
        }



        /*

        private void input()
        {
            Console.Write("What do you want to do? (? for help)\n>");
            string Input = Console.ReadLine();

            if (Input == "exit")
            {
                Console.Clear();
                Program program = new Program();
                program.returning();
            }
            if (Input == "?")
            {
                Console.WriteLine("___Help___\n\n" +
                    " exit\ttakes you back to the main menue\n" +
                    " clear\tclears the console\n" +
                    " add\tadd a new pasword\n" +
                    " get\tget a password\n" +
                    " list\ta list of all available passwords\n" +
                    " "
                    );
            }
            if (Input == "clear")
            {
                Console.Clear();
            }

            if (Input == "add")
            {
                Console.Write("enter application Name\n>");
                string applicationName = Console.ReadLine();
                Console.Write("enter username\n>");
                string userName = Console.ReadLine();
                Console.Write("enter your Password (Don't have one? generate you one with --gen)\n>");
                string password = "";

                string input = Console.ReadLine();
                if (input == "--gen")
                {
                    Console.Write("Input your desired password length (max: 1000)\n>");
                    if (int.TryParse(input, out int length))
                    {
                        if (length < 8)
                        {
                            Console.WriteLine("unsuported password length (min: 8)");
                            return;
                        }
                        if (length > 1000)
                        {
                            Console.WriteLine("unsuported password length (max: 1000)");
                            return;
                        }

                        password = Generation.trueRandom(length);
                    }
                    else
                    {
                        Console.WriteLine("Please only input Numbers!");
                    }
                }
                Console.Write("are you sure your password for " + applicationName + " is >" + input + "<? You can't change it later! If you are sure type in: yes\n>");

                if (Console.ReadLine() != "yes")
                {
                    Console.WriteLine("Canceled please try again");
                    return;
                }
                
                password = input;

                if (string.IsNullOrEmpty(key)) Run();

                byte[] passBytes = encrypt(key, password);
                if (passBytes == null) return;
                password = System.Text.Encoding.ASCII.GetString(passBytes);
                password = System.Text.Encoding.ASCII.GetString(passBytes);
                addPassword(applicationName, userName, password);
            }

            if (Input == "get")
            {
                Console.Write("Enter Application Name:\n>");
                getPassword(Console.ReadLine());
            }
            if (Input == "list")
            {
                getPassword();
            }
            if(Input == "rm")
            {
                Console.Write("Input application name of the password you desire to delete (--all for all)\n>");
                string input = Console.ReadLine();

                if(input == "--all") delPassword();

                delPassword(input);
            }




            string[] SplitInput = Input.Split(" ");

        }

        private static byte[] encrypt(string keyString, string data)
        {



            //string message = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam"; ;


            if (!string.IsNullOrEmpty(keyString))
            {
                Cryptographic cryptographic = new Cryptographic();
                byte[] keyByte = Convert.FromBase64String(keyString);
                byte[] IV = Convert.FromBase64String("4KABv4INAf/CZpL3o9CANQ==");
                byte[] cipher = cryptographic.EncryptStringToBytes_Aes(data,keyByte,IV);
                Console.WriteLine(Encoding.Unicode.GetString(cipher));
                return cipher;
            }
            else
            {
                Console.WriteLine("enter valid key");
                return null;
            }
        }
        private static string decrypt(string keyString, byte[] cipher)
        {
            Cryptographic cryptographic = new Cryptographic();

            byte[] keyByte = Convert.FromBase64String(keyString);
            byte[] IV = Convert.FromBase64String("4KABv4INAf/CZpL3o9CANQ==");

            string data = cryptographic.DecryptStringFromBytes_Aes(cipher, keyByte, IV);
            return data;
                        
        }




        static void addPassword(string application, string username, string password)
        {
            string jsonString = "";
            if (File.Exists(filepath))
                jsonString = File.ReadAllText(filepath);
            else
            {
                File.Create(filepath).Close();
                File.WriteAllText(filepath, "[]");
                Console.WriteLine("database Created. Now you can add passwords!");
                return;
            }




            List<Passwords> psws = new List<Passwords>();
            psws = JsonConvert.DeserializeObject<List<Passwords>>(jsonString);

            foreach(Passwords pass in psws)
            {
                if(pass.ApplicationName == application)
                {
                    Console.WriteLine("\nThere already existst a entry to this application please enter a different application name\n");
                    return;
                }
            }

            try
            {
                Passwords psw = new Passwords()
                {
                    ApplicationName = application,
                    UserName = username,
                    Password = password
                };

                psws.Add(psw);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }






            var json = JsonConvert.SerializeObject(psws);
            File.Delete(filepath);
            File.WriteAllText(filepath, json);
            Console.WriteLine("succesfully added to your password list!");

        }

        static void getPassword(string application)
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("No passwords stored");
                return;
            }
            var jsonString = File.ReadAllText(filepath);

            if (string.IsNullOrEmpty(jsonString))
                return;
            passwords = JsonConvert.DeserializeObject<List<Passwords>>(jsonString);

            foreach (Passwords pass in passwords)
            {
                if (pass.ApplicationName == application)
                {
                    Console.WriteLine(pass.Password);
                    string passString = decrypt(key, Encoding.Unicode.GetBytes(pass.Password));

                    if (passString == null) return;
                    Console.WriteLine(
                        "\n****************************\n" +
                        "Application:\t" + pass.ApplicationName + "\n" +
                        "Username:\t" + pass.UserName + "\n" +
                        "Password:\t" + passString + "\n" +
                        "****************************\n"
                        );
                    return;
                }
            }
            Console.WriteLine("no entry to Application " + application + " found! If you want an overview of your stored applications type getall");
        }

        static void getPassword()
        {
            string jsonString;
            if (File.Exists(filepath))
                jsonString = File.ReadAllText(filepath);
            else
            {
                Console.WriteLine("No password stored");
                return;
            }

            if (string.IsNullOrEmpty(jsonString))
                return;
            passwords = JsonConvert.DeserializeObject<List<Passwords>>(jsonString);

            Console.Write("\n");
            for (int i = 0; i < passwords.Count; i++)
            {
                Passwords pass = passwords[i];

                Console.WriteLine("[" + i + "] " + pass.ApplicationName);
            }
            Console.Write("\n");
        }
        static void delPassword(string application)
        {
            string jsonString;
            if (File.Exists(filepath))
                jsonString = File.ReadAllText(filepath);
            else
            {
                Console.WriteLine("No password stored");
                return;
            }
            if (string.IsNullOrEmpty(jsonString))
                return;

            List<Passwords> psws = new List<Passwords>();
            psws = JsonConvert.DeserializeObject<List<Passwords>>(jsonString);
            for(int i = 0; i< psws.Count; i++)
            {
                if (psws[i].ApplicationName == application)
                {
                    Console.ForegroundColor= ConsoleColor.DarkRed;
                    Console.Write("Do you really want to delete the entry of " + application + "? Then type in: " + application +"\n>");
                    
                    if (Console.ReadLine() != application)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Removel of " + application + " canceled!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    psws.RemoveAt(i);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            var json = JsonConvert.SerializeObject(psws);
            File.Delete(filepath);
            File.WriteAllText(filepath, json);
            Console.WriteLine("Succesfully remoed " + application + "from the database");

        }

        static void delPassword()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Do you really want to remove your entire password database? Then type in database\n>");
            if (Console.ReadLine() == "database")
            {
                File.Delete(filepath);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Password database completly removed");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Removel of your entire password database canceled");
            Console.ForegroundColor = ConsoleColor.White;

        }

        class Passwords
        {
            public string ApplicationName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

        }
        */


        class Password
        {
            public byte[] IV { get; set; }
            public string password { get; set; }
            public string username { get; set; }
        }

    }
}
