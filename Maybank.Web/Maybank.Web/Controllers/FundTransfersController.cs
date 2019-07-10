using Maybank.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maybank.DomainModelEntity.Enums;

namespace Maybank.Web.Controllers
{
    public class FundTransfersController : Controller
    {
        // GET: FundTransfers
        public ActionResult Index()
        {
            VMFundTransfer vmFundTransfer = new VMFundTransfer();
            vmFundTransfer.AccountNo = 123456789123;
            vmFundTransfer.AccountType = AccountType.Savings_Account;
            vmFundTransfer.AccountBalance = 2500;

            return View(vmFundTransfer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VMFundTransfer VMFundTransfer)
        {
            return View();
        }
    }
}