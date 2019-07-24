using Maybank.InfrastructurePersistent.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Maybank.UIConsole
{
    static class GeneralFunction
    {
        private static UnitOfWork db = new UnitOfWork();

        public static long AccountNoGeneration()
        {
            var random = new Random();
            string s = string.Empty;
            int length = 12;
            bool check = false;

            do
            {
                for (int i = 0; i < length; i++)
                {
                    s = string.Concat(s, random.Next(10).ToString());

                    if (s[0] == '0')
                    {
                        s = string.Empty;
                        i--;
                    }
                }

                check = db.BankAccount.SearchDuplicateAccountNo(Convert.ToInt64(s));

            } while (check == true);


            return Convert.ToInt64(s);
        }

        public static int AgeCalculate(DateTime dob)
        {
            var today = DateTime.Today;
            var month = today.Month - dob.Month;
            int age = today.Year - dob.Year;

            if (month < 0 || (month == 0 && today.Date < dob.Date))
                age--;

            return age;
        }

        public static string PasswordMaking()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            Console.WriteLine();

            return pass;
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.
        // If second parameter is true, the return string is lowercase
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)
        public static string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(RandomNumber(3, 7), true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(RandomNumber(3, 7), false));
            return builder.ToString();
        }

        public static void SendEmail(string RecipientEmail, string Subject, string Content)
        {
            //Configuring webMail class to send emails

            //gmail smtp server
            WebMail.SmtpServer = "smtp-mail.outlook.com";

            //gmail port to send emails
            WebMail.SmtpPort = 587;

            WebMail.SmtpUseDefaultCredentials = true;

            //sending emails with secure protocol
            WebMail.EnableSsl = true;

            //Email ID used to send emails from application
            WebMail.UserName = "kaichung0070@outlook.com";
            WebMail.Password = "swan1113";

            //Sender email address
            WebMail.From = "kaichung0070@outlook.com";

            //Send email 
            WebMail.Send(to: RecipientEmail, subject: Subject, body: Content, isBodyHtml: true);
        }
    }
}
