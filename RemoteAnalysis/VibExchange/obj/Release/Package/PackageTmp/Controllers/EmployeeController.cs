using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VibExchange.Models;
using WebMatrix.WebData;
using VibExchange.Models;
using System.IO;
using VibExchange.Filters;

namespace VibExchange.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeLogin(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembershipAttribute]
        public ActionResult EmployeeLogin(Login model, string returnUrl)
        {
            bool sts = ModelState.IsValid;
            DataTable dt = new DataTable();
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        dt = context.getData("select * from Emp_Detail where LoginID = '" + model.UserName + "' and Password='" + model.Password + "'", CommandType.Text);

                        if (dt.Rows.Count > 0)
                        {
                            WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
                            if (Convert.ToString(dt.Rows[0]["UserRole"].ToString()) == "Employee")
                            {
                                Session["UserRole"] = "Employee";
                                Session["Department"] = dt.Rows[0]["Department"].ToString();
                            }
                            if (Convert.ToString(dt.Rows[0]["UserRole"].ToString()) == "Consultant")
                            {
                                Session["UserRole"] = "Consultant";
                                Session["Department"] = null;

                            }
                            Session["UserName"] = model.UserName;
                            Session["ImagePath"] = Convert.ToString(dt.Rows[0]["Image"]);
                            Session["IsActiveSession"] = true;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        //[InitializeSimpleMembershipAttribute]
        public ActionResult EmployerRegister(string user)
        {
            RegisterModel emp = new RegisterModel();
            ViewBag.User = user;
            List<SelectListItem> specs = new List<SelectListItem>();
            List<SelectListItem> GraduationList = new List<SelectListItem>();
            List<SelectListItem> PostGraduationList = new List<SelectListItem>();
            List<SelectListItem> CertificationList = new List<SelectListItem>();
            List<SelectListItem> ExperienceList = new List<SelectListItem>();
            GraduationList.Add(new SelectListItem { Text = "B.Arch", Value = "B.Arch" });
            GraduationList.Add(new SelectListItem { Text = "BBA", Value = "BBA" });
            GraduationList.Add(new SelectListItem { Text = "BCA", Value = "BCA" });
            GraduationList.Add(new SelectListItem { Text = "B.A", Value = "B.A" });
            GraduationList.Add(new SelectListItem { Text = "B.Com", Value = "B.Com" });
            GraduationList.Add(new SelectListItem { Text = "B.Sc ", Value = "B.Sc" });
            GraduationList.Add(new SelectListItem { Text = "BE", Value = "BE" });
            GraduationList.Add(new SelectListItem { Text = "B.Tech", Value = "B.Tech" });
            GraduationList.Add(new SelectListItem { Text = "B.Pharma", Value = "B.Pharma" });
            GraduationList.Add(new SelectListItem { Text = "Other", Value = "Other" });
            ViewBag.gradList = GraduationList;

            PostGraduationList.Add(new SelectListItem { Text = "MA", Value = "MA" });
            PostGraduationList.Add(new SelectListItem { Text = "M.Com", Value = "M.Com" });
            PostGraduationList.Add(new SelectListItem { Text = "MS", Value = "MS" });
            PostGraduationList.Add(new SelectListItem { Text = "M.Tech", Value = "M.Tech" });
            PostGraduationList.Add(new SelectListItem { Text = "MCA", Value = "MCA" });
            PostGraduationList.Add(new SelectListItem { Text = "ME ", Value = "ME" });
            PostGraduationList.Add(new SelectListItem { Text = "Other ", Value = "Other" });
            ViewBag.postgradList = PostGraduationList;

            CertificationList.Add(new SelectListItem { Text = "CAT-Level-1", Value = "CAT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-2", Value = "CAT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-3", Value = "CAT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-4", Value = "CAT-Level-4" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-1", Value = "CAT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-2", Value = "ASNT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-3", Value = "ASNT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-4", Value = "ASNT-Level-4" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-1", Value = "BiNDT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-2", Value = "BiNDT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-3", Value = "BiNDT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-4", Value = "BiNDT-Level-4" });
            ViewBag.certList = CertificationList;

            ExperienceList.Add(new SelectListItem { Text = "1 Year", Value = "1 Year" });
            ExperienceList.Add(new SelectListItem { Text = "2 Year", Value = "2 Year" });
            ExperienceList.Add(new SelectListItem { Text = "3 Year", Value = "3 Year" });
            ExperienceList.Add(new SelectListItem { Text = "4 Year", Value = "4 Year" });
            ExperienceList.Add(new SelectListItem { Text = "5 Year", Value = "5 Year" });
            ExperienceList.Add(new SelectListItem { Text = "6 Year", Value = "6 Year" });
            ExperienceList.Add(new SelectListItem { Text = "7 Year", Value = "7 Year" });
            ExperienceList.Add(new SelectListItem { Text = "8 Year", Value = "8 Year" });
            ExperienceList.Add(new SelectListItem { Text = "9 Year", Value = "9 Year" });
            ExperienceList.Add(new SelectListItem { Text = "10 Year", Value = "10 Year" });
            ExperienceList.Add(new SelectListItem { Text = "11 Year", Value = "11 Year" });
            ExperienceList.Add(new SelectListItem { Text = "12 Year", Value = "12 Year" });
            ExperienceList.Add(new SelectListItem { Text = "13 Year", Value = "13 Year" });
            ExperienceList.Add(new SelectListItem { Text = "14 Year", Value = "14 Year" });
            ExperienceList.Add(new SelectListItem { Text = "15 Year", Value = "15 Year" });
            ExperienceList.Add(new SelectListItem { Text = "16 Year", Value = "16 Year" });
            ExperienceList.Add(new SelectListItem { Text = "17 Year", Value = "17 Year" });
            ExperienceList.Add(new SelectListItem { Text = "18 Year", Value = "18 Year" });
            ExperienceList.Add(new SelectListItem { Text = "19 Year", Value = "19 Year" });
            ExperienceList.Add(new SelectListItem { Text = "20 Year", Value = "20 Year" });
            ExperienceList.Add(new SelectListItem { Text = "20+ Year", Value = "20+ Year" });
            ViewBag.expList = ExperienceList;

            specs.Add(new SelectListItem { Text = "Vibration Analysis", Value = "Vibration Analysis" });
            specs.Add(new SelectListItem { Text = "Thermal Analysis", Value = "Thermal Analysis" });
            specs.Add(new SelectListItem { Text = "Leak Detection", Value = "Leak Detection" });
            specs.Add(new SelectListItem { Text = "Balancing", Value = "Balancing" });
            emp.specialization = specs;
            return View(emp);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EmployerRegister(RegisterModel model, FormCollection form, string allchecked)
        {
            bool sts = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBClass context = new DBClass())
                    {
                        int i = 0;
                        context.AddParameter("@Emp_Name", model.Name);
                        context.AddParameter("@DisplayName", "Vibration Expert");
                        context.AddParameter("@LoginID", model.UserName);
                        context.AddParameter("@Password", model.Password);
                        context.AddParameter("@UserRole", "Consultant");
                        context.AddParameter("@EmailId", model.Email);
                        context.AddParameter("@Phone", model.phoneno);
                        i = context.ExecuteNonQuery("spAddEmployee", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                            WebSecurity.Login(model.UserName, model.Password);

                            Session["UserRole"] = "Consultant";
                            Session["UserName"] = model.UserName;
                            Session["IsActiveSession"] = true;
                            if (model.Department == "")
                            {
                                Session["Department"] = null;
                            }
                            else { Session["Department"] = model.Department; }
                            Session["IsActiveSession"] = true;
                            Session["ImagePath"] = "";
                            Session["FullName"] = model.Name;
                            MailMessage msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            MailAddress from = new MailAddress("info@vibration-service.com");
                            StringBuilder sb = new StringBuilder();
                            msg.IsBodyHtml = true;
                            smtp.Host = "smtp.zoho.com";
                            smtp.Port = 587;
                            msg.To.Add(model.Email);
                            msg.From = from;
                            msg.Subject = "Registration success full on VibExchange ! ";
                            msg.Body += " <html>";
                            msg.Body += "<body>";
                            msg.Body += "<table>";
                            msg.Body += "<tr>";
                            msg.Body += "<td>Thank you for registering on VibExChange. Your User Name and Password is : </td>";
                            msg.Body += "</tr>";
                            msg.Body += "<tr>";
                            msg.Body += "<td>User Name : </td><td>" + model.UserName + "</td>";
                            msg.Body += "</tr>";
                            msg.Body += "<tr>";
                            msg.Body += "<td>Password : </td><td>" + model.Password + "</td>";
                            msg.Body += "</tr>";
                            msg.Body += "<tr>";
                            msg.Body += "<td>for more information </td><td><a href='www.vibration-service.com'> Click Here </a></td>";
                            msg.Body += "</tr>";
                            msg.Body += "</table>";
                            msg.Body += "</body>";
                            msg.Body += "</html>";
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new System.Net.NetworkCredential("info@vibration-service.com", "nishant78");
                            smtp.Send(msg);
                            msg.Dispose();
                            //TempData["Message"] = "Your registration has been succesfully done.Our executive will contact you within 24 hours to activate your account.";
                            return RedirectToAction("EditUserProfile", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Registration Unsuccessfull ! Please try again.");
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult UserNameExist(string UserName)
        {
            var user = Membership.GetUser(UserName);
            return Json(user == null);
        }

        [HttpGet]
        public ActionResult GetConsultant()
        {
            RegisterModel regmode = new RegisterModel();
            regmode.AllConsultant = AllConsultantList();
            List<SelectListItem> CertificationList = new List<SelectListItem>();
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-1", Value = "CAT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-2", Value = "CAT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-3", Value = "CAT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "CAT-Level-4", Value = "CAT-Level-4" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-1", Value = "CAT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-2", Value = "ASNT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-3", Value = "ASNT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "ASNT-Level-4", Value = "ASNT-Level-4" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-1", Value = "BiNDT-Level-1" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-2", Value = "BiNDT-Level-2" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-3", Value = "BiNDT-Level-3" });
            CertificationList.Add(new SelectListItem { Text = "BiNDT-Level-4", Value = "BiNDT-Level-4" });
            ViewBag.certificateList = CertificationList;
            return View(regmode);
        }
        public static List<RegisterModel> AllConsultantList()
        {
            List<RegisterModel> consultantList = new List<RegisterModel>();  // creating list of model.
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("getAllConsultant", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    string image = Convert.ToString(dr["Image"]);
                    string imagesource = "~/Images/" + image;
                    //string DisplayName = Convert.ToString(dr["DisplayName"]) + "_" + Convert.ToString(dr["EmpID"]);
                    consultantList.Add(new RegisterModel
                    {
                        Name = Convert.ToString(dr["Emp_Name"]),
                        specs = Convert.ToString(dr["Specialization"]),
                        City = Convert.ToString(dr["ECity"]),
                        Experience = Convert.ToString(dr["Experience"]),
                        companyName = Convert.ToString(dr["CompName"]),
                        EmpID = Convert.ToString(dr["EmpID"]),
                        imageSrc = image
                    });

                }
                return consultantList;
            }
        }
        public ActionResult GetConsContactDetail(RegisterModel reg, string ID)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select * from Emp_Detail where EmpID = '" + ID + "' and  UserRole='Consultant'", CommandType.Text);
                reg.Address = Convert.ToString(dt.Rows[0]["EAddress"]);
                reg.phoneno = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                reg.OfficeNo = Convert.ToString(dt.Rows[0]["OfficeContactNo"]);
                reg.Email = Convert.ToString(dt.Rows[0]["EmailId"]);
            }
            ViewBag.Label = "All Consultant Deatils ...";
            return View("_consContactDetail", reg);
        }

        public ActionResult showExpertData(RegisterModel expert)
        {
            expert.AllConsultant = AllConsultantList();
            return PartialView("_getExpertData", expert.AllConsultant);
        }
    }
}
