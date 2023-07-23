using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nasi_ispani.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;


namespace Nasi_ispani.Controllers
{
    public class AccountController : Controller
    {
        private IspaniDBEntities db = new IspaniDBEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin login, string ReturnUrl = "")
        {
            string message = "";
            using (IspaniDBEntities dc = new IspaniDBEntities())
            {
                var v = dc.Admins.Where(a => a.AdminID == login.AdminID).FirstOrDefault();
                if (v != null)
                {
                    Session["AdminID"] = v.AdminID;
                    this.Session["Name"] = v.Name;
                    Session["Surname"] = v.Surname;
                    Session["IdNumber"] = v.IdNumber;
                    Session["Cellphone"] = v.CellPhone;
                    Session["Email"] = v.Email;

                    if (v.Email == login.Email && v.Password == login.Password)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //[HttpGet]
        public ActionResult AddApplication(int id = 0)
        {
            Application application = new Application();

            int count = 0;
            foreach (var item in db.Applications.ToList())
            {
                int idn = 0;
                count = db.Applications.Where(x => x.ApplicantId >= idn).Count();
                var selct1 = db.Applications.OrderByDescending(x => x.ApplicantId).FirstOrDefault().ApplicantId;
              
            }   

          
            ViewBag.ApplicantId =+ count;
            return View();
        }


        [HttpPost]
        public ActionResult AddApplication(Application application)
        {


            using (IspaniDBEntities db = new IspaniDBEntities())
            {

                if (db.Applications.Any(x => x.ApplicantName == application.ApplicantName && x.IdNumber == application.IdNumber))
                {
                    ViewBag.DublicateMessage = "Application already exists";
                    return View("AddApplication", application);
                }
                else
                {
                    try
                    {


                        using (IspaniDBEntities dd = new IspaniDBEntities())
                        {

                            dd.Applications.Add(application);
                            dd.SaveChanges();
                            ModelState.Clear();
                            ViewBag.SuccessMessage = "Thank you for applying "+ application.ApplicantName ;
                        }
                    }
                    catch (Exception c)
                    {

                        Console.WriteLine("validate your infor", c);
                    }


                }

            }

            return View("AddApplication", new Application());
        }
    }
}