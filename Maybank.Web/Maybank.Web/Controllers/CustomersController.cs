using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Maybank.DomainModelEntity.Entities;
using Maybank.InfrastructurePersistent.Context;

namespace Maybank.Web.Controllers
{
    public class CustomersController : Controller
    {
        private string controller = "Customers";
        private HttpResponseMessage response;

        // GET: Customeras
        public ActionResult Index()
        {
            response = GlobalVariable.WebApiClient.GetAsync(controller).Result;
            IEnumerable<Customer> customerList = response.Content.ReadAsAsync<IEnumerable<Customer>>().Result;

            return View(customerList);
        }

        // GET: Customeras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customeras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customeras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,Fullname,NRIC,DateOfBirth,Age,Email,Photo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PostAsJsonAsync(controller, customer).Result;
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customeras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customeras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Password,Fullname,NRIC,DateOfBirth,Age,Email,Photo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                response = GlobalVariable.WebApiClient.PutAsJsonAsync(string.Concat(controller, $"/{customer.ID}"), customer).Result;
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customeras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            response = GlobalVariable.WebApiClient.GetAsync(string.Concat(controller, $"/{id}")).Result;
            Customer customer = response.Content.ReadAsAsync<Customer>().Result;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customeras/Delete/5
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
