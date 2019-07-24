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
    public class AdministratorsController : Controller
    {
        //private AppDbContext db = new AppDbContext();
        //private UnitOfWork db = new UnitOfWork();

        private string controller = "Administrators";
        private HttpResponseMessage response;

        // GET: Administrators
        public ActionResult Index()
        {
            if (GlobalVariable.PreventIntruder(Session["CustomerID"]))
            {
                return RedirectToAction("Index", "Logins");
            }

            response = GlobalVariable.WebApiClient.GetAsync(controller).Result;
            IEnumerable<Administrator> administratorList = response.Content.ReadAsAsync<IEnumerable<Administrator>>().Result;

            return View(administratorList);

            //return View(db.Administrator.ReadAll());
        }

        // GET: Administrators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Administrator administrator = response.Content.ReadAsAsync<Administrator>().Result;
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // GET: Administrators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,Fullname")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PostAsJsonAsync(string.Concat(controller, $"/{administrator.ID}"), administrator).Result;
                //db.Administrator.Add(administrator);
                //db.Commit();
                return RedirectToAction("Index");
            }

            return View(administrator);
        }

        // GET: Administrators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Administrator administrator = response.Content.ReadAsAsync<Administrator>().Result;
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Password,Fullname")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{administrator.ID}"), administrator).Result;

                //db.Entry(administrator).State = EntityState.Modified;
                //db.Administrator.Update(administrator);
                //db.Commit();
                return RedirectToAction("Index");
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Administrator administrator = response.Content.ReadAsAsync<Administrator>().Result;
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            response = GlobalVariable.WebApiClient.DeleteAsync(string.Concat(controller, $"/{id}")).Result;

            //Administrator administrator = db.Administrator.ReadSingle(id);
            ////db.Administrator.Remove(administrator);
            //db.Administrator.Delete(administrator);
            //db.Commit();
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
