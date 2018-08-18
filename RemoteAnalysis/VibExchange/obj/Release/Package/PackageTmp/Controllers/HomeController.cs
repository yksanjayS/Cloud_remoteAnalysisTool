using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Web.Security;
using VibExchange.Filters;
using VibExchange.Models;
using WebMatrix.Data;
using WebMatrix.WebData;
using System.Text;
using System.IO;


namespace VibExchange.Controllers
{
    /// <summary>
    /// This is change to GitHub.
    /// </summary>
    public class HomeController : Controller
    {
        //DataTable dt = new DataTable();

        [InitializeSimpleMembershipAttribute]
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome in VibExchange. ";
            return View();
        }
        //public ActionResult Index1()
        //{
        //    ViewBag.Message = "Welcome in VibExchange. ";
        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            //var ret = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
            ViewBag.ReturnUrl = ReturnUrl;
            //if (ReturnUrl != null)
            //{
            //    ViewBag.ReturnUrl = ReturnUrl;
            //}
            //if (ReturnUrl == null)
            //{
            //    ViewBag.ReturnUrl = TempData["returnURL"];
            //}
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembershipAttribute]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Username", model.UserName);
                        context.AddParameter("@Password", model.Password);
                        DataTable dtLogin = context.getData("CheckUser", CommandType.StoredProcedure);
                        if (dtLogin.Rows.Count > 0)
                        {
                            WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
                            Session["UserRole"] = Convert.ToString(dtLogin.Rows[0]["UserRole"]);
                            Session["UserName"] = Convert.ToString(dtLogin.Rows[0]["LoginId"]);
                            if (Convert.ToString(dtLogin.Rows[0]["UserRole"]) == "Employee")
                            {
                                Session["Department"] = Convert.ToString(dtLogin.Rows[0]["Department"]);
                            }
                            else { Session["Department"] = null; }
                            Session["IsActiveSession"] = true;
                            Session["ImagePath"] = dtLogin.Rows[0]["Image"].ToString();
                            Session["FullName"] = Convert.ToString(dtLogin.Rows[0]["Emp_Name"]);
                            return RedirectToLocal(ReturnUrl);
                            //return RedirectToLocal(ViewData["baseUrl"].ToString());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembership]
        public ActionResult LogOff()
        {
            if (Session["UserName"] != null)
            {
                Session.Abandon();
            }
            WebSecurity.Logout();
            return RedirectToAction("Home", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Home", "Home", ViewBag.UserRole);
            }
        }

        List<SelectListItem> City = new List<SelectListItem>();
        List<SelectListItem> State = new List<SelectListItem>();
        List<SelectListItem> Country = new List<SelectListItem>();
        DataTable dtList = new DataTable();

        [HttpGet]
        //[InitializeSimpleMembershipAttribute]
        public ActionResult UserRegister()
        {
            this.ModelState.Clear();
            ViewBag.CountryList = GetCountry();
            ViewBag.StateList = State;
            ViewBag.CityList = City;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegister(UserRegisterViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = model.UserName;
                    bool sts = Convert.ToBoolean(form["termsCondition"]);
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@UserName", model.UserName);
                        context.AddParameter("@Password", model.Password);
                        context.AddParameter("@Name", model.uName);
                        context.AddParameter("@EmailID", model.Email);
                        context.AddParameter("@CompanyName", model.uCompany);
                        context.AddParameter("@ContactNo", model.uMobile);
                        context.AddParameter("@UserRole", "Client");
                        if (context.ExecuteNonQuery("spAddUser", CommandType.StoredProcedure) == 1)
                        {
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                            WebSecurity.Login(model.UserName, model.Password);
                            Session["UserName"] = model.UserName;
                            Session["UserRole"] = "Client";
                            Session["ImagePath"] = "";
                            Session["FullName"] = model.uName;
                            Session["IsActiveSession"] = true;
                          DataTable dtRegister = context.getData("Select * from UserDetail where UserName ='" + model.UserName + "' and uPassword ='" + model.Password + "'", CommandType.Text);
                          //context.ExecuteNonQuery("Insert into webpages_UsersInRoles(UserId ,RoleId) values('" + Convert.ToInt32(dtRegister.Rows[0]["AutoId"]) + "',1)", CommandType.Text);
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

                            return RedirectToAction("EditUserProfile", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                        }
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult UserNameExist(string UserName)
        {
            var user = Membership.GetUser(UserName);
            return Json(user == null);
        }

        [HttpPost]
        public JsonResult EmailIDExist(string Email)
        {
            using (DBClass context = new DBClass())
            {
                bool chk = false;
                if (Session["UserName"] != null)
                {
                    DataTable emailid = context.getData("Select UserName , uPassword, Email_ID from UserDetail where Email_ID = '" + Email + "' union all Select LoginId , Password, EmailId from Emp_Detail where EmailId = '" + Email + "'", CommandType.Text);

                    if (emailid.Rows.Count == 0)
                    {
                        chk = true;
                    }

                    else if (Convert.ToString(emailid.Rows[0]["Email_ID"]) == Email)
                    {
                        chk = true;
                    }
                    else
                    {
                        DataTable mailid = context.getData("Select Email_Id from UserDetail where UserName = '" + Session["UserName"].ToString() + "' union all Select EmailID from Emp_Detail where LoginID ='" + Session["UserName"].ToString() + "'", CommandType.Text);
                        if (mailid.Rows.Count > 0)
                        {
                            if (Convert.ToString(mailid.Rows[0]["Email_ID"]) == Email)
                            {
                                chk = true;
                            }
                        }
                    }
                    return Json(chk);
                }
                else
                {
                    var emailid = context.getData("Select UserName , uPassword, Email_ID from UserDetail where Email_ID = '" + Email + "' union all Select LoginId , Password, EmailId from Emp_Detail where EmailId = '" + Email + "'", CommandType.Text);
                    if (emailid.Rows.Count == 0)
                    {
                        chk = true;
                    }
                    return Json(chk);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [InitializeSimpleMembershipAttribute]
        public ActionResult EditUserProfile(string userName)
        {
            EditUserProfile edit = new EditUserProfile();
            edit.specialization = "";
            try
            {
                List<SelectListItem> specs = new List<SelectListItem>();
                List<SelectListItem> GraduationList = new List<SelectListItem>();
                List<SelectListItem> PostGraduationList = new List<SelectListItem>();
                List<SelectListItem> CertificationList = new List<SelectListItem>();
                List<SelectListItem> ExperienceList = new List<SelectListItem>();
                List<SelectListItem> DepartmentList = new List<SelectListItem>();
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
                PostGraduationList.Add(new SelectListItem { Text = "Other", Value = "Other" });
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
                CertificationList.Add(new SelectListItem { Text = "Other", Value = "Other" });
                ViewBag.certList = CertificationList;

                ExperienceList.Add(new SelectListItem { Text = "0-1 Year", Value = "0 Year" });
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
                ViewBag.SpecList = specs;

                DepartmentList.Add(new SelectListItem { Text = "Sales & Marketing", Value = "Sales & Marketing" });
                DepartmentList.Add(new SelectListItem { Text = "Account", Value = "Account" });
                DepartmentList.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
                DepartmentList.Add(new SelectListItem { Text = "HR", Value = "HR" });
                DepartmentList.Add(new SelectListItem { Text = "IT", Value = "IT" });
                ViewBag.departmentList = DepartmentList;

                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@UserName", Session["UserName"].ToString());
                    context.AddParameter("@UserRole", Session["UserRole"].ToString());
                    DataTable dt = context.getData("getUserDetail", CommandType.StoredProcedure);
                    if (dt.Rows.Count > 0)
                    {
                        if (Session["UserRole"].ToString() == "Client")
                        {
                            edit.fullName = Convert.ToString(dt.Rows[0]["uName"]);
                            edit.UserName = Convert.ToString(dt.Rows[0]["UserName"]);
                            edit.Email = Convert.ToString(dt.Rows[0]["Email_ID"]);
                            edit.Phone = Convert.ToString(dt.Rows[0]["uMobile_No"]);
                            edit.Address = Convert.ToString(dt.Rows[0]["Address"]);
                            edit.City = Convert.ToString(dt.Rows[0]["City"]);
                            edit.State = Convert.ToString(dt.Rows[0]["State"]);
                            edit.Country = Convert.ToString(dt.Rows[0]["Country"]);
                            edit.imageSrc = Convert.ToString(dt.Rows[0]["ImagePath"]);
                            edit.companyName = Convert.ToString(dt.Rows[0]["uCompanyName"]);
                            edit.compAddress = Convert.ToString(dt.Rows[0]["uCompanyAddress"]);
                            edit.cmpCity = Convert.ToString(dt.Rows[0]["uCity"]);
                            edit.cmpState = Convert.ToString(dt.Rows[0]["uState"]);
                            edit.cmpCountry = Convert.ToString(dt.Rows[0]["uCountry"]);
                            edit.OfficeNo = Convert.ToString(dt.Rows[0]["officePhone"]);
                            edit.cmpWebsite = Convert.ToString(dt.Rows[0]["uCompany_Website"]);
                        }
                        if (Session["UserRole"].ToString() == "Employee")
                        {
                            edit.fullName = Convert.ToString(dt.Rows[0]["Emp_Name"]);
                            edit.UserName = Convert.ToString(dt.Rows[0]["LoginID"]);
                            edit.Email = Convert.ToString(dt.Rows[0]["EmailId"]);
                            edit.Phone = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                            edit.Address = Convert.ToString(dt.Rows[0]["EAddress"]);
                            edit.City = Convert.ToString(dt.Rows[0]["ECity"]);
                            edit.State = Convert.ToString(dt.Rows[0]["EState"]);
                            edit.Country = Convert.ToString(dt.Rows[0]["ECountry"]);
                            edit.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                            edit.Department = Convert.ToString(dt.Rows[0]["Department"]);
                            edit.imageSrc = Convert.ToString(dt.Rows[0]["Image"]);
                        }
                        if (Session["UserRole"].ToString() == "Consultant")
                        {
                            edit.fullName = Convert.ToString(dt.Rows[0]["Emp_Name"]);
                            edit.UserName = Convert.ToString(dt.Rows[0]["LoginID"]);
                            edit.Email = Convert.ToString(dt.Rows[0]["EmailId"]);
                            edit.Phone = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                            edit.Address = Convert.ToString(dt.Rows[0]["EAddress"]);
                            edit.City = Convert.ToString(dt.Rows[0]["ECity"]);
                            edit.State = Convert.ToString(dt.Rows[0]["EState"]);
                            edit.Country = Convert.ToString(dt.Rows[0]["ECountry"]);
                            edit.imageSrc = Convert.ToString(dt.Rows[0]["Image"]);
                            edit.companyName = Convert.ToString(dt.Rows[0]["CompName"]);
                            edit.graduation = Convert.ToString(dt.Rows[0]["Graduation"]);
                            edit.postgraduation = Convert.ToString(dt.Rows[0]["Postgraduation"]);
                            edit.certification = Convert.ToString(dt.Rows[0]["Certification"]);
                            edit.Experience = Convert.ToString(dt.Rows[0]["Experience"]);



                            edit.specialization = Convert.ToString(dt.Rows[0]["Specialization"]);
                            edit.compAddress = Convert.ToString(dt.Rows[0]["CAddress"]);
                            edit.cmpCity = Convert.ToString(dt.Rows[0]["CCity"]);
                            edit.cmpState = Convert.ToString(dt.Rows[0]["CState"]);
                            edit.cmpCountry = Convert.ToString(dt.Rows[0]["CCountry"]);
                            edit.OfficeNo = Convert.ToString(dt.Rows[0]["OfficeContactNo"]);
                            edit.cmpWebsite = Convert.ToString(dt.Rows[0]["CompWebsite"]);
                        }
                    }
                }
            }
            catch { }
            return View(edit);
        }

        [HttpPost]
        [InitializeSimpleMembershipAttribute]
        public ActionResult EditUserProfile(EditUserProfile edituser, string allchecked, FormCollection formCollection)
        {
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@UserName", Session["UserName"].ToString());
                    context.AddParameter("@UserRole", Session["UserRole"].ToString());
                    context.AddParameter("@FullName", edituser.fullName);
                    context.AddParameter("@Phone", edituser.Phone);
                    context.AddParameter("@EmailID", edituser.Email);
                    context.AddParameter("@Address", edituser.Address);
                    context.AddParameter("@City", edituser.City);
                    context.AddParameter("@State", edituser.State);
                    context.AddParameter("@Country", edituser.Country);
                    context.AddParameter("@CompanyName", edituser.companyName);
                    context.AddParameter("@compAddress", edituser.compAddress);
                    context.AddParameter("@compCity", edituser.cmpCity);
                    context.AddParameter("@compState", edituser.cmpState);
                    context.AddParameter("@compCountry", edituser.cmpCountry);
                    context.AddParameter("@OfficeNo", edituser.OfficeNo);
                    context.AddParameter("@compWebsite", edituser.cmpWebsite);
                    context.AddParameter("@designation", edituser.Designation);
                    context.AddParameter("@department", edituser.Department);
                    context.AddParameter("@Graduation", edituser.graduation);
                    context.AddParameter("@PostGraduation", edituser.postgraduation);
                    context.AddParameter("@Certificate", edituser.certification);
                    context.AddParameter("@Experience", edituser.Experience);
                    context.AddParameter("@specialization", formCollection["allchecked"]);
                    context.ExecuteNonQuery("updateUserProfile", CommandType.StoredProcedure);
                }
            }
            catch { }
            return RedirectToAction("Home", "Home");
            //return View("Index", "Home");
        }

        [HttpPost]
        [InitializeSimpleMembershipAttribute]
        public ActionResult ChangeProfileImage()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files[0];
            string FileName = Session["UserName"].ToString() + "@" + file.FileName;
            string extension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            if (file.FileName != null || file.FileName != "")
            {
                string fname = Path.Combine(Server.MapPath("../Content/User Image/"), FileName);
                string filepath = "../Content/User Image/" + FileName;
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@UserName", Session["UserName"]);
                    context.AddParameter("@UserRole", Session["UserRole"]);
                    context.AddParameter("@Filepath", filepath);
                    int i = context.ExecuteNonQuery("updateUserImage", CommandType.StoredProcedure);
                    if (i > 0)
                        file.SaveAs(fname);
                    Session["ImagePath"] = filepath;
                    //string imgpath = "../Content/User Image/" + FileName;
                    //ViewData["ImagePath"] = imgpath;
                    return Json(new { status = "Success", path = filepath });
                }
            }
            else
            {
                return Json(new { status = "Failed", Message = "You have not selected any image !" });
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword f)
        {
            using (DBClass context = new DBClass())
            {
                var emailid = context.getData("Select UserName , uPassword, Email_ID from UserDetail where Email_ID = '" + f.EmailID + "' union all Select Emp_Name , Password, EmailId from Emp_Detail where EmailId = '" + f.EmailID + "'", CommandType.Text);
                try
                {
                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    MailAddress from = new MailAddress("support@vibration-service.com");
                    StringBuilder sb = new StringBuilder();
                    msg.IsBodyHtml = true;
                    smtp.Host = "smtp.zoho.com";
                    smtp.Port = 587;
                    msg.To.Add(f.EmailID);
                    msg.From = from;
                    msg.Subject = "User Name and Password recovery for VibExchange !";
                    msg.Body += " <html>";
                    msg.Body += "<body>";
                    msg.Body += "<table>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Your User Name and Password for VibExchange is : </td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>User name:</td><td>" + emailid.Rows[0]["UserName"].ToString() + "</td>";
                    msg.Body += "<td></td>";
                    msg.Body += "</tr>";
                    msg.Body += "<tr>";
                    msg.Body += "<td>Password:</td><td>" + emailid.Rows[0]["uPassword"].ToString() + "</td>";
                    msg.Body += "<td></td>";
                    msg.Body += "</tr>";
                    //msg.Body += "<td>Click here to login on VibExchange !</td>";
                    msg.Body += "<tr>";
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
                catch
                {
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult EmailIDVerify(string EmailID)
        {
            using (DBClass context = new DBClass())
            {
                var emailid = context.getData("Select UserName , uPassword, Email_ID from UserDetail where Email_ID = '" + EmailID + "'union all Select LoginId , Password, EmailId from Emp_Detail where EmailId = '" + EmailID + "'", CommandType.Text);
                bool chk = false;
                if (emailid.Rows.Count > 0)
                {
                    chk = true;
                }
                return Json(chk);
            }
        }

        //
        // POST: /Home/ResetPassword
        [HttpPost]
        public JsonResult CurrentPasswordVerify(string CurrentPassword)
        {

            using (DBClass context = new DBClass())
            {
                var emailid = context.getData("Select UserName,uPassword from UserDetail where UserName ='" + Session["UserName"].ToString() + "' and uPassword ='" + CurrentPassword + "' union all Select LoginID , Password from Emp_Detail where LoginID = '" + Session["UserName"].ToString() + "' and Password ='" + CurrentPassword + "'", CommandType.Text);
                bool chk = false;
                if (emailid.Rows.Count > 0)
                {
                    chk = true;
                }
                return Json(chk);
            }
        }

        //
        // GET: /Home/ResetPassword
        [AllowAnonymous]
        [InitializeSimpleMembershipAttribute]
        public ActionResult ResetPassword(string code)
        {
            return View();
        }

        //
        // POST: /Home/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembershipAttribute]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@username", Session["UserName"].ToString());
                    DataTable dt = context.getData("getUserRole", CommandType.StoredProcedure);
                    if (Convert.ToString(dt.Rows[0]["UserRole"]) == "Employee" || Convert.ToString(dt.Rows[0]["UserRole"]) == "Admin")
                    {
                        context.ExecuteNonQuery("Update Emp_Detail set Password = '" + model.NewPassword + "' where LoginID = '" + Session["UserName"].ToString() + "'", CommandType.Text);
                    }
                    else
                    {
                        context.ExecuteNonQuery("Update UserDetail set uPassword = '" + model.NewPassword + "' where UserName = '" + Session["UserName"].ToString() + "'", CommandType.Text);
                    }
                    WebSecurity.ChangePassword(Session["UserName"].ToString(), model.CurrentPassword, model.NewPassword);
                    return View("Index");
                }
            }
            return View();
        }

        private SelectList GetCountry()
        {
            using (DBClass context = new DBClass())
            {
                try
                {
                    dtList.Clear();
                    dtList = context.getData("getCountry", CommandType.StoredProcedure);
                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {
                        Country.Add(new SelectListItem { Text = Convert.ToString(dtList.Rows[i]["CountryName"]), Value = Convert.ToString(dtList.Rows[i]["CountryID"]) });
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
                return new SelectList(Country, "Value", "Text", "id");
            }
        }

        public JsonResult getState(int countryid)
        {
            using (DBClass context = new DBClass())
            {
                try
                {
                    dtList.Clear();
                    context.AddParameter("@CountryID", countryid);
                    dtList = context.getData("getStateByCountry", CommandType.StoredProcedure);
                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {
                        State.Add(new SelectListItem { Text = Convert.ToString(dtList.Rows[i]["StateName"]), Value = Convert.ToString(dtList.Rows[i]["StateID"]) });
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
            return Json(State, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getCity(int stateid)
        {
            try
            {
                using (DBClass context = new DBClass())
                {
                    dtList.Clear();
                    context.AddParameter("@StateID", stateid);
                    dtList = context.getData("getCityByState", CommandType.Text);
                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {
                        City.Add(new SelectListItem { Text = Convert.ToString(dtList.Rows[i]["CityName"]), Value = Convert.ToString(dtList.Rows[i]["CityID"]) });
                    }

                }
            }
            catch { }
            return Json(City, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [InitializeSimpleMembershipAttribute]
        public ActionResult RemoteAnalysis()
        {

            return View("RAMHome");
        }

        #region
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
