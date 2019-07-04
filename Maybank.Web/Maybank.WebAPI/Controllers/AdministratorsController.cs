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
    public class AdministratorsController : ApiController
    {
        //private AppDbContext db = new AppDbContext();
        private UnitOfWork db = new UnitOfWork();

        // GET: api/Administrators
        public IQueryable<Administrator> GetAdministrator()
        {
            return db.Administrator.ReadAll();
        }

        // GET: api/Administrators/5
        [ResponseType(typeof(Administrator))]
        public IHttpActionResult GetAdministrator(int id)
        {
            Administrator administrator = db.Administrator.ReadSingle(id);
            if (administrator == null)
            {
                return NotFound();
            }

            return Ok(administrator);
        }

        // PUT: api/Administrators/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAdministrator(int id, Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != administrator.ID)
            {
                return BadRequest();
            }

            //db.Entry(administrator).State = EntityState.Modified;
            db.Administrator.Update(administrator);

            try
            {
                db.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!AdministratorExists(id))
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

        // POST: api/Administrators
        [ResponseType(typeof(Administrator))]
        public IHttpActionResult PostAdministrator(Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Administrator.Add(administrator);
            db.Commit();

            return CreatedAtRoute("DefaultApi", new { id = administrator.ID }, administrator);
        }

        // DELETE: api/Administrators/5
        [ResponseType(typeof(Administrator))]
        public IHttpActionResult DeleteAdministrator(int id)
        {
            Administrator administrator = db.Administrator.ReadSingle(id);
            if (administrator == null)
            {
                return NotFound();
            }

            db.Administrator.Delete(administrator);
            db.Commit();

            return Ok(administrator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool AdministratorExists(int id)
        //{
        //    return db.Administrator.Count(e => e.ID == id) > 0;
        //}
    }
}