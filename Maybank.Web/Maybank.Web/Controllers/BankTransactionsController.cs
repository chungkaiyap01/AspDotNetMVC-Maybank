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
using Maybank.InfrastructurePersistent.UnitOfWork;
using System.Net.Http;

namespace Maybank.Web.Controllers
{
    public class BankTransactionsController : Controller
    {
        private UnitOfWork db = new UnitOfWork();
        private string controller = "BankTransactions";
        private HttpResponseMessage response;

        // GET: BankTransactions
        public ActionResult Index()
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            BankAccount bankAccount = db.BankAccount.ReadByCustomerID(Convert.ToInt32(Session["CustomerID"]));
            //BankTransaction GetBankTransactionID = db.BankTransaction.ReadByBankAccountID(bankAccount.ID);
            //response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{GetBankTransactionID.ID}")).Result;
            //IEnumerable<BankTransaction> bankTransactionList = response.Content.ReadAsAsync<IEnumerable<BankTransaction>>().Result;
            IEnumerable<BankTransaction> bankTransactionList = db.BankTransaction.ReadByBankAccountID(bankAccount.ID);

            return View(bankTransactionList);

            //var bankTransaction = db.BankTransaction.Include(b => b.BankAccount);
            //return View(bankTransaction.ToList());
        }

        //// GET: BankTransactions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
        //    BankTransaction bankTransaction = response.Content.ReadAsAsync<BankTransaction>().Result;
        //    if (bankTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bankTransaction);
        //}

        //// GET: BankTransactions/Create
        //public ActionResult Create()
        //{
        //    ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "ID");
        //    return View();
        //}

        //// POST: BankTransactions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,BankAccountID,TransactionDateTime,TransactionAmount,TransferMode,Remarks")] BankTransaction bankTransaction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        response = GlobalVariable.WebApiClient.PostAsJsonAsync(string.Concat(controller, $"/{bankTransaction.ID}"), bankTransaction).Result;
        //        //db.BankTransaction.Add(bankTransaction);
        //        //db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    //ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "ID", bankTransaction.BankAccountID);
        //    return View(bankTransaction);
        //}

        //// GET: BankTransactions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BankTransaction bankTransaction = db.BankTransaction.Find(id);
        //    if (bankTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "ID", bankTransaction.BankAccountID);
        //    return View(bankTransaction);
        //}

        //// POST: BankTransactions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,BankAccountID,TransactionDateTime,TransactionAmount,TransferMode,Remarks")] BankTransaction bankTransaction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(bankTransaction).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    //ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "ID", bankTransaction.BankAccountID);
        //    return View(bankTransaction);
        //}

        //// GET: BankTransactions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BankTransaction bankTransaction = db.BankTransaction.Find(id);
        //    if (bankTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bankTransaction);
        //}

        //// POST: BankTransactions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BankTransaction bankTransaction = db.BankTransaction.Find(id);
        //    db.BankTransaction.Remove(bankTransaction);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
