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
    public class PickController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: Pick
        public ActionResult Index(string searchString)
        {
            IList<pick> picks = db.picks.Include(p => p.league_team).Include(p => p.team).Include(p => p.match).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                picks = picks.Where(s => s.league_team._league_team_descriptor.Contains(searchString)
                                       || s.team.name.Contains(searchString)
                                       ).ToList();
            }
            
            return View(picks);
        }

        // GET: Pick/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pick pick = db.picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }
            return View(pick);
        }

        // GET: Pick/Create
        public ActionResult Create()
        {
            ViewBag.league_team_id = new SelectList(db.league_team, "id", "_league_team_descriptor");
            ViewBag.picked_team_id = new SelectList(db.teams, "id", "name");
            ViewBag.match_id = new SelectList(db.matches, "id", "_match_descriptor");
            return View();
        }

        // POST: Pick/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,match_id,league_team_id,picked_team_id,confidence_value")] pick pick)
        {
            if (ModelState.IsValid)
            {
                pick.created_on = DateTime.UtcNow;
                db.picks.Add(pick);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.league_team_id = new SelectList(db.league_team, "id", "_league_team_descriptor", pick.league_team_id);
            ViewBag.picked_team_id = new SelectList(db.teams, "id", "name", pick.picked_team_id);
            ViewBag.match_id = new SelectList(db.matches, "id", "_match_descriptor", pick.match_id);
            return View(pick);
        }

        // GET: Pick/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pick pick = db.picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }
            ViewBag.league_team_id = new SelectList(db.league_team, "id", "name", pick.league_team_id);
            ViewBag.picked_team_id = new SelectList(db.teams, "id", "name", pick.picked_team_id);
            ViewBag.match_id = new SelectList(db.matches, "id", "description", pick.match_id);
            return View(pick);
        }

        // POST: Pick/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,match_id,league_team_id,picked_team_id,confidence_value")] pick pick)
        {
            if (ModelState.IsValid)
            {
                pick.modified_on = DateTime.UtcNow;
                db.Entry(pick).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.league_team_id = new SelectList(db.league_team, "id", "name", pick.league_team_id);
            ViewBag.picked_team_id = new SelectList(db.teams, "id", "name", pick.picked_team_id);
            ViewBag.match_id = new SelectList(db.matches, "id", "description", pick.match_id);
            return View(pick);
        }

        // GET: Pick/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pick pick = db.picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }
            return View(pick);
        }

        // POST: Pick/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pick pick = db.picks.Find(id);
            db.picks.Remove(pick);
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
