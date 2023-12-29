using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagmentSystem.Models.DB;

namespace HotelManagmentSystem.Controllers
{
    public class tbl_payment_typeController : Controller
    {
        private HMSDBContext db = new HMSDBContext();

        // GET: tbl_payment_type
        public ActionResult Index()
        {
            return View(db.tbl_payment_type.ToList());
        }

        // GET: tbl_payment_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_payment_type tbl_payment_type = db.tbl_payment_type.Find(id);
            if (tbl_payment_type == null)
            {
                return HttpNotFound();
            }
            return View(tbl_payment_type);
        }

        // GET: tbl_payment_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_payment_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "payment_type_id,payment_type")] tbl_payment_type tbl_payment_type)
        {
            if (ModelState.IsValid)
            {
                db.tbl_payment_type.Add(tbl_payment_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_payment_type);
        }

        // GET: tbl_payment_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_payment_type tbl_payment_type = db.tbl_payment_type.Find(id);
            if (tbl_payment_type == null)
            {
                return HttpNotFound();
            }
            return View(tbl_payment_type);
        }

        // POST: tbl_payment_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "payment_type_id,payment_type")] tbl_payment_type tbl_payment_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_payment_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_payment_type);
        }

        // GET: tbl_payment_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_payment_type tbl_payment_type = db.tbl_payment_type.Find(id);
            if (tbl_payment_type == null)
            {
                return HttpNotFound();
            }
            return View(tbl_payment_type);
        }

        // POST: tbl_payment_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_payment_type tbl_payment_type = db.tbl_payment_type.Find(id);
            db.tbl_payment_type.Remove(tbl_payment_type);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
