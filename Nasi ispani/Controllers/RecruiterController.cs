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
    public class RecruiterController : Controller
    {
        private IspaniDBEntities db = new IspaniDBEntities();

        // GET: Recruiter
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RecruiterLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecruiterLogin(Recruiter login, string ReturnUrl = "")
        {
            string message = "";
            
            using (IspaniDBEntities dc = new IspaniDBEntities())
            {
                var v = dc.Recruiters.Where(a => a.Id == login.Id).FirstOrDefault();
                if (v != null)
                {
                    Session["Id"] = v.Id;
                    this.Session["Name"] = v.Name;
                    Session["EmailAddress"] = v.EmailAddress;
                    Session["UserName"] = v.UserName;
                    Session["Password"] = v.Password;
                  

                    if (v.UserName == login.UserName && v.Password == login.Password)
                    {
                        return RedirectToAction("Index", "Recruiter");
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
        // Adding a job
        //[HttpGet]
      
        public ActionResult AddJob(int id = 0)
        {
            Job jobs = new Job();

            int count = 0;
            foreach (var item in db.Jobs.ToList())
            {
                int idn = 0;
                count = db.Jobs.Where(x => x.Id >= idn).Count();
                var selct1 = db.Jobs.OrderByDescending(x => x.Id).FirstOrDefault().Id;

            }


            ViewBag.Id = +count;
            return View();
        }


        [HttpPost]
        public ActionResult AddJob(Job job)
        {

            int count = 0;
            int idn  = 0;
            using (IspaniDBEntities db = new IspaniDBEntities())
            {

                if (db.Jobs.Any(x => x.Title == job.Title && x.CompanyName == job.CompanyName))
                {
                    ViewBag.DublicateMessage = "Job already exists";
                
                }
                
                else
                {
                    try
                    {


                        using (IspaniDBEntities dd = new IspaniDBEntities())
                        {

                            dd.Jobs.Add(job);
                            dd.SaveChanges();
                            ModelState.Clear();
                            ViewBag.SuccessMessage = "Job added Successfully.";
                        }
                    }
                    catch (Exception c)
                    {

                        Console.WriteLine("validate your infor", c);
                    }


                }

            }

            return View("AddJob", new Job());
        }
        // View Jobs
        public ActionResult ViewJobs(String searching)
        {

            var jobs = from s in db.Jobs
                           select s;

            return View(jobs.ToList());
        }
    }
    }

    
  
