using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VibExchange.Filters;

namespace VibExchange.Areas.RemoteAnalysis.Controllers
{
    public class HomeRAMController : Controller
    {
        //
        // GET: /RemoteAnalysis/HomeRAM/
       [InitializeSimpleMembershipAttribute]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        //
        // GET: /RemoteAnalysis/HomeRAM/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RemoteAnalysis/HomeRAM/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RemoteAnalysis/HomeRAM/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RemoteAnalysis/HomeRAM/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RemoteAnalysis/HomeRAM/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RemoteAnalysis/HomeRAM/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RemoteAnalysis/HomeRAM/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
