using Maybank.DomainModelEntity.Entities;
using Maybank.InfrastructurePersistent.UnitOfWork;
using Maybank.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maybank.Web.Controllers
{
    public class LoginsController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        // GET: Logins
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VMLogin VMLogin)
        {
            Customer customer = db.Customer.LoginValidation(VMLogin.Username, VMLogin.Password);

            if (customer == null)
            {
                ViewBag.ErrorMessage = "Wrong Username or Password !";
                ViewBag.ErrorMessage2 = "Please try again later.";

                return View();
            }

            Session["CustomerID"] = customer.ID;

            return RedirectToAction("Index", "Customers");
        }
    }
}