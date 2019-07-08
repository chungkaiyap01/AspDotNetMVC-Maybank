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
    public class BankTransactionsController : ApiController
    {
        //private AppDbContext db = new AppDbContext();
        private UnitOfWork db = new UnitOfWork();

        // GET: api/BankTransactions
        public IQueryable<BankTransaction> GetBankTransaction()
        {
            return db.BankTransaction.ReadAll();
        }

        // GET: api/BankTransactions/5
        [ResponseType(typeof(BankTransaction))]
        public IHttpActionResult GetBankTransaction(int id)
        {
            BankTransaction bankTransaction = db.BankTransaction.ReadSingle(id);
            if (bankTransaction == null)
            {
                return NotFound();
            }

            return Ok(bankTransaction);
        }

        // PUT: api/BankTransactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBankTransaction(int id, BankTransaction bankTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bankTransaction.ID)
            {
                return BadRequest();
            }

            //db.Entry(bankTransaction).State = EntityState.Modified;
            db.BankTransaction.Update(bankTransaction);

            try
            {
                db.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankTransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BankTransactions
        [ResponseType(typeof(BankTransaction))]
        public IHttpActionResult PostBankTransaction(BankTransaction bankTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BankTransaction.Add(bankTransaction);
            db.Commit();

            return CreatedAtRoute("DefaultApi", new { id = bankTransaction.ID }, bankTransaction);
        }

        // DELETE: api/BankTransactions/5
        [ResponseType(typeof(BankTransaction))]
        public IHttpActionResult DeleteBankTransaction(int id)
        {
            BankTransaction bankTransaction = db.BankTransaction.ReadSingle(id);
            if (bankTransaction == null)
            {
                return NotFound();
            }

            db.BankTransaction.Delete(bankTransaction);
            db.Commit();

            return Ok(bankTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BankTransactionExists(int id)
        {
            return db.BankTransaction.Count() > 0;
        }
    }
}