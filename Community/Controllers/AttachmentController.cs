using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Community.Controllers
{
    public class AttachmentController : Controller
    {
        // GET: Attachment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Attachment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Attachment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attachment/Create
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

        // GET: Attachment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Attachment/Edit/5
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

        // GET: Attachment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Attachment/Delete/5
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
