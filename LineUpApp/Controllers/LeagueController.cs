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
    public class LeagueController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: League
        public ActionResult Index()
        {
            var leagues = db.leagues.Include(l => l.game_type);
            return View(leagues.ToList());
        }

        // GET: League/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league league = db.leagues.Find(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // GET: League/Create
        public ActionResult Create()
        {
            ViewBag.admin_user_id = new SelectList(db.users, "id", "_user_descriptor").OrderBy(l => l.Text);
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name").OrderBy(l => l.Text);

            return View();
        }

        // POST: League/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,admin_user_id,game_type_id,name,max_players,lock_date,price,round_winnings_percentage,is_private,password,is_completed,description,created_on,modified_on")] league league)
        {
            if (ModelState.IsValid)
            {
                league.created_on = DateTime.UtcNow;
                db.leagues.Add(league);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", league.game_type_id).OrderBy(l => l.Text);
            ViewBag.admin_user_id = new SelectList(db.users, "id", "_user_descriptor", league.admin_user_id).OrderBy(l => l.Text);

            return View(league);
        }

        // GET: League/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league league = db.leagues.Find(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", league.game_type_id).OrderBy(l => l.Text);
            ViewBag.admin_user_id = new SelectList(db.users, "id", "_user_descriptor", league.admin_user_id).OrderBy(l => l.Text);
            return View(league);
        }

        // POST: League/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,admin_user_id,game_type_id,name,max_players,lock_date,price,round_winnings_percentage,is_private,password,is_completed,description,created_on,modified_on")] league league)
        {
            if (ModelState.IsValid)
            {
                league.modified_on = DateTime.UtcNow;
                db.Entry(league).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", league.game_type_id).OrderBy(l => l.Text);
            ViewBag.admin_user_id = new SelectList(db.users, "id", "_user_descriptor", league.admin_user_id).OrderBy(l => l.Text);
            return View(league);
        }

        // GET: League/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            league league = db.leagues.Find(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: League/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            league league = db.leagues.Find(id);
            db.leagues.Remove(league);
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
