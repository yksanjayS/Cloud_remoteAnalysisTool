using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VibExchange.Controllers
{
    public class ReturnController : Controller
    {
        //
        // GET: /Return/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Return(FormCollection form)
        {
            try
            {
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                if (form["status"].ToString() == "success")
                {
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + form["status"].ToString();
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");
                    }
                    Response.Write(merc_hash_string);
                    merc_hash = Generatehash512(merc_hash_string).ToLower();
                    if (merc_hash != form["hash"])
                    {
                        Response.Write("Hash value did not matched");
                    }
                    else
                    {
                        order_id = Request.Form["txnid"];
                        return Content("<script language='javascript' type='text/javascript'>alert('Your Payment has been done successfully.');</script>");
                        //return RedirectToAction("ClientList", "Clients");
                        //return View();
                    }
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Your Payment has been done successfully.');</script>");
                   
                    //return View();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Transaction failed. Please try again !');</script>");
            
            //return RedirectToAction("ClientList", "Clients");
            //return View();
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public ActionResult SomeAction()
        {
            // ...
            return Json(
                new { Message = "Success Message!" },
                JsonRequestBehavior.AllowGet
            );
        }

    }
}
