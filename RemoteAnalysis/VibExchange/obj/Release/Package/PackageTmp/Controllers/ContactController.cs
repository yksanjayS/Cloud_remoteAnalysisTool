using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VibExchange.Models;
using System.Windows;
using System.IO;

namespace VibExchange.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Contacts()
        {
            Contact temp = new Contact();
            return View(temp);
        }

        [HttpPost]
        public ActionResult Contacts(Contact c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    MailAddress from = new MailAddress("info@vibration-service.com");
                    StringBuilder sb = new StringBuilder();
                    msg.IsBodyHtml = true;
                    smtp.Host = "smtp.zoho.com";
                    smtp.Port = 587;
                    msg.To.Add("support@vibration-service.com");
                    msg.From = from;
                    msg.Subject = c.Subject;
                    msg.Body += " <html>";
                    msg.Body += "<body>";
                    msg.Body += "<table>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>First Name : </td><td>"+c.Name+"</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Email ID : </td><td>" + c.Email + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Contact No. : </td><td>" + c.Phone + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Description : </td><td>" + c.Message + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "</table>";
                    msg.Body += "</body>";
                    msg.Body += "</html>";
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("info@vibration-service.com", "nishant78");
                    smtp.Send(msg);
                    msg.Dispose();
                    return RedirectToAction("Home", "Home");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }

        public ActionResult GetJobs()
        {
            CurrentOpenings opening = new CurrentOpenings();
            opening.AllOpenings = opening.GetAllOpenings();
            return View(opening.AllOpenings);
        }

        [HttpPost]
        public ActionResult GetJobs(FormCollection form, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var file = HttpContext.Request.Files["resume"];
                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    MailAddress from = new MailAddress("info@vibration-service.com");
                    StringBuilder sb = new StringBuilder();
                    msg.IsBodyHtml = true;
                    smtp.Host = "smtp.zoho.com";
                    smtp.Port = 587;
                    msg.To.Add("ruchika@iadeptmarketing.com");
                    msg.From = from;
                    msg.Subject = "Resume for " + form["position"];
                    msg.Body += " <html>";
                    msg.Body += "<body>";
                    msg.Body += "<table>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>First Name : </td><td>" + form["Fname"] + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Phone Number : </td><td>" + form["mobile"] + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Email Address : </td><td>" + form["email"] + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Work Experience : </td><td>" + form["experience"] + "</td>";
                    msg.Body += "</tr>";
                    msg.Body += "</table>";
                    msg.Body += "</body>";
                    msg.Body += "</html>";
                    msg.Attachments.Add(new Attachment(file.InputStream, file.FileName));
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("info@vibration-service.com", "nishant78");
                    smtp.Send(msg);
                    msg.Dispose();
                    return RedirectToAction("GetJobs", "Contact");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }
    }
}
