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
using GeneByGene.Models;

namespace GeneByGene.Controllers
{
    public class SamplesController : ApiController
    {
        private GeneTasksEntities1 db = new GeneTasksEntities1();

        // GET: api/Samples
        public IQueryable<object> GetSamples()
        {
            var View1 =
                from sample in db.Samples
                join status in db.Statuses on sample.StatusId equals status.StatusId
                join user in db.Users on sample.CreatedBy equals user.UserId
                select new { sample.SampleId, sample.Barcode, sample.CreatedAt, status.Status1, user.FirstName, user.LastName };

            return View1;

            // return db.Samples;
        }

        // GET: api/Samples/5
        [ResponseType(typeof(object))]
        public IHttpActionResult GetSample(int id)
        {
            //Sample sample = db.Samples.Find(id);
            var View2 =
                from sample in db.Samples
                join status in db.Statuses on sample.StatusId equals status.StatusId
                where sample.StatusId == id
                select new { sample.SampleId, sample.Barcode, sample.CreatedAt, status.Status1 };

            if (View2 == null)
            {
                return NotFound();
            }

            return Ok(View2);
        }

        // GET: api/Samples/name/x
        [ResponseType(typeof(object))]
        //[Route("name/{nameFilter}")]
        public IHttpActionResult GetSample(string nameFilter)
        {
            //Sample sample = db.Samples.Find(id);
            var View3 =
                from sample in db.Samples
                join user in db.Users on sample.CreatedBy equals user.UserId
                //where (user.FirstName.Contains(nameFilter) || user.LastName.Contains(nameFilter))
                where user.LastName.Contains(nameFilter)
                select new { sample.SampleId, sample.Barcode, sample.CreatedAt, user.FirstName ,user.LastName };

            if (View3 == null)
            {
                return NotFound();
            }

            return Ok(View3);
        }


        // POST: api/Samples
        [ResponseType(typeof(Sample))]
        public IHttpActionResult PostSample([FromBody]Sample sample)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Samples.Add(sample);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sample.SampleId }, sample);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SampleExists(int id)
        {
            return db.Samples.Count(e => e.SampleId == id) > 0;
        }
    }
}