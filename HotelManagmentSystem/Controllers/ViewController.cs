using HotelManagmentSystem.Models;
using HotelManagmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Validation;
//System.Diagnostics.Debug.WriteLine

namespace HotelManagmentSystem.Controllers
{
    public class ViewController : Controller
    {
        public HMSDBContext db = new HMSDBContext();

        // GET: View
        public ActionResult Home()
        {
            var bookedStatusId = 1; // Set the correct booking status ID for "Booked"

            var availableRooms = db.tbl_room
                .Where(room => room.tbl_booking_status.booking_status_id != bookedStatusId)
                .ToList();

            // Query the rooms created in Tbl_roomController
            var createdRooms = db.tbl_room.ToList();

            ViewBag.AvailableRooms = availableRooms;
            ViewBag.CreatedRooms = createdRooms;

            // Populate the ViewBag for payment types
            var paymentTypes = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            ViewBag.payment_type = paymentTypes;

            return View();
        }

        [HttpGet]
        public ActionResult BookRoomOnline()
        {
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookRoomOnline(tbl_booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validate Anti-Forgery Token
                    if (!ModelState.IsValidField("__RequestVerificationToken"))
                    {
                        ModelState.AddModelError("", "Anti-forgery token validation failed.");
                        // Re-populate the ViewBag properties and return the view
                        ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
                        ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
                        return View(booking);
                    }

                    // Convert the booking dates to local time (if necessary)
                    booking.booking_from = booking.booking_from.ToLocalTime();
                    booking.booking_to = booking.booking_to.ToLocalTime();

                    bool isRoomAvailable = IsRoomAvailable(booking.assigned_room, booking.booking_from, booking.booking_to);
                    if (!isRoomAvailable)
                    {
                        ModelState.AddModelError("", "The selected room is not available for booking.");
                        // Re-populate the ViewBag properties and return the view
                        ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
                        ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
                        return View(booking);
                    }

                    int numberOfDays = (int)(booking.booking_to - booking.booking_from).TotalDays;
                    tbl_room_type roomType = db.tbl_room_type.FirstOrDefault(rt => rt.room_type_id == booking.assigned_room);
                    decimal roomPrice = (decimal)(roomType != null ? roomType.room_price : 0);
                    booking.total_amount = roomPrice * numberOfDays;

                    db.tbl_booking.Add(booking);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Room booked successfully!"; // Set the success message in TempData
                    return RedirectToAction("BookRoomOnline");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationError in ex.EntityValidationErrors)
                    {
                        foreach (var error in validationError.ValidationErrors)
                        {
                            // Log or display error message
                            Console.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                        }
                    }
                    throw; // Re-throw the exception after handling
                }
            }

            // If validation fails, redisplay the form with validation errors
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
            return View(booking);
        }


        public ActionResult CreatedRooms()
        {
            var rooms = db.tbl_room.Include(r => r.tbl_room_type).ToList();

            var roomAvailability = new Dictionary<int, bool>();
            foreach (var room in rooms)
            {
                roomAvailability.Add(room.room_id, IsRoomAvailable(room.room_id, DateTime.Now, DateTime.Now.AddDays(1))); // Modify the dates as needed
            }

            ViewBag.RoomAvailability = roomAvailability;

            var conflictingBookings = new Dictionary<int, List<tbl_booking>>();
            foreach (var room in rooms)
            {
                conflictingBookings.Add(room.room_id, GetConflictingBookings(room.room_id));
            }

            ViewBag.ConflictingBookings = conflictingBookings;

            return View(rooms);
        }



        private bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        {
            var conflictingBookings = db.tbl_booking
                .Where(b =>
                    b.assigned_room == roomId &&
                    ((startDate >= b.booking_from && startDate <= b.booking_to) || // Check start date overlap
                    (endDate >= b.booking_from && endDate <= b.booking_to) ||      // Check end date overlap
                    (startDate <= b.booking_from && endDate >= b.booking_to)))     // Check complete overlap
                .ToList();

            return conflictingBookings.Count == 0;
        }




        private List<tbl_booking> GetConflictingBookings(int roomId)
        {
            var currentDate = DateTime.Now;
            var conflictingBookings = db.tbl_booking
                .Where(b =>
                    b.assigned_room == roomId &&
                    currentDate >= b.booking_from && currentDate <= b.booking_to)
                .ToList();

            return conflictingBookings;
        }











        //private bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        //{
        //    var conflictingBookings = db.tbl_booking.Where(b =>
        //        b.assigned_room == roomId &&
        //        (startDate >= b.booking_from && startDate <= b.booking_to) || // Check start date overlap
        //        (endDate >= b.booking_from && endDate <= b.booking_to) ||     // Check end date overlap
        //        (startDate <= b.booking_from && endDate >= b.booking_to)      // Check complete overlap
        //    ).ToList();

        //    return conflictingBookings.Count == 0;
        //}







        public ActionResult About()
        {
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            return View();
        }
        [HttpPost]
        public ActionResult About([Bind(Include = "booking_id,customer_name,customer_address,customer_email,customer_phone_no,booking_from,booking_to,payment_type,assigned_room,no_of_members,total_amount")] tbl_booking tbl_booking)
        {
            int numberofday = Convert.ToInt32((tbl_booking.booking_to - tbl_booking.booking_from).TotalDays);

            tbl_room_type objroom = db.tbl_room_type.Single(model => model.room_type_id == tbl_booking.assigned_room);
            decimal Roomprices = Convert.ToDecimal(objroom.room_price);
            decimal TotalAmount = Roomprices * numberofday;
            tbl_booking roombooking = new tbl_booking()
            {
                customer_name = tbl_booking.customer_name,
                customer_address = tbl_booking.customer_address,
                customer_email = tbl_booking.customer_email,
                customer_phone_no = tbl_booking.customer_phone_no,
                booking_from = tbl_booking.booking_from,
                booking_to = tbl_booking.booking_to,
                payment_type = tbl_booking.payment_type,
                assigned_room = tbl_booking.assigned_room,
                no_of_members = tbl_booking.no_of_members,
                total_amount = TotalAmount,
            };


            if (ModelState.IsValid)
            {
                db.tbl_booking.Add(roombooking);
                db.SaveChanges();
                return RedirectToAction("About");
            }

            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number", tbl_booking.assigned_room);
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type", tbl_booking.payment_type);
            return View(tbl_booking);
        }

        public ActionResult Contact()
        {
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number");
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type");
            return View();
        }
        [HttpPost]
        public ActionResult Contact([Bind(Include = "booking_id,customer_name,customer_address,customer_email,customer_phone_no,booking_from,booking_to,payment_type,assigned_room,no_of_members,total_amount")] tbl_booking tbl_booking)
        {
            int numberofday = Convert.ToInt32((tbl_booking.booking_to - tbl_booking.booking_from).TotalDays);

            tbl_room_type objroom = db.tbl_room_type.Single(model => model.room_type_id == tbl_booking.assigned_room);
            decimal Roomprices = Convert.ToDecimal(objroom.room_price);
            decimal TotalAmount = Roomprices * numberofday;
            tbl_booking roombooking = new tbl_booking()
            {
                customer_name = tbl_booking.customer_name,
                customer_address = tbl_booking.customer_address,
                customer_email = tbl_booking.customer_email,
                customer_phone_no = tbl_booking.customer_phone_no,
                booking_from = tbl_booking.booking_from,
                booking_to = tbl_booking.booking_to,
                payment_type = tbl_booking.payment_type,
                assigned_room = tbl_booking.assigned_room,
                no_of_members = tbl_booking.no_of_members,
                total_amount = TotalAmount,
            };


            if (ModelState.IsValid)
            {
                db.tbl_booking.Add(roombooking);
                db.SaveChanges();
                return RedirectToAction("Contact");
            }
            ViewBag.Message = "Success Deliver";
            ViewBag.assigned_room = new SelectList(db.tbl_room, "room_id", "room_number", tbl_booking.assigned_room);
            ViewBag.payment_type = new SelectList(db.tbl_payment_type, "payment_type_id", "payment_type", tbl_booking.payment_type);
            return View(tbl_booking);
        }
    }
}