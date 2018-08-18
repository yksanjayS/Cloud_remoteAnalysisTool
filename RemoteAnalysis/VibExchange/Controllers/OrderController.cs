using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VibExchange.Filters;
using VibExchange.Models;
using WebMatrix.WebData;
using System.Web.Security;
using System.Net.Mail;
using System.Text;
namespace VibExchange.Controllers
{
      
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View("GetConsultant");
        }

        //[HttpGet]
        //public ActionResult getEnquiry()
        //{
        //    if (Session["UserName"] == null)
        //    {
        //        return Content("<script language='javascript' type='text/javascript'>$('#myModal').modal({ keyboard: false })</script>");
        //        // return PartialView("Login");
        //    }
        //    else
        //    {
        //        List<SelectListItem> CategoryList = new List<SelectListItem>();
        //        CategoryList.Add(new SelectListItem { Text = "Vibration Analysis", Value = "Vibration" });
        //        CategoryList.Add(new SelectListItem { Text = "Balancing", Value = "Balancing" });
        //        CategoryList.Add(new SelectListItem { Text = "Leak Detection", Value = "Sound" });
        //        CategoryList.Add(new SelectListItem { Text = "Thermal Analysis", Value = "Temprature" });
        //        ViewBag.categoryList = CategoryList;
        //        return PartialView("_EnquiryPartial");
        //    }
        //}

        [HttpGet]
        public ActionResult getEnquiry()
        {
            List<SelectListItem> CategoryList = new List<SelectListItem>();
            CategoryList.Add(new SelectListItem { Text = "Vibration Analysis", Value = "Vibration" });
            CategoryList.Add(new SelectListItem { Text = "Balancing", Value = "Balancing" });
            CategoryList.Add(new SelectListItem { Text = "Leak Detection", Value = "Sound" });
            CategoryList.Add(new SelectListItem { Text = "Thermal Analysis", Value = "Temprature" });
            ViewBag.categoryList = CategoryList;
            return PartialView("_EnquiryPartial");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult getEnquiry(FormCollection form,Order order)
        {
            string[] user = form["EmailID"].ToString().Split('@');
            string username = Convert.ToString(user[0]);
            using (DBClass context = new DBClass())
            {
                if (Session["UserName"] != null)
                {
                    context.AddParameter("@UserName", Session["UserName"].ToString());
                    context.AddParameter("@EnquirySubject", form["EnquirySubject"]);
                    context.AddParameter("@EnquiryCategory", form["CategoryList"]);
                    context.AddParameter("@EnquiryDetail", form["EnquiryDetail"]);
                    context.AddParameter("@CreateDate", context.GetDateTime());
                    if (context.ExecuteNonQuery("addEnquiry", CommandType.StoredProcedure) > 0)
                    {
                        return RedirectToAction("showEnquiry", "Order");
                    }
                }
                else
                {
                    context.AddParameter("@Name", form["fullName"]);
                    context.AddParameter("@UserName", username);
                    context.AddParameter("@Password", form["Password"].ToString());
                    context.AddParameter("@EmailID", form["emailID"]);
                    context.AddParameter("@Phone", form["phone"].ToString());
                    context.AddParameter("@EnquiryCategory", form["CategoryList"]);
                    context.AddParameter("@EnquiryDetail", form["EnquiryDetail"]);
                    context.AddParameter("@CreateDate", context.GetDateTime());
                    if (context.ExecuteNonQuery("addEnquiryfromHome", CommandType.StoredProcedure) > 0)
                    {
                        WebSecurity.CreateUserAndAccount(username, order.password);
                        //WebSecurity.CreateUserAndAccount(form["EmailID"].ToString(), form["Password"].ToString());
                        WebSecurity.Login(username, order.password);
                        Session["UserRole"] = "Client";
                        Session["UserName"] = username;
                        Session["Department"] = null;
                        Session["IsActiveSession"] = true;
                        Session["ImagePath"] = "";
                        Session["FullName"] = form["fullName"];
                        return RedirectToAction("EditUserProfile", "Home");
                    }
                }
            }
            return View();
        }

      
        public ActionResult showEnquiry()
        {
            return View();
        }

        public ActionResult showEnquiry1()
        {
            var Enq_List = Json(Enquiry.getAllEnquiry(Convert.ToString(Session["UserRole"])), JsonRequestBehavior.AllowGet);
            return Enq_List;
        }

        public ActionResult showEnquiryData(Enquiry enq)
        {
            enq.enquiryList = enq.getAllEnquiryData();
            return PartialView("_getEnquiryData", enq.enquiryList);
        }

        public ActionResult buyEnquiry(string Id)
        {
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@username", Session["UserName"].ToString());
                DataSet ds = context.ExecuteDataSet("getUserRole", CommandType.StoredProcedure);
                if (Convert.ToString(ds.Tables[0].Rows[0]["UserRole"]) != "Client")
                {
                    DataTable dt = new DataTable();
                    context.AddParameter("@UserName", Session["UserName"].ToString());
                    context.AddParameter("@EnquiryID", Convert.ToInt32(Id));
                     dt = context.getData("getExpertBalance", CommandType.StoredProcedure);
                    int CurrentBalance = Convert.ToInt32(dt.Rows[0]["Current_PointBalance"]);
                    int enquiryCost = Convert.ToInt32(dt.Rows[0]["Enquiry_Cost"]);
                    if (CurrentBalance >= enquiryCost)
                    {
                        context.AddParameter("@EnquiryID", Id);
                        context.AddParameter("@UserName", Session["UserName"].ToString());
                        context.AddParameter("@EnquiryCost", enquiryCost);
                        int i = context.ExecuteNonQuery("addEnquiryToBuyer", CommandType.StoredProcedure);
                        if (i > 0)
                        {

                            return Json(new { status = "Success", message = "Success" });
                        }
                        else { return Json(new { status = "Database", message = "Problem In Database. Please try again !" }); }
                    }
                    else
                    {
                        return Json(new { status = "Fail", message = "Fail" });
                    }

                }
                else { return Json(new { status = "Client", message = "You are not autherized to buy an enquiry !" }); }
            }

        }

        public ActionResult getEnquiryStatus(string Id)
        {
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@EnquiryID", Id);
                context.AddParameter("@UserName", Session["UserName"].ToString());
                DataTable dt = context.getData("getEnquiryStatus", CommandType.StoredProcedure);
                if (dt.Rows.Count > 0)
                {
                    return Json(new { sts = "True", message = "True" });
                }
                else
                {
                    return Json(new { sts = "False", message = "False" });
                }
            }

        }

        public ActionResult showEnquiryDetail(string EnquiryID)
        {
            Enquiry enquiry = new Enquiry();
            using(DBClass context = new DBClass())
            {
                context.AddParameter("@EnquiryID",EnquiryID);
                DataTable dt  = context.getData("getEnquiryDetailByID",CommandType.StoredProcedure);
                if (dt.Rows.Count > 0)
                {

                    enquiry.UserName = Convert.ToString(dt.Rows[0]["uName"]);
                    enquiry.company = Convert.ToString(dt.Rows[0]["uCompanyName"]);
                    enquiry.phone = Convert.ToString(dt.Rows[0]["uMobile_No"]);
                    enquiry.emailid = Convert.ToString(dt.Rows[0]["Email_ID"]);
                    enquiry.EnquiryID = Convert.ToString(dt.Rows[0]["ID"]);
                    enquiry.Enq_Category = Convert.ToString(dt.Rows[0]["EnquiryCategory"]);
                    enquiry.Enq_Subject = Convert.ToString(dt.Rows[0]["EnquirySubject"]);
                    enquiry.EnquiryDetail1 = Convert.ToString(dt.Rows[0]["EnquiryDetail"]);
                }
                else
                {
                    ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                }
            }
            return PartialView("_showEnquiryDetail", enquiry);
        }

        [HttpGet]
        public ActionResult getExpertQuery()
        {
            return PartialView("_getExpertQuery");
        }

        [HttpPost]
        public ActionResult getExpertQuery(Contact query, FormCollection form)
        {
            MailMessage msg = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            MailAddress from = new MailAddress("support@vibration-service.com");
            StringBuilder sb = new StringBuilder();
            msg.IsBodyHtml = true;
            smtp.Host = "smtp.zoho.com";
            smtp.Port = 587;
            msg.To.Add("info@vibration-service.com");
            msg.From = from;
            msg.Subject = "User's Query on VibExchange !";
            msg.Body += " <html>";
            msg.Body += "<body>";
            msg.Body += "<table class='table' >";

            msg.Body += "<tr>";
            msg.Body += "<td>User Name:</td><td>" + form["Name"].ToString() + "</td>";
            msg.Body += "<td></td>";
            msg.Body += "</tr>";

            msg.Body += "<tr>";
            msg.Body += "<td>Email Address:</td><td>" + form["Email"].ToString() + "</td>";
            msg.Body += "<td></td>";
            msg.Body += "</tr>";

            msg.Body += "<tr>";
            msg.Body += "<td>Phone Number:</td><td>" + form["Phone"].ToString() + "</td>";
            msg.Body += "<td></td>";
            msg.Body += "</tr>";

            msg.Body += "<tr>";
            msg.Body += "<td>Subject:</td><td>" + form["Subject"].ToString() + "</td>";
            msg.Body += "<td></td>";
            msg.Body += "</tr>";

            msg.Body += "<tr>";
            msg.Body += "<td>Description:</td><td>" + form["Message"].ToString() + "</td>";
            msg.Body += "<td></td>";
            msg.Body += "</tr>";

            msg.Body += "</table>";
            msg.Body += "</body>";
            msg.Body += "</html>";
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("support@vibration-service.com", "Pass@123");
            smtp.Send(msg);
            msg.Dispose();
            return RedirectToAction("Home", "Home");
        }
    }
}
