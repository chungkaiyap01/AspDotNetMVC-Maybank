using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Maybank.InfrastructurePersistent.Context;
using Maybank.DomainModelEntity.Entities;
using Maybank.InfrastructurePersistent.UnitOfWork;

namespace Maybank.WebAPI.Controllers
{
    public class BankAccountsController : ApiController
    {
        //private AppDbContext db = new AppDbContext();
        private UnitOfWork db = new UnitOfWork();

        // GET: api/BankAccounts
        public IQueryable<BankAccount> GetBankAccount()
        {
            return db.BankAccount.ReadAll();
        }

        // GET: api/BankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult GetBankAccount(int id)
        {
            BankAccount bankAccount = db.BankAccount.ReadSingle(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return Ok(bankAccount);
        }

        // PUT: api/BankAccounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBankAccount(int id, BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bankAccount.ID)
            {
                return BadRequest();
            }

            //db.Entry(bankAccount).State = EntityState.Modified;
            db.BankAccount.Update(bankAccount);

            try
            {
                db.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!BankAccountExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BankAccounts
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult PostBankAccount(BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BankAccount.Add(bankAccount);
            db.Commit();

            return CreatedAtRoute("DefaultApi", new { id = bankAccount.ID }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult DeleteBankAccount(int id)
        {
            BankAccount bankAccount = db.BankAccount.ReadSingle(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            db.BankAccount.Delete(bankAccount);
            db.Commit();

            return Ok(bankAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool BankAccountExists(int id)
        //{
        //    return db.BankAccount.Count(e => e.ID == id) > 0;
        //}
    }
}