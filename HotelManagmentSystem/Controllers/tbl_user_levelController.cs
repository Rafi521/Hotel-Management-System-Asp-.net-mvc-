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
    public class tbl_user_levelController : Controller
    {
        private HMSDBContext db = new HMSDBContext();

        // GET: tbl_user_level
        public ActionResult Index()
        {
            return View(db.tbl_user_level.ToList());
        }

        // GET: tbl_user_level/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_user_level tbl_user_level = db.tbl_user_level.Find(id);
            if (tbl_user_level == null)
            {
                return HttpNotFound();
            }
            return View(tbl_user_level);
        }

        // GET: tbl_user_level/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_user_level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_level_id,user_type")] tbl_user_level tbl_user_level)
        {
            if (ModelState.IsValid)
            {
                db.tbl_user_level.Add(tbl_user_level);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_user_level);
        }

        // GET: tbl_user_level/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_user_level tbl_user_level = db.tbl_user_level.Find(id);
            if (tbl_user_level == null)
            {
                return HttpNotFound();
            }
            return View(tbl_user_level);
        }

        // POST: tbl_user_level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_level_id,user_type")] tbl_user_level tbl_user_level)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_user_level).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_user_level);
        }

        // GET: tbl_user_level/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_user_level tbl_user_level = db.tbl_user_level.Find(id);
            if (tbl_user_level == null)
            {
                return HttpNotFound();
            }
            return View(tbl_user_level);
        }

        // POST: tbl_user_level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_user_level tbl_user_level = db.tbl_user_level.Find(id);
            db.tbl_user_level.Remove(tbl_user_level);
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
