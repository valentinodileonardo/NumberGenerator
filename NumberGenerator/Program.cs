using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace ConsoleApp1
{

    // command line parameter class
    public class Options
    {
        [Option('d', "digits", Required = true, HelpText = "Specifiy how many digits should be generated (exclusive prefix), e.g. 7")]
        public int Digits { get; set; }

        [Option('p', "prefix", Required = true, HelpText = "Specifiy the international prefix AND provider prefix, e.g. +49177")]
        public string Prefix { get; set; }

        [Option('l', "label", Required = false, HelpText = "Label (group) all generated contacts will be assigned to AND prefix of contact name default: MyNewContacts")]
        public string Label { get; set; }

        [Option('a', "amount", Required = false, HelpText = "Amount of numbers you whish to generate, default: 10.000. Googles max is 12K")]
        public int Amount { get; set; }

        [Option('g', "google", Required = false, HelpText = "Specify the mode g for google csv format, vcf default.")]
        public bool Mode { get; set; }


    }

    class Program
    {
        // vcf specifications
        private static string vcfLine = "BEGIN:VCARD\nVERSION:2.1\nFN:{0}\nTEL;CELL:{1}\nEND:VCARD";

        // csv specifications
        private static string csvHeader = "Name,Given Name, Additional Name,Family Name, Yomi Name,Given Name Yomi,Additional Name Yomi,Family Name Yomi,Name Prefix, Name Suffix,Initials,Nickname,Short Name, Maiden Name,Birthday,Gender,Location,Billing Information, Directory Server,Mileage,Occupation,Hobby,Sensitivity,Priority,Subject,Notes,Language,Photo,Group Membership, Phone 1 - Type,Phone 1 - Value";
        private static string csvLine = "{0},,,,,,,,,,,,,,,,,,,,,,,,,,,,* {1} ::: * starred,Mobile,{2}";
        private static string phoneNumberFormat = "{0}{1}";

        // csv and cache
        private static string usedNumberFilePath = Path.Combine(Environment.CurrentDirectory, "generator.cache");
        private static string contactsFilePath;
        private static List<string> usedNumber = new List<string>();

        // defaults
        private static int amount = 10000;
        private static string label = "MyNewContacts";
        private static string prefix = String.Empty;
        private static int digits = -1;
        private static bool mode = false;

        // random number generator
        private static Random r = new Random();

        static void Main(string[] args)
        {
            try
            {
                var result = Parser.Default.ParseArguments<Options>(args)
                        .WithParsed<Options>(o =>
                        {
                            if (o.Digits > 0)
                                digits = o.Digits;

                            if (o.Amount > 0)
                                amount = o.Amount;

                            if (!String.IsNullOrEmpty(o.Label))
                                label = o.Label;

                            if (!String.IsNullOrEmpty(o.Prefix))
                                prefix = o.Prefix;

                            mode = o.Mode;
                        });

                // just get the timestamp in ms for that specific run
                string timeStampOfRun = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(); 

                if (result.Tag == ParserResultType.NotParsed)
                {
                    // some required parameter was not provided
                    Console.WriteLine("Press any key to close...");
                    Console.ReadKey();
                    return;
                }

                // load cache and display amount of used numbers
                if (File.Exists(usedNumberFilePath))
                    usedNumber = File.ReadAllLines(usedNumberFilePath).ToList();
                Console.WriteLine(String.Format("{0} phone numbers loaded from the cache", usedNumber.Count));

                if (mode)
                    contactsFilePath = Path.Combine(Environment.CurrentDirectory, "contacts.csv");
                else
                    contactsFilePath = Path.Combine(Environment.CurrentDirectory, "contacts.vcf");

                if (File.Exists(contactsFilePath))
                {
                    string newFilePath = contactsFilePath + String.Format("_{0}", timeStampOfRun);
                    File.Move(contactsFilePath, newFilePath);
                    Console.WriteLine(String.Format("{0} moved to {1}", contactsFilePath, newFilePath));
                }

                using (var tw = new StreamWriter(contactsFilePath, true))
                {
                    if (mode)
                        // write this line only if we are in csv mode
                        tw.WriteLine(csvHeader);

                    for (int i = 0; i < amount; i++)
                    {
                        while (true)
                        {
                            int random = 0;
                            switch (digits)
                            {
                                case 9:
                                    random = r.Next(10000000, 999999999);
                                    break;
                                case 8:
                                    random = r.Next(1000000, 99999999);
                                    break;
                                case 7:
                                    random = r.Next(1000000, 9999999);
                                    break;
                                case 6:
                                    random = r.Next(100000, 999999);
                                    break;
                                case 5:
                                    random = r.Next(10000, 99999);
                                    break;
                            }
                            string number = String.Format(phoneNumberFormat, prefix, random);
                            if (!usedNumber.Contains(number))
                            {
                                usedNumber.Add(number);
                                if (mode)
                                    tw.WriteLine(string.Format(csvLine, label + i, label, number));
                                else
                                    tw.WriteLine(string.Format(vcfLine, label + i, number));
                                break;
                            }
                            else
                                Console.WriteLine(String.Format("{0} already used, trying next...", number));
                        }
                    }
                }

                // check if an old cache exists and rename the file
                if (File.Exists(usedNumberFilePath))
                    File.Move(usedNumberFilePath, usedNumberFilePath + String.Format("_{0}", timeStampOfRun));

                // write current cache file
                File.WriteAllLines(usedNumberFilePath, usedNumber);

                Console.WriteLine("Finish.");
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

