using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagmentSystem.Models.DB;

namespace HotelManagmentSystem.Controllers
{
    public class tbl_bookingController : Controller
    {
        private HMSDBContext db = new HMSDBContext();

        // GET: tbl_booking
        public ActionResult Index()
        {
            var tbl_booking = db.tbl_booking.Include(t => t.tbl_room).Include(t => t.tbl_payment_type);
            return View(tbl_booking.ToList());
        }

        // GET: tbl_booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_booking tbl_booking = db.tbl_booking.Find(id);
            if (tbl_booking == null)
            {
                return HttpNotFound();
            }
            return View(tbl_booking);
        }

        // GET: tbl_booking/Create
        // GET: tbl_booking/Create
        // GET: tbl_booking/Create
        public ActionResult Create()
        {
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            return View();
        }


        // POST: tbl_booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customer_name,customer_address,customer_email,customer_phone_no,booking_from,booking_to,payment_type,assigned_room,no_of_members,total_amount")] tbl_booking tbl_booking)
        {
            if (ModelState.IsValid)
            {
                List<tbl_booking> conflictingBookings;
                bool isRoomAvailable = IsRoomAvailable(tbl_booking.assigned_room, tbl_booking.booking_from, tbl_booking.booking_to, out conflictingBookings, null);

                if (isRoomAvailable)
                {
                    try
                    {
                        db.tbl_booking.Add(tbl_booking);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                ModelState.AddModelError(
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }
                        // Handle validation errors or other exceptions here
                    }
                }
                else
                {
                    ViewBag.ConflictingBookings = conflictingBookings;
                    ModelState.AddModelError("", "The selected room is not available for the specified period.");
                }
            }

            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number", tbl_booking.assigned_room);
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type", tbl_booking.payment_type);
            return View(tbl_booking);
        }

        private bool IsRoomAvailable(int roomNumber, DateTime fromDate, DateTime toDate, out List<tbl_booking> conflictingBookings, int? currentBookingId)
        {
            conflictingBookings = db.tbl_booking
                .Where(b =>
                    b.assigned_room == roomNumber &&
                    (currentBookingId == null || b.booking_id != currentBookingId) && // Exclude the current booking being edited
                    ((fromDate >= b.booking_from && fromDate <= b.booking_to) ||
                    (toDate >= b.booking_from && toDate <= b.booking_to) ||
                    (fromDate <= b.booking_from && toDate >= b.booking_to)))
                .ToList();

            bool isAvailable = conflictingBookings.Count == 0;

            return isAvailable;
        }





        // GET: tbl_booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_booking tbl_booking = db.tbl_booking.Find(id);
            if (tbl_booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number", tbl_booking.assigned_room);
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type", tbl_booking.payment_type);
            return View(tbl_booking);
        }

        // POST: tbl_booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "booking_id,customer_name,customer_address,customer_email,customer_phone_no,booking_from,booking_to,payment_type,assigned_room,no_of_members,total_amount")] tbl_booking tbl_booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number", tbl_booking.assigned_room);
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type", tbl_booking.payment_type);
            return View(tbl_booking);
        }

        // GET: tbl_booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_booking tbl_booking = db.tbl_booking.Find(id);
            if (tbl_booking == null)
            {
                return HttpNotFound();
            }
            return View(tbl_booking);
        }

        // POST: tbl_booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_booking tbl_booking = db.tbl_booking.Find(id);
            db.tbl_booking.Remove(tbl_booking);
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
