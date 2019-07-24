using Maybank.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maybank.DomainModelEntity.Enums;
using Maybank.InfrastructurePersistent.Context;
using Maybank.InfrastructurePersistent.UnitOfWork;
using Maybank.DomainModelEntity.Entities;
using System.Net.Http;
using System.Configuration;

namespace Maybank.Web.Controllers
{
    public class FundTransfersController : Controller
    {
        private UnitOfWork db = new UnitOfWork();
        private string controller = "BankAccounts";
        private HttpResponseMessage response;

        // GET: FundTransfers
        public ActionResult Index()
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            int CustomerID = Convert.ToInt32(Session["CustomerID"]);
            BankAccount bankAccount = db.BankAccount.ReadByCustomerID(CustomerID);
            VMFundTransfer fundTransfer = new VMFundTransfer();

            ViewBag.AccountType = new SelectList(db.BankAccount.IReadByCustomerID(CustomerID), "AccountType", "AccountType");
            ViewBag.AccountNo = new SelectList(db.BankAccount.IReadByCustomerID(CustomerID), "AccountNo", "AccountNo");
            fundTransfer.AccountBalance = bankAccount.AccountBalance;

            return View(fundTransfer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VMFundTransfer VMFundTransfer)
        {
            int CustomerID = Convert.ToInt32(Session["CustomerID"]);
            BankAccount bankAccount = db.BankAccount.ReadByCustomerID(CustomerID);
            VMFundTransfer fundTransfer = new VMFundTransfer();

            ViewBag.AccountType = new SelectList(db.BankAccount.IReadByCustomerID(CustomerID), "AccountType", "AccountType");
            ViewBag.AccountNo = new SelectList(db.BankAccount.IReadByCustomerID(CustomerID), "AccountNo", "AccountNo");
            fundTransfer.AccountBalance = bankAccount.AccountBalance;

            BankAccount OwnerAccount = db.BankAccount.ReadByCustomerID(Convert.ToInt32(Session["CustomerID"]));
            BankAccount RecipientAccount = db.BankAccount.ReadByAccountNo(VMFundTransfer.RecipientAccount);

            if (RecipientAccount == null || RecipientAccount.Bank != VMFundTransfer.RecipientBank)
            {
                ViewBag.ErrorMessage = "Recipient Account Does Not Exist !";

                return View(fundTransfer);
            }

            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{OwnerAccount.ID}")).Result;
            BankAccount Owner = response.Content.ReadAsAsync<BankAccount>().Result;
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{RecipientAccount.ID}")).Result;
            BankAccount Recipient = response.Content.ReadAsAsync<BankAccount>().Result;

            Owner.AccountBalance -= VMFundTransfer.TransactionAmount;
            Recipient.AccountBalance += VMFundTransfer.TransactionAmount;

            TransferMode transferMode = VMFundTransfer.TransferMode;
            string transferModeChosen = transferMode.ToString();
            decimal transferModeChargeAmount = Convert.ToDecimal(ConfigurationManager.AppSettings[$"{transferModeChosen}"]);
            Owner.AccountBalance -= transferModeChargeAmount;

            response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{Owner.ID}"), Owner).Result;

            if (response.IsSuccessStatusCode == false)
            {
                ViewBag.ErrorMessage = "Fund Transaction Fail !";
                ViewBag.ErrorMessage2 = "Please Try Again Later.";

                return View(fundTransfer);
            }

            response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{Recipient.ID}"), Recipient).Result;

            if (response.IsSuccessStatusCode == false)
            {
                ViewBag.ErrorMessage = "Fund Transaction Fail !";
                ViewBag.ErrorMessage2 = "Please Try Again Later.";

                return View(fundTransfer);
            }

            // GlobalVariable.SendEmail(Owner.Customer.Email, "Fund Transaction", $"Hi {Owner.Customer.Fullname},<br/>You did an {transferModeChosen} Transfer of RM {VMFundTransfer.TransactionAmount} on {DateTime.Now} to {Recipient.Customer.Fullname}.<br/>For enquiry, please call 999.");
            // GlobalVariable.SendEmail(Recipient.Customer.Email, "Fund Transaction", $"Hi {Recipient.Customer.Fullname},<br/>You receive an {transferModeChosen} Transfer of RM {VMFundTransfer.TransactionAmount} on {DateTime.Now} from {Owner.Customer.Fullname}.<br/>For enquiry, please call 999.");

            BankType recipientBankType = VMFundTransfer.RecipientBank;
            string recipientBank = recipientBankType.ToString();

            BankTransaction OwnerBankTransaction = new BankTransaction();
            OwnerBankTransaction.BankAccountID = Owner.ID;
            OwnerBankTransaction.TransactionDateTime = DateTime.Now;
            OwnerBankTransaction.TransactionAmount = VMFundTransfer.TransactionAmount;
            OwnerBankTransaction.TransferMode = VMFundTransfer.TransferMode;
            OwnerBankTransaction.Remarks = $"Transfer to {Recipient.Customer.Fullname} {recipientBankType}({Recipient.AccountNo}).";

            BankType ownerBankType = VMFundTransfer.RecipientBank;
            string ownerBank = ownerBankType.ToString();

            BankTransaction RecipientBankTransaction = new BankTransaction();
            RecipientBankTransaction.BankAccountID = Recipient.ID;
            RecipientBankTransaction.TransactionDateTime = DateTime.Now;
            RecipientBankTransaction.TransactionAmount = VMFundTransfer.TransactionAmount;
            RecipientBankTransaction.TransferMode = VMFundTransfer.TransferMode;
            RecipientBankTransaction.Remarks = $"Receive from {Owner.Customer.Fullname} {ownerBank}({Owner.AccountNo}).";

            controller = "BankTransactions";

            response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, OwnerBankTransaction).Result;
            response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, RecipientBankTransaction).Result;



            ViewBag.SuccessMessage = "Fund Transaction Successful !";

            return View(fundTransfer);
        }
    }
}