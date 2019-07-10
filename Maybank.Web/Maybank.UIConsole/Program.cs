using ConsoleTables;
using Maybank.DomainModelEntity.Entities;
using Maybank.DomainModelEntity.Enums;
using Maybank.InfrastructurePersistent.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.UIConsole
{
    class Program
    {
        private static UnitOfWork db = new UnitOfWork();
        private static string controller;
        private static HttpResponseMessage response;

        static void Main(string[] args)
        {
            string option = "";

            do
            {
                Console.WriteLine("Welcome To Administrator Menu");
                Console.WriteLine("1. Print All Customers");
                Console.WriteLine("2. Add Customer");
                Console.WriteLine("3. Manage Customer");
                Console.WriteLine("4. Exit");
                Console.Write("Choose your option : ");

                option = Console.ReadLine().ToString();

                switch (option)
                {
                    case "1":
                        ReadAllCustomer();
                        break;

                    case "2":
                        AddCustomer();
                        break;

                    case "3":
                        ManageCustomer();
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;

                    default:
                        Environment.Exit(0);
                        break;
                }

            } while (option != "4");


        }

        public static void ManageCustomer()
        {
            ReadAllCustomer();
            Console.WriteLine();
            Console.Write("Please type the customer ID you want to manage : ");
            int customerID = Convert.ToInt32(Console.ReadLine());
            Customer customer = db.Customer.ReadSingle(customerID);

            Console.WriteLine();
            Console.WriteLine($"{customer.Fullname} {customer.NRIC} selected ");
            Console.WriteLine("1. Edit Customer Profile");
            Console.WriteLine("2. Edit Customer Bank Account");
            Console.WriteLine("3. Delete Customer Profile and Bank Account");
            Console.WriteLine("4. Back To Main Menu");
            Console.Write("Please choose your option : ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":

                    break;

                case "3":
                    DeleteCustomerProfileAndBankAccount(customerID);
                    break;

                default:
                    break;
            }
        }

        private static void EditCustomerProfile(int CustomerID)
        {
            ReadSingleCustomer(CustomerID);
            Console.WriteLine();

            string[] colName = { "CustomerID", "Fullname", "NRIC", "DateOfBirth", "Age", "Email" };
            string editcolName = "";
            int editcolValue = 0;
            bool loopcheck = false;

            do
            {
                Console.Write("Please type the exact column name *(exclude CustomerID)* that you want to modify without space : ");
                editcolName = Console.ReadLine();

                foreach (var col in colName)
                {
                    if (col.ToLower() == editcolName.ToLower())
                    {
                        loopcheck = true;
                        editcolValue = Array.IndexOf(colName, col);
                    }
                }
            } while (loopcheck != true);

            Console.WriteLine("Please insert the new value you want to assign with correct format !");
            Console.Write($"{colName[editcolValue]} : ");
            string newcolValue = Console.ReadLine();






        }

        private static void DeleteCustomerProfileAndBankAccount(int CustomerID)
        {
            bool check = false;

            controller = "BankAccounts";
            BankAccount bankAccount = db.BankAccount.ReadByCustomerID(CustomerID);
            response = GlobalVariable.WebApiClient.DeleteAsync(string.Concat(controller, $"/{bankAccount.ID}")).Result;

            if (response.IsSuccessStatusCode == true)
                check = true;

            controller = "Customers";
            response = GlobalVariable.WebApiClient.DeleteAsync(string.Concat(controller, $"/{CustomerID}")).Result;

            if (response.IsSuccessStatusCode == true)
                check = true;

            if (check == true)
                Console.WriteLine("Customer Profile and Bank Account delete successfully");
        }

        private static void ReadSingleCustomer(int CustomerID)
        {
            controller = "Customers";
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{CustomerID}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;

            var table = new ConsoleTable("Customer ID", "Fullname", "NRIC", "Date Of Birth", "Age", "Email");

            table.AddRow(customer.ID, customer.Fullname, customer.NRIC, string.Format("{0:yyyy/MM/dd}", customer.DateOfBirth), customer.Age, customer.Email);

            table.Options.EnableCount = false;
            table.Write();
        }

        public static void ReadAllCustomer()
        {
            controller = "Customers";

            response = GlobalVariable.WebApiClient.GetAsync(controller).Result;
            IEnumerable<Customer> customerList = response.Content.ReadAsAsync<IEnumerable<Customer>>().Result;

            var table = new ConsoleTable("CustomerID", "Full Name", "NRIC", "Date Of Birth", "Age", "Email");

            foreach (var item in customerList)
            {
                table.AddRow(item.ID, item.Fullname, item.NRIC, string.Format("{0:yyyy/MM/dd}", item.DateOfBirth), item.Age, item.Email);
            }

            table.Options.EnableCount = false;
            table.Write();
        }

        public static void AddCustomer()
        {
            bool check = false;

            Customer customer = new Customer();

            Console.Write("Username : ");
            customer.Username = Console.ReadLine();
            if (customer.Username == "")
                customer.Username = null;

            Console.Write("Password : ");
            customer.Password = PasswordMaking();
            if (customer.Password == "")
                customer.Password = null;

            Console.Write("Fullname : ");
            customer.Fullname = Console.ReadLine();

            Console.Write("NRIC : ");
            customer.NRIC = Console.ReadLine();

            Console.Write("Date Of Birth : ");
            customer.DateOfBirth = Convert.ToDateTime(Console.ReadLine());
            customer.Age = AgeCalculate(customer.DateOfBirth);

            Console.Write("Email : ");
            customer.Email = Console.ReadLine();

            controller = "Customers";
            response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, customer).Result;

            if (response.IsSuccessStatusCode == true)
                check = true;

            BankAccount bankAccount = new BankAccount();

            bankAccount.CustomerID = db.Customer.LatestCustomerID();
            bankAccount.AccountType = (int)AccountType.Savings_Account;
            bankAccount.AccountNo = AccountNoGeneration();

            Console.Write("Account Balance : ");
            bankAccount.AccountBalance = Convert.ToDecimal(Console.ReadLine());
            bankAccount.Bank = (int)BankType.Maybank;

            controller = "BankAccounts";
            response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, bankAccount).Result;

            if (response.IsSuccessStatusCode == true)
                check = true;

            if (check == true)
                Console.WriteLine("Customer profile and bank account create successfully !");
        }

        private static long AccountNoGeneration()
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

        private static int AgeCalculate(DateTime dob)
        {
            var today = DateTime.Today;
            var month = today.Month - dob.Month;
            int age = today.Year - dob.Year;

            if (month < 0 || (month == 0 && today.Date < dob.Date))
                age--;

            return age;
        }

        private static string PasswordMaking()
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
    }
}
