using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maybank.InfrastructurePersistent.Context;
using Maybank.DomainModelEntity.Entities;
using System.Net.Http;
using Maybank.InfrastructurePersistent.UnitOfWork;

namespace Maybank.Web.Controllers
{
    public class BankAccountsController : Controller
    {
        private UnitOfWork db = new UnitOfWork();
        private string controller = "BankAccounts";
        private HttpResponseMessage response;

        // GET: BankAccounts
        public ActionResult Index()
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            string CustomerID = Session["CustomerID"].ToString();
            BankAccount bankAccountByCustomerID = db.BankAccount.ReadByCustomerID(Convert.ToInt32(CustomerID));

            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{bankAccountByCustomerID.ID}")).Result;
            BankAccount bankAccount = response.Content.ReadAsAsync<BankAccount>().Result;

            return View(bankAccount);


            //var bankAccount = db.BankAccount.Include(b => b.Customer);
            //return View(bankAccount.ToList());
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            BankAccount bankAccount = response.Content.ReadAsAsync<BankAccount>().Result;
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            //ViewBag.CustomerID = new SelectList(db.Customer, "ID", "Username");
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,AccountType,AccountNo,AccountBalance,Bank")] BankAccount bankAccount)
        {
            string CustomerID = Session["CustomerID"].ToString();
            bankAccount.CustomerID = Convert.ToInt32(CustomerID);

            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(controller, bankAccount).Result;
                return RedirectToAction("Index");
            }

            //ViewBag.CustomerID = new SelectList(db.Customer, "ID", "Username", bankAccount.CustomerID);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            BankAccount bankAccount = response.Content.ReadAsAsync<BankAccount>().Result;
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CustomerID = new SelectList(db.Customer, "ID", "Username", bankAccount.CustomerID);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,AccountType,AccountNo,AccountBalance,Bank")] BankAccount bankAccount)
        {
            string CustomerID = Session["CustomerID"].ToString();
            bankAccount.CustomerID = Convert.ToInt32(CustomerID);

            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{bankAccount.ID}"), bankAccount).Result;
                return RedirectToAction("Index");
            }
            //ViewBag.CustomerID = new SelectList(db.Customer, "ID", "Username", bankAccount.CustomerID);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            BankAccount bankAccount = response.Content.ReadAsAsync<BankAccount>().Result;
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            response = GlobalVariable.WebApiClient.DeleteAsync(string.Concat(controller, $"/{id}")).Result;
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
