using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LineUpApp.Models;

namespace LineUpApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Match_TypeController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: Match_Type
        public ActionResult Index()
        {
            return View(db.match_type.ToList());
        }

        // GET: Match_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match_type match_type = db.match_type.Find(id);
            if (match_type == null)
            {
                return HttpNotFound();
            }
            return View(match_type);
        }

        // GET: Match_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Match_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,created_on,modified_on")] match_type match_type)
        {
            if (ModelState.IsValid)
            {
                match_type.created_on = DateTime.UtcNow;
                db.match_type.Add(match_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(match_type);
        }

        // GET: Match_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match_type match_type = db.match_type.Find(id);
            if (match_type == null)
            {
                return HttpNotFound();
            }
            return View(match_type);
        }

        // POST: Match_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,created_on,modified_on")] match_type match_type)
        {
            if (ModelState.IsValid)
            {
                match_type.modified_on = DateTime.UtcNow;
                db.Entry(match_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(match_type);
        }

        // GET: Match_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match_type match_type = db.match_type.Find(id);
            if (match_type == null)
            {
                return HttpNotFound();
            }
            return View(match_type);
        }

        // POST: Match_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            match_type match_type = db.match_type.Find(id);
            db.match_type.Remove(match_type);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
