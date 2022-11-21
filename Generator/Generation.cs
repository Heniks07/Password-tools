using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Generator
{
    internal class Generation
    {
        
    


        string filePath = "C:\\Users\\Hendr\\Desktop\\pswds.txt";
        //random generator
        private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //clearing bools
        bool doClearC;
        bool doClearF;
        //diagnostics
        bool diagnostics;
        float elapsedMillisecondsToGenerate = 0;

        bool doWriteToFile = true;



        private int input()
        {

            Console.Write("password Lenght:\n>");
            int length = 0;
            string input = Console.ReadLine();
            if (doClearC) Console.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (c != '.')
                {
                    sb.Append(c);
                }
            }

            string Input = sb.ToString();

            if (Input == "?")
            {
                string doCLearCOn = doClearC ? "on" : "off";
                string doClearFOn = doClearF ? "on" : "off";
                string doWriteOn = doWriteToFile ? "on" : "off";
                string isDiagnostic = diagnostics ? "\n d\tdiagnostics activated\n f\tfile Information\n" : "";
                Console.WriteLine(
                    "______Help______\n\n" +
                    "Length\n\n" +
                    " ran\trandom lenght between 20 and 100\n" +
                    " max\tthe maximum available password length (" + (int.MaxValue / 4) + ")\n" +
                    " safe\ta predefined safe enought lenght (16)\n" +
                    " cmax\tthe maximum password lenght, that is still shown in the console (250.000)\n" +
                    "\nClearing\n\n" +
                    " cl\tswitch console clearing on input on/off (currently " + doCLearCOn + " )\n" +
                    " clf\tswitch file clearing before pswd generation on/off (curerently " + doClearFOn + " )\n" +
                    " c\tclear console now\n" +
                    " cf\tclear file now\n" +
                    " rm\tremove file now\n" +
                    "\nOther\n\n" +
                    " wf\tToggle write to file on/off (currently " + doWriteOn + " )\n" +
                    " exit\ttakes you back to the main menue\n" +
                    isDiagnostic

                    );
                return 0;
            }
            if(Input == "exit")
            {
                Console.Clear();
                return -1;
            }
            //password length options
            if (Input == "ran")
            {
                Random random = new Random();
                int ranInt = random.Next(20, 100);
                Console.WriteLine("length: " + ranInt + "\n");
                return ranInt;
            }
            if (Input == "max")
            {
                return int.MaxValue / 4;
            }
            if (Input == "min")
            {
                return 10;
            }
            if (Input == "safe")
            {
                return 16;
            }
            if (Input == "cmax")
            {
                return 250000;
            }

            //features
            if (Input == "cl")
            {
                doClearC = doClearC == true ? false : true;
                Console.WriteLine(doClearC == true ? "console clearing on input now on" : "console clearing before pswd generation now off");
                return 0;
            }
            if (Input == "clf")
            {
                doClearF = doClearF == true ? false : true;
                Console.WriteLine(doClearF == true ? "file clearing before pswd generation now on" : "file clearing before pswd generation now off");
                return 0;
            }
            if (Input == "c")
            {
                Console.Clear();
                return 0;
            }
            if (Input == "cf")
            {
                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine("file cleared");
                return 0;
            }
            if (Input == "rm")
            {
                File.Delete(filePath);
                Console.WriteLine("file removed");
                return 0;
            }
            if (Input == "wf")
            {
                doWriteToFile = !doWriteToFile;
                Console.WriteLine(doWriteToFile ? "toggled on write to file: now creates a file with your generated password at\n" + filePath + "\n" : "toggled off write to file: won't create anny files\n");
                return 0;
            }

            if (Input == "d")
            {
                Console.WriteLine(diagnostics ? "Toggled Diagnostics off" : "Toggled Diagnostics on");
                diagnostics = !diagnostics;
                return 0;
            }


            //multi Arguments
            string[] inputArgs = Input.Split(" ");
            if (inputArgs[0] == "f")
            {
                try
                {
                    if (inputArgs[1] == "-?")
                    {
                        Console.WriteLine(
                            "______f______\n\n" +
                            " -i\tinformation about the file (third argument requiered)\n" +
                            "\t-i -s\tfile size\n" +
                            "\t-i -dir\tfile directory and name\n\n" +
                            " -path <drive>:\\<folder>\n\tsets the folder where the file schould be stored\n"
                            );
                        return 0;
                    }
                    if (inputArgs[1] == "-i")
                    {
                        if (inputArgs.Length == 2)
                        {
                            Console.WriteLine("___Error___\nf -i requiers a third argument! f -? for help\n");
                            return 0;
                        }
                        if (inputArgs[2] == "s")
                        {
                            FileInfo fi = new FileInfo(filePath);
                            try
                            {
                                string fileSize = "0 Bytes";
                                if (fi.Length < 1024)
                                {
                                    fileSize = fi.Length + " Bytes";
                                }
                                else if (fi.Length < 1024 * 1024)
                                {
                                    fileSize = (fi.Length / 1024) + " KiB";
                                }
                                else if (fi.Length < 1024 * 1024 * 1024)
                                {
                                    fileSize = (fi.Length / (1024 * 1024)) + " MiB";
                                }
                                else
                                {
                                    fileSize = (fi.Length / (1024 * 1024 * 1024)) + " GiB";
                                }
                                Console.WriteLine("\n File size: " + fileSize + "\n");
                            }
                            catch (FileNotFoundException)
                            {
                                Console.WriteLine(doWriteToFile ? "file doesn't exist! Generate password to create file\n" : "file doesn't exist! Turn on write to file (wf) and generate a password to create file\n");
                            }
                        }
                        if (inputArgs[2] == "dir")
                        {

                            Console.WriteLine("File Path\\Name: " + filePath + "\n");
                        }
                        return 0;
                    }
                    if (inputArgs[1] == "-path")
                    {
                        if (inputArgs.Length == 2)
                        {
                            Console.WriteLine("___Error___\nf -path requiers a third argument that contains the new path where the file should be stored. E.g f -path C:\\Users\\root\\Desktop\n");
                            return 0;
                        }

                        if (Directory.Exists(inputArgs[2]))
                        {
                            filePath = inputArgs[2] + "\\pswds.txt";
                        }
                        else
                        {
                            Console.WriteLine("___Error___\nplease input a valid path! E.g C:\\Users\\root\\Desktop\n");
                        }
                        return 0;
                    }

                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("___Error___\nf requiers two or more arguments. (f -? for help)\n");
                    return 0;
                }
            }

            //generation
            if (float.TryParse(Input, out float f))
            {
                if (f > int.MaxValue)
                {
                    Console.WriteLine("way to high number (max: " + (int.MaxValue / 4) + " ) (? for help)");
                    return 0;
                }
            }
            if (int.TryParse(Input, out length))
            {
                if (length <= int.MaxValue / 4)
                    return length;
                else
                {
                    veryLongPassword(length);
                    //Console.WriteLine("to high number (max: " + (int.MaxValue / 4) + " ) (? for help)");
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("pleas input only numbers between 10 and " + (int.MaxValue / 4) + " ) (? for help)");
                return 0;
            }
        }

        public void Run()
        {
            Console.Title = "Password generator";
            Stopwatch sw = new Stopwatch();

            float elapsedMillisecondsToWriteToConsole;
            float elapsedMillisecondsToWriteToFile;
            int inputint = input();
            //rerun
            if (inputint == 0) Run();
            //exit
            if (inputint == -1)
            {
                Program program = new Program();
                program.returning();
            }

            string pswd = trueRandom(inputint);

            

            if (doClearF) File.WriteAllText(filePath, string.Empty);

            if (pswd.Length <= 9)
            {
                string timeToCrack;
                switch (pswd.Length)
                {
                    case 6:
                        {
                            timeToCrack = "only 20sec";
                            break;
                        }
                    case 7:
                        {
                            timeToCrack = "only 50min";
                            break;
                        }
                    case 8:
                        {
                            timeToCrack = "\"only\" 2d";
                            break;
                        }
                    case 9:
                        {
                            timeToCrack = "\"only?\" 2y";
                            break;
                        }
                    default:
                        {
                            timeToCrack = "less than 1sec";
                            break;
                        }
                }
                Console.WriteLine("Verry short password. This Password could get hacked in " + timeToCrack + ". Don't use it for your bank account or similar. If you want to continue nonetheless press \"Enter\", otherwise press any other key and create a password longer than 8 characters\n");

                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    //diagnostics
                    sw.Start();

                    if (doWriteToFile)
                        File.AppendAllText(filePath, pswd + "\r\n\r\n");

                    //diagnostics
                    sw.Stop();
                    elapsedMillisecondsToWriteToFile = sw.ElapsedMilliseconds;
                    sw.Restart();

                    Console.WriteLine(pswd);

                    //diagnostics
                    elapsedMillisecondsToWriteToConsole = sw.ElapsedMilliseconds;
                    sw.Stop();
                    if (diagnostics)
                    {
                        Console.Write("\nGenerated in " + elapsedMillisecondsToGenerate + " milliseconds\n" +
                        "Written to Console in " + elapsedMillisecondsToWriteToConsole + " milliseconds\n");
                        Console.WriteLine(
                        doWriteToFile ? "Written to File in " + elapsedMillisecondsToWriteToFile + " milliseconds" : "didn't write to file");
                    }

                    Console.Write(doClearC == true ? "\n" : "");
                    Run();
                }
                Run();
            }
            if (pswd.Length > 250000)
            {
                //diagnostics
                sw.Start();

                if (doWriteToFile)
                    File.AppendAllText(filePath, pswd + "\r\n\r\n");

                //diagnostics
                sw.Stop();
                elapsedMillisecondsToWriteToFile = sw.ElapsedMilliseconds;

                Console.WriteLine("only wrote to file because more than 250.000 characters ( " + pswd.Length + " )");
                //diagnostics
                if (diagnostics)
                {
                    Console.Write("\nGenerated in " + elapsedMillisecondsToGenerate + " milliseconds\n");
                    Console.WriteLine(
                        doWriteToFile ? "Written to File in " + elapsedMillisecondsToWriteToFile + " milliseconds" : "didn't write to file");
                }

                Console.Write(doClearC == true ? "\n" : "");
            }
            else
            {
                //diagnostics
                sw.Start();

                if (doWriteToFile)
                    File.AppendAllText(filePath, pswd + "\r\n\r\n");

                //diagnostics
                elapsedMillisecondsToWriteToFile = sw.ElapsedMilliseconds;
                sw.Restart();

                Console.WriteLine(pswd);

                //diagnostics
                sw.Stop();
                elapsedMillisecondsToWriteToConsole = sw.ElapsedMilliseconds;
                sw.Stop();
                if (diagnostics)
                {
                    Console.Write("\nGenerated in " + elapsedMillisecondsToGenerate + " milliseconds\n" +
                        "Written to Console in " + elapsedMillisecondsToWriteToConsole + " milliseconds\n");
                    Console.WriteLine(
                        doWriteToFile ? "Written to File in " + elapsedMillisecondsToWriteToFile + " milliseconds" : "didn't write to file");
                }

                Console.Write(doClearC == true ? "\n" : "");
            }
            Run();
        }

        private void veryLongPassword(int length)
        {

            if (doClearF) File.WriteAllText(filePath, string.Empty);

            //diagnostics
            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            float elapsedMillisecondsToWriteToFile = 0;

            double pswdLength = length / 536870911d;
            int roundedLength = (int)Math.Ceiling(pswdLength);
            string[] pswds = new string[roundedLength];

            int alreadeyDone = 0;

            //diagnostics
            sw2.Start();
            sw.Start();


            //generate
            for (int i = 0; i < roundedLength; i++)
            {
                int I = length - alreadeyDone;
                I = Math.Clamp(I, 0, 536870911);
                pswds[i] = trueRandom(I);

                alreadeyDone += I;


                //diagnsotics
                if (diagnostics) Console.WriteLine(Math.Floor(alreadeyDone / (double)length * 100d) + "% generated");
            }
            //diagnsotics
            sw.Stop();
            elapsedMillisecondsToGenerate = sw.ElapsedMilliseconds;
            sw.Restart();

            Console.Write("\n");
            if (doWriteToFile)
            {
                for (int i = 0; i < roundedLength; i++)
                {
                    File.AppendAllText(filePath, pswds[i]);
                    if (diagnostics) Console.WriteLine(((i + 1) / (double)roundedLength * 100d) + "% written to File");
                }
            }
            Console.Write("\n");

            //diagnostics
            sw.Stop();
            elapsedMillisecondsToWriteToFile = sw.ElapsedMilliseconds;
            sw2.Stop();

            Console.WriteLine("only wrote to file because more than 250.000 characters ( " + length + " )");

            //diagnsotics
            if (diagnostics)
            {
                Console.Write("\n" +
                    "Generated in " + elapsedMillisecondsToGenerate + " milliseconds\n");
                Console.Write(
                    doWriteToFile ? "Written to File in " + elapsedMillisecondsToWriteToFile + " milliseconds\n" : "didn't write to file\n");
                Console.WriteLine(
                    "Whole Task completet in " + sw2.ElapsedMilliseconds + " milliseconds");
            }



        }

        public string trueRandom(int size)
        {
            if (size <= 0) return "";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            byte[] randomValue = new byte[size];
            using (RNGCryptoServiceProvider randomValueGenerator = new RNGCryptoServiceProvider())
            {
                randomValueGenerator.GetBytes(randomValue);
            }
            sw.Stop();
            elapsedMillisecondsToGenerate = sw.ElapsedMilliseconds;


            string pswd = Convert.ToBase64String(randomValue);
            return pswd?.Substring(0, Math.Min(pswd.Length, size));
        }
    }
}


