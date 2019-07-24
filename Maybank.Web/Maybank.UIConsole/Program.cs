using ConsoleTables;
using Maybank.DomainModelEntity.Entities;
using Maybank.DomainModelEntity.Enums;
using Maybank.InfrastructurePersistent.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            Console.WriteLine();
            ReadAllCustomer();
            Console.WriteLine();
            Console.Write("Please type the customer ID you want to manage : ");
            int customerID = Convert.ToInt32(Console.ReadLine());
            Customer customer = db.Customer.ReadSingle(customerID);

            Console.WriteLine();
            Console.WriteLine($"{customer.Fullname} {customer.NRIC} selected ");
            Console.WriteLine("1. Edit Customer Profile");
            Console.WriteLine("2. Reset Customer Password");
            Console.WriteLine("3. Delete Customer Profile and Bank Account");
            Console.WriteLine("4. Back To Main Menu");
            Console.Write("Please choose your option : ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    EditCustomerProfile(customerID);
                    break;

                case "2":
                    ResetCustomerPassword(customerID);
                    break;

                case "3":
                    DeleteCustomerProfileAndBankAccount(customerID);
                    break;

                default:
                    break;
            }
        }

        private static void ResetCustomerPassword(int CustomerID)
        {
            Console.WriteLine();
            Console.WriteLine("Resetting Password...");
            Thread.Sleep(2000);

            controller = "Customers";
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{CustomerID}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;

            customer.Password = GeneralFunction.RandomPassword();

            response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{customer.ID}"), customer).Result;

            GeneralFunction.SendEmail(customer.Email,"Maybank Password Reset",$"Here is your new password {customer.Password}.");

            Console.WriteLine("Password reset successful !");
            Console.WriteLine("New password is send to customer's email");
            Console.WriteLine();
        }

        private static void EditCustomerProfile(int CustomerID)
        {
            ReadSingleCustomerForEdit(CustomerID);
            Console.WriteLine();

            string editcol = "";
            Dictionary<int, string> Table = new Dictionary<int, string>()
            {
                { 1,"Fullname"},
                { 2,"NRIC"},
                { 3,"Date Of Birth"},
                { 4,"Age"},
                { 5,"Email"},
                { 6,"Bank"}
            };

            bool check = false;

            do
            {
                Console.Write("Please insert the column number as display above that you want to edit : ");
                editcol = Console.ReadLine();
                Console.WriteLine();

                if (Table.ContainsKey(Convert.ToInt32(editcol)))
                {
                    check = true;
                }
                else
                {
                    Console.WriteLine("Invalid column number !");
                    Console.WriteLine("Please try again.");
                }

            } while (check == false);



            if (editcol == "6")
            {
                Console.WriteLine("Maybank : 0");
                Console.WriteLine("Public Bank : 1");
                Console.WriteLine("CIMB Bank : 2");
                Console.WriteLine();
            }

            Console.WriteLine("Please insert the new value you want to assign with correct format !");
            Console.Write($"{Table[Convert.ToInt16(editcol)]} : ");
            string newcolValue = Console.ReadLine();

            UpdateSingleCustomerForEdit(CustomerID, Convert.ToInt16(editcol), newcolValue);

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

        private static void ReadSingleCustomerForEdit(int CustomerID)
        {
            controller = "BankAccounts";

            BankAccount GetBankAccountID = db.BankAccount.ReadByCustomerID(CustomerID);
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{GetBankAccountID.ID}")).Result;
            BankAccount x = response.Content.ReadAsAsync<BankAccount>().Result;

            var table = new ConsoleTable("1", "2", "3", "4", "5", "6");

            table.AddRow("Fullname", "NRIC", "Date Of Birth", "Age", "Email", "Bank");

            BankType bankType = x.Bank;

            table.AddRow(x.Customer.Fullname, x.Customer.NRIC, string.Format("{0:yyyy/MM/dd}", x.Customer.DateOfBirth), x.Customer.Age, x.Customer.Email, bankType.ToString());

            table.Options.EnableCount = false;
            table.Write();
        }

        private static void UpdateSingleCustomerForEdit(int CustomerID, int editcol, string newcolValue)
        {
            controller = "Customers";
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{CustomerID}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;


            BankAccount GetBankAccountID = db.BankAccount.ReadByCustomerID(CustomerID);
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat("BankAccounts", $"/{GetBankAccountID.ID}")).Result;
            BankAccount bankAccount = response.Content.ReadAsAsync<BankAccount>().Result;

            switch (editcol)
            {
                case 1:
                    customer.Fullname = newcolValue;
                    break;

                case 2:
                    customer.NRIC = newcolValue;
                    break;

                case 3:
                    customer.DateOfBirth = Convert.ToDateTime(newcolValue);
                    break;

                case 4:
                    customer.Age = Convert.ToInt32(newcolValue);
                    break;

                case 5:
                    customer.Email = newcolValue;
                    break;

                case 6:

                    switch (Convert.ToInt32(newcolValue))
                    {
                        case 0:
                            bankAccount.Bank = BankType.Maybank;
                            break;

                        case 1:
                            bankAccount.Bank = BankType.Public_Bank;
                            break;

                        case 2:
                            bankAccount.Bank = BankType.CIMB_Bank;
                            break;

                        default:
                            Environment.Exit(0);
                            break;
                    }

                    break;

                default:
                    Environment.Exit(0);
                    break;
            }

            if(editcol == 6)
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat("BankAccounts", $"/{bankAccount.ID}"), bankAccount).Result;
            }
            else
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{customer.ID}"), customer).Result;
            }

            

            if (response.IsSuccessStatusCode == true)
            {
                Console.WriteLine("Customer Profile Updated.\nPress any key to continue");
                Console.ReadKey();
                Console.WriteLine();
            }



        }

        public static void ReadAllCustomer()
        {
            controller = "BankAccounts";

            response = GlobalVariable.WebApiClient.GetAsync(controller).Result;
            IEnumerable<BankAccount> bankAccountList = response.Content.ReadAsAsync<IEnumerable<BankAccount>>().Result;

            var table = new ConsoleTable("CustomerID", "Full Name", "NRIC", "Date Of Birth", "Age", "Email", "Account Type", "AccountNo", "Account Balance", "Bank");

            foreach (var item in bankAccountList)
            {
                AccountType accountType = item.AccountType;
                BankType bankType = item.Bank;

                table.AddRow(item.Customer.ID, item.Customer.Fullname, item.Customer.NRIC, string.Format("{0:yyyy/MM/dd}", item.Customer.DateOfBirth), item.Customer.Age, item.Customer.Email, accountType.ToString(), item.AccountNo, item.AccountBalance, bankType.ToString());
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
            customer.Password = GeneralFunction.PasswordMaking();
            if (customer.Password == "")
                customer.Password = null;

            Console.Write("Fullname : ");
            customer.Fullname = Console.ReadLine();

            Console.Write("NRIC : ");
            customer.NRIC = Console.ReadLine();

            Console.Write("Date Of Birth : ");
            customer.DateOfBirth = Convert.ToDateTime(Console.ReadLine());
            customer.Age = GeneralFunction.AgeCalculate(customer.DateOfBirth);

            Console.Write("Email : ");
            customer.Email = Console.ReadLine();

            controller = "Customers";
            response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, customer).Result;

            if (response.IsSuccessStatusCode == true)
                check = true;

            BankAccount bankAccount = new BankAccount();

            bankAccount.CustomerID = db.Customer.LatestCustomerID();
            bankAccount.AccountType = (int)AccountType.Savings_Account;
            bankAccount.AccountNo = GeneralFunction.AccountNoGeneration();

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


    }
}
