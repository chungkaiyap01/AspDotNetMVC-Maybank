using Maybank.DomainModelEntity.Entities;
using Maybank.InfrastructurePersistent.UnitOfWork;
using Maybank.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Maybank.Web.Controllers
{
    public class LoginsController : Controller
    {
        private UnitOfWork db = new UnitOfWork();
        private string controller = "Customers";
        private HttpResponseMessage response;

        // GET: Logins
        public ActionResult Index()
        {
            try
            {
                if (!string.IsNullOrEmpty(Session["RegisterMsg"].ToString()))
                {
                    ViewBag.RegisterMsg = Session["RegisterMsg"].ToString();
                    Session.Remove("RegisterMsg");
                }
            }
            catch
            {

            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VMLogin VMLogin)
        {
            Customer customer = db.Customer.LoginValidation(VMLogin.Username, VMLogin.Password);

            if (customer == null || (VMLogin.Username == null || VMLogin.Password == null))
            {
                ViewBag.ErrorMessage = "Wrong Username or Password !";
                ViewBag.ErrorMessage2 = "Please try again later.";

                return View();
            }

            Session["CustomerID"] = customer.ID;

            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(VMRegister VMRegister)
        {
            Customer customer = db.Customer.CheckBankAccount(VMRegister.Fullname, VMRegister.NRIC);

            if (customer == null)
            {
                ViewBag.ErrorMessage = "Bank Account not yet registered !";
                ViewBag.ErrorMessage2 = "Please visit nearest Maybank branch to register a bank account.";
                
                return View();
            }
            else if (customer.Username != null && customer.Password != null)
            {
                ViewBag.ErrorMessage = "Online account already registered !";
                ViewBag.ErrorMessage2 = "Please proceed to login.";
            }

            if (db.Customer.CheckRegisteredAccount(VMRegister.Username, VMRegister.Password))
            {
                ViewBag.ErrorMessage = "Online account already registered !";
                ViewBag.ErrorMessage2 = "Please proceed to login.";
            }

            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller,$"/{customer.ID}")).Result;
            Customer customerUpdate = response.Content.ReadAsAsync<Customer>().Result;
            customerUpdate.Username = VMRegister.Username;
            customerUpdate.Password = VMRegister.Password;
            response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{customerUpdate.ID}"), customerUpdate).Result;

            if (response.IsSuccessStatusCode == true)
            {
                Session["RegisterMsg"] = "Register Successful ! Please proceed to login.";
            }
            else
            {
                Session["RegisterMsg"] = "Register Unsuccessful ! Please try again later.";
            }


            return RedirectToAction("Index","Logins");
        }

        [HttpPost]
        public ActionResult AjaxCheckRegisteredAccount(string Fullname, string NRIC)
        {
            string result = "";
            bool showHidden = false;

            Customer customer = db.Customer.CheckBankAccount(Fullname, NRIC);

            if (customer == null)
            {
                result = "Account not yet registered ! Please visit nearest Maybank branch to register a bank account.";
            }
            else if (customer.Username != null && customer.Password != null)
            {
                result = "Online account already registered ! Please proceed to login.";
            }
            else
            {
                result = "Account valid !";
                showHidden = true;
            }

            return Json(new { success = true, msg1 = result, valid = showHidden }, JsonRequestBehavior.AllowGet);
        }
    }
}