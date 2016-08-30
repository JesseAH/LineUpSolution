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
    public class League_TeamController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: League_Team
        public ActionResult Index(string searchString)
        {
            var league_team = db.league_team.Include(l => l.league).Include(l => l.user);

            if (!String.IsNullOrEmpty(searchString))
            {
                league_team = league_team.Where(s => s.name.Contains(searchString)
                                       || s.league.name.Contains(searchString)
                                       || s.user.username.Contains(searchString)
                                       );
            }


            return View(league_team.ToList());
        }

        // GET: League_Team/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league_team league_team = db.league_team.Find(id);
            if (league_team == null)
            {
                return HttpNotFound();
            }
            return View(league_team);
        }

        // GET: League_Team/Create
        public ActionResult Create()
        {
            ViewBag.league_id = new SelectList(db.leagues, "id", "name");
            ViewBag.user_id = new SelectList(db.users, "id", "first_name");
            return View();
        }

        // POST: League_Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,league_id,name,is_paid_up,created_on,modified_on")] league_team league_team)
        {
            if (ModelState.IsValid)
            {
                league_team.created_on = DateTime.UtcNow;
                db.league_team.Add(league_team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.league_id = new SelectList(db.leagues, "id", "name", league_team.league_id);
            ViewBag.user_id = new SelectList(db.users, "id", "first_name", league_team.user_id);
            return View(league_team);
        }

        // GET: League_Team/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league_team league_team = db.league_team.Find(id);
            if (league_team == null)
            {
                return HttpNotFound();
            }
            ViewBag.league_id = new SelectList(db.leagues, "id", "name", league_team.league_id);
            ViewBag.user_id = new SelectList(db.users, "id", "first_name", league_team.user_id);
            return View(league_team);
        }

        // POST: League_Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,league_id,name,is_paid_up,created_on,modified_on")] league_team league_team)
        {
            if (ModelState.IsValid)
            {
                league_team.modified_on = DateTime.UtcNow;
                db.Entry(league_team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.league_id = new SelectList(db.leagues, "id", "name", league_team.league_id);
            ViewBag.user_id = new SelectList(db.users, "id", "first_name", league_team.user_id);
            return View(league_team);
        }

        // GET: League_Team/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league_team league_team = db.league_team.Find(id);
            if (league_team == null)
            {
                return HttpNotFound();
            }
            return View(league_team);
        }

        // POST: League_Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            league_team league_team = db.league_team.Find(id);
            db.league_team.Remove(league_team);
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
