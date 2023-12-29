using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagmentSystem.Factory.AbstractFactory;
using HotelManagmentSystem.Models.DB;

namespace HotelManagmentSystem.Controllers
{
    public class Tbl_roomController : Controller
    {
        private HMSDBContext db = new HMSDBContext();
        private int roomId = 0;

        // GET: tbl_room
        public ActionResult Index()
        {
            var tbl_room = db.tbl_room.Include(t => t.tbl_booking_status).Include(t => t.tbl_room_type);
            return View(tbl_room.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbl_room tbl_room = db.tbl_room.Find(id);
            if (tbl_room == null)
            {
                return HttpNotFound();
            }

            // Check if the Room_Image property is empty or null, and if so, use the placeholder image URL
            string imageUrl = string.IsNullOrEmpty(tbl_room.Room_Image) ? "/Content/Images/room.jpg" : tbl_room.Room_Image;

            ViewBag.RoomImage = imageUrl;
            return View(tbl_room);
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.booking_status_id = new SelectList(db.tbl_booking_status, "booking_status_id", "booking_status");
            ViewBag.room_type_id = new SelectList(db.tbl_room_type, "room_type_id", "room_name");
            ViewBag.RoomImage = null; // Set the initial image URL to null
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_room room, HttpPostedFileBase RoomImageFile)
        {
            if (!db.tbl_booking_status.Any(bs => bs.booking_status_id == room.booking_status_id))
            {
                ModelState.AddModelError("booking_status_id", "Invalid booking status selected.");
            }

            // Check if the room with the same number already exists in the database
            if (db.tbl_room.Any(r => r.room_number == room.room_number))
            {
                ModelState.AddModelError("room_number", "A room with the same number already exists.");
            }

            if (ModelState.IsValid)
            {
                if (RoomImageFile != null && RoomImageFile.ContentLength > 0)
                {
                    if (!RoomImageFile.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("RoomImageFile", "Please upload a valid image file.");
                        // Handle the error accordingly...
                    }
                    else
                    {
                        string fileName = Path.GetFileName(RoomImageFile.FileName);
                        string extension = Path.GetExtension(fileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + extension;
                        room.Room_Image = "/Content/Images/" + uniqueFileName;
                        string path = Path.Combine(Server.MapPath("~/Content/Images/"), uniqueFileName);
                        RoomImageFile.SaveAs(path);
                    }
                }
                else
                {
                    room.Room_Image = "/Content/Images/default.jpg"; // Set a default image path if no image is provided
                }

                db.tbl_room.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If the model state is invalid, pass null for the image URL to the view
            ViewBag.RoomImage = null;
            ViewBag.booking_status_id = new SelectList(db.tbl_booking_status, "booking_status_id", "booking_status");
            ViewBag.room_type_id = new SelectList(db.tbl_room_type, "room_type_id", "room_name");
            return View(room);
        }

        private dynamic GetRoomImageURL(int room_id)
        {
            var room = db.tbl_room.FirstOrDefault(r => r.room_id == room_id); // Correct the parameter name

            if (room != null)
            {
                // If the room exists, fetch the image URL from the "Room_Image" property of the room
                string imageUrl = room.Room_Image;

                // If the image URL is not empty or null, return it
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    return imageUrl;
                }
            }

            // If the room doesn't exist in the database or the image URL is not available, return null
            return null; // Return null instead of a placeholder image URL
        }


        public FileResult RoomImage(int id)
        {
            var imageUrl = GetRoomImageURL(id);

            if (imageUrl != null)
            {
                return File(imageUrl, "image/jpeg");
            }

            // If the room doesn't exist or the image URL is empty, return a default image
            string defaultImagePath = Server.MapPath("~/Content/Images/");
            return File(defaultImagePath, "image/jpeg");
        }




        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_room tbl_room = db.tbl_room.Find(id);
            if (tbl_room == null)
            {
                return HttpNotFound();
            }
            ViewBag.booking_status_id = new SelectList(db.tbl_booking_status, "booking_status_id", "booking_status", tbl_room.booking_status_id);
            ViewBag.room_type_id = new SelectList(db.tbl_room_type, "room_type_id", "room_name", tbl_room.room_type_id);

            // Set the correct URL for the room image
            ViewBag.RoomImage = tbl_room.Room_Image;

            return View(tbl_room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "room_id,room_number,room_type_id,booking_status_id,is_Active,Room_Image,RoomImageFile")] tbl_room tbl_room, HttpPostedFileBase RoomImageFile)
        {
            if (!db.tbl_booking_status.Any(bs => bs.booking_status_id == tbl_room.booking_status_id))
            {
                ModelState.AddModelError("booking_status_id", "Invalid booking status selected.");
            }

            if (ModelState.IsValid)
            {
                // Check if there's another room with the same room number
                if (db.tbl_room.Any(r => r.room_id != tbl_room.room_id && r.room_number == tbl_room.room_number))
                {
                    ModelState.AddModelError("room_number", "A room with the same number already exists.");
                }
                else
                {
                    if (RoomImageFile != null && RoomImageFile.ContentLength > 0)
                    {
                        if (!RoomImageFile.ContentType.StartsWith("image/"))
                        {
                            ModelState.AddModelError("RoomImageFile", "Please upload a valid image file.");
                        }
                        else
                        {
                            string fileName = Path.GetFileName(RoomImageFile.FileName);
                            string extension = Path.GetExtension(fileName);
                            string uniqueFileName = Guid.NewGuid().ToString() + extension;
                            tbl_room.Room_Image = "/Content/Images/" + uniqueFileName;
                            string path = Path.Combine(Server.MapPath("~/Content/Images/"), uniqueFileName);
                            RoomImageFile.SaveAs(path);
                        }
                    }

                    db.Entry(tbl_room).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.booking_status_id = new SelectList(db.tbl_booking_status, "booking_status_id", "booking_status", tbl_room.booking_status_id);
            ViewBag.room_type_id = new SelectList(db.tbl_room_type, "room_type_id", "room_name", tbl_room.room_type_id);

            // If the model state is invalid, provide the existing room image URL and pass it to the view
            ViewBag.RoomImage = tbl_room.Room_Image; // This will be null or the selected image URL
            return View(tbl_room);
        }



        // GET: tbl_room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_room tbl_room = db.tbl_room.Find(id);
            if (tbl_room == null)
            {
                return HttpNotFound();
            }
            return View(tbl_room);
        }

        // POST: tbl_room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_room tbl_room = db.tbl_room.Find(id);
            db.tbl_room.Remove(tbl_room);
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
