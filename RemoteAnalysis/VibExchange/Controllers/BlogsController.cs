using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VibExchange.Filters;
using VibExchange.Models;

namespace VibExchange.Controllers
{
    public class BlogsController : Controller
    {
        public ActionResult ShowBlogs(Blogs blog)
        {
            blog.blogsList = getAllBlogs();
            return View(blog.blogsList);
        }

        public List<Blogs> getAllBlogs()
        {
            List<Blogs> blogList = new List<Blogs>();
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("getAllBlogs", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    //string image = Convert.ToString(dr["Image"]);
                    //string imagesource = "~/Images/" + image;
                    blogList.Add(new Blogs
                    {
                        blogID=Convert.ToInt32(dr["id"]),
                        blogTitle = Convert.ToString(dr["BlogTitle"]),
                        blogText = Convert.ToString(dr["BlogText"]),
                        blogImage = Convert.ToString(dr["BlogImagePath"]),
                        CreateDate = Convert.ToString(dr["CreateDate"]),
                        likes = Convert.ToInt32(dr["Likes"])
                    });

                }
                return blogList;
            }
        }

        [HttpGet]
        public ActionResult addBlogs()
        {
            if (Session["UserName"] == null)
            {
                return Content("<script language='javascript' type='text/javascript'>$('#myModal').modal({ keyboard: false })</script>");
                // return PartialView("Login");
            }
            else
            {
                return View("_writeBlog");
            }
        }

        [HttpPost]
        [InitializeSimpleMembershipAttribute]
        public ActionResult addBlogs(FormCollection form)
        {
            try
            {
                var file = HttpContext.Request.Files["blogImage"];
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Image");

                }
                else if (file.ContentLength > 0)
                {

                    double MaxContentLength = Convert.ToDouble(1024 * 1024 * .5); //1 MB
                    string[] AllowedFileExtensions = new string[] { ".jpeg", ".gif", ".bmp", ".png", ".jpg", ".GIF", ".BMP", ".PNG" };
                    string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        return View("ShowBlogs", ModelState);

                    }
                    if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " KB. Please zip your image and upload agail.");

                    }
                    else
                    {
                        using (DBClass context = new DBClass())
                        {
                            DataTable blogdt = context.getData("Select count(id) as id from BlogsData", CommandType.Text);
                            var fileName = Path.GetFileName(file.FileName);
                            int id;
                            if (Convert.ToInt32(blogdt.Rows[0]["id"]) != 0)
                            {
                                id = Convert.ToInt32(blogdt.Rows[0]["id"]) + 1;
                            }
                            else
                            {
                                id = 1;
                            }
                            string fname = Session["UserName"].ToString() + Convert.ToString(id) + "@" + fileName;
                            string filepath = "../Content/BlogsImages/" + fname;
                            var path = Path.Combine(Server.MapPath("../Content/BlogsImages"), fname);
                            context.AddParameter("@UserName", Session["UserName"].ToString());
                            context.AddParameter("@blogTitle", form["blogTitle"]);
                            context.AddParameter("@blogText", form["blogText"]);
                            context.AddParameter("@blogImagePath", filepath);
                            if (context.ExecuteNonQuery("addNewBlogs", CommandType.StoredProcedure) > 0)
                            {
                                file.SaveAs(path);
                                //bool chk = sendMail(form["blogTitle"], filepath, form["blogText"]);
                                ModelState.Clear();
                                ViewBag.Message = "Blog submit successfully";
                                return RedirectToAction("ShowBlogs", "Blogs");
                            }
                            else
                            {
                                ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");

                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return View("ShowBlogs", ModelState);
        }

        public ActionResult updateLikes(string blogID)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select * from BlogsData where id='" + blogID + "'", CommandType.Text);
                int likes = Convert.ToInt32(dt.Rows[0]["Likes"]) +1;
                context.AddParameter("@Likes",likes );
                context.AddParameter("@blogID",blogID);
                context.ExecuteNonQuery("updateBlog", CommandType.StoredProcedure);
            }
            return RedirectToAction("ShowBlogs");
        }


        public bool sendMail(string title , string imagepath , string text)
        {
            bool status = false;
            try
            {
                using (DBClass context = new DBClass())
                {

                    DataTable dt = context.getData("Select * from blogsEmailID", CommandType.Text);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string EmailID = Convert.ToString(dr["EmailID"]);
                        MailMessage msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        MailAddress from = new MailAddress("info@vibration-service.com");
                        StringBuilder sb = new StringBuilder();
                        msg.IsBodyHtml = true;
                        smtp.Host = "smtp.zoho.com";
                        smtp.Port = 587;
                        msg.To.Add(EmailID);
                        msg.From = from;
                        msg.Subject = "Daily News on VibExchange : " + title;
                        msg.Body += " <html>";
                        msg.Body += "<body>";
                        msg.Body += "<table>";
                        msg.Body += "<tr>";
                        msg.Body += "<td>" + title + "</td>";
                        msg.Body += "</tr>";
                        msg.Body += "<tr>";
                        msg.Body += "<td><img src='" + imagepath + "' class='img-thumbnail'/> </td>"; 
                        msg.Body += "</tr>";
                        msg.Body += "<tr>";
                        msg.Body += "<td>" + text + "</td>";
                        msg.Body += "</tr>";
                        msg.Body += "<tr>";
                        msg.Body += "<td>More Info. </td>";
                        msg.Body += "</tr>";
                        msg.Body += "<tr>";
                        msg.Body += "<td><a href='www.vibration-service.com'> Click Here </a></td>";
                        msg.Body += "</tr>";
                        msg.Body += "</table>";
                        msg.Body += "</body>";
                        msg.Body += "</html>";
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential("info@vibration-service.com", "nishant78");
                        smtp.Send(msg);
                        msg.Dispose();
                    }
                    status = true;
                }
            }
            catch
            {
                status = false;
            }
            return status;
        }

    }
}
