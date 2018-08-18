using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VibExchange.Models;

namespace VibExchange.Controllers
{
    public class ServicesController : Controller
    {
        //
        // GET: /Services/

        public ActionResult ServiceDetail()
        {
            ViewBag.method = "Manually Analysis Method";
            return View();
        }
        public ActionResult AnalysisMethod(string id)
        {
            ViewBag.AnalysisMethod = id;
            if (Session["UserName"] != null)
            {
                ViewBag.UserName = "";
            }
            else
            {
                ViewBag.UserName = Session["UserName"].ToString();
            }
            return View();
        }

        public ActionResult getService(string serviceID)
        {
            AnalysisMethod file = new AnalysisMethod();
            file.CostList = GetCostList();
            switch (serviceID)
            {
                case "Centrifugal Pump":
                    {
                        return View("CentrifugalpumpService",file);
                    }
                case "Fans":
                    {
                        return View("FansService", file);
                    }
                case "Bearings":
                    {
                        return View("BearingsService", file);
                    }
                case "Gears":
                    {
                        return View("GearsService", file);
                    }
                case "Motor Current":
                    {
                        return View("MotorcurrentService", file);
                    }
                case "HFD":
                    {
                        return View("HFDService", file);
                    }
                default:
                    {
                        return View("ServiceDetail");
                    }
            }

        }

        public List<UploadFile> GetCostList()
        {
            List<UploadFile> allList = new List<UploadFile>();

            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("select * from AnalysisCost", CommandType.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    allList.Add(new UploadFile
                    {
                        sr = Convert.ToString(dr["ServiceType"]),
                        costperpoint = Convert.ToString(dr["CostPerPoint"]),
                        costpergraph = Convert.ToString(dr["CostPerGraphOrImage"]),
                        AnalysisTime = Convert.ToString(dr["AnalysisTime"])
                    });
                }
                return allList;
            }
        }
    }
}
