using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using HotelManagmentSystem.Models;
using HotelManagmentSystem.Models.DB;

namespace HotelManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
       
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult sendmail()
        {
           
            
            return View();
        }
        [HttpPost]
        public ActionResult sendmail(HotelManagmentSystem.Models.MailModel _objModelMail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(_objModelMail.To);
                    //mail.From = new MailAddress(_objModelMail.From);
                    mail.From = new MailAddress("hotelservcies@gmail.com", "hotel");
                    mail.Subject = _objModelMail.Subject;
                    string Body = _objModelMail.Body;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;

                    smtp.Credentials = new NetworkCredential("hotelservcies@gmail.com", "hotelservcies007");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return View("sendmail");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                return View("sendmail");
            }
          
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_user objuser)
        {
            HMSDBContext obj = new HMSDBContext();
            
            var user = obj.tbl_user.Where(x => x.email == objuser.email && x.user_password == objuser.user_password).FirstOrDefault();
            ViewBag.u = user;
            try
            {
            ViewBag.email = user.email;
            ViewBag.password = user.user_password;
                Session["Name"] = user.username;
            }
            catch (Exception)
            {

                
            }
            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

       
    }
}
