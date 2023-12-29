using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HotelManagmentSystem.Models.DB;

namespace HotelManagmentSystem.Controllers
{
    public class tbl_booking_statusController : Controller
    {
        private HMSDBContext db = new HMSDBContext();

        // GET: tbl_booking_status
        public ActionResult Index()
        {
            var bookingStatuses = db.tbl_booking_status.Include("tbl_room.tbl_booking");
            return View(bookingStatuses.ToList());
        }




        // GET: tbl_booking_status/ChangeStatus/5
        public ActionResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            if (bookingStatus == null)
            {
                return HttpNotFound();
            }

            // Create a list of possible booking status values
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "Confirmed", Text = "Confirmed" },
                new { Value = "Cancelled", Text = "Cancelled" }
            }, "Value", "Text", bookingStatus.booking_status); // Pre-select the current status

            return View(bookingStatus);
        }

        // POST: tbl_booking_status/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(int id, string selectedStatus)
        {
            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            if (bookingStatus == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Update the booking status
                bookingStatus.booking_status = selectedStatus;
                db.Entry(bookingStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the same view
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Pending", Text = "Pending" },
                new { Value = "Confirmed", Text = "Confirmed" },
                new { Value = "Cancelled", Text = "Cancelled" }
            }, "Value", "Text", selectedStatus); // Maintain the selected status

            return View(bookingStatus);
        }

        // GET: tbl_booking_status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            if (bookingStatus == null)
            {
                return HttpNotFound();
            }

            return View(bookingStatus);
        }


        // GET: tbl_booking_status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_booking_status/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "booking_status_id,booking_status")] tbl_booking_status bookingStatus)
        {
            if (ModelState.IsValid)
            {
                db.tbl_booking_status.Add(bookingStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingStatus);
        }

        // GET: tbl_booking_status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            if (bookingStatus == null)
            {
                return HttpNotFound();
            }
            return View(bookingStatus);
        }

        // POST: tbl_booking_status/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "booking_status_id,booking_status")] tbl_booking_status bookingStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingStatus);
        }

        // GET: tbl_booking_status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            if (bookingStatus == null)
            {
                return HttpNotFound();
            }
            return View(bookingStatus);
        }

        // POST: tbl_booking_status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_booking_status bookingStatus = db.tbl_booking_status.Find(id);
            db.tbl_booking_status.Remove(bookingStatus);
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
