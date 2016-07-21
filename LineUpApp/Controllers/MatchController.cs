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
    public class MatchController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: Match
        public ActionResult Index(string searchString)
        {
            var matches = db.matches.Include(m => m.round).Include(m => m.match_type).Include(m => m.team).Include(m => m.team1).Include(m => m.team2);

            if (!String.IsNullOrEmpty(searchString))
            {
                matches = matches.Where(s => s.round.name.Contains(searchString)
                                       || s.team.name.Contains(searchString)
                                       || s.match_type.name.Contains(searchString)
                                       || s.team2.name.Contains(searchString)
                                       );
            }


            return View(matches.ToList());
        }

        // GET: Match/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // GET: Match/Create
        public ActionResult Create()
        {
            ViewBag.round_id = new SelectList(db.rounds, "id", "name");
            ViewBag.match_type_id = new SelectList(db.match_type, "id", "name");
            ViewBag.team1_id = new SelectList(db.teams, "id", "name");
            ViewBag.team2_id = new SelectList(db.teams, "id", "name");
            ViewBag.winning_team_id = new SelectList(db.teams, "id", "name");
            return View();
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,match_type_id,round_id,team1_id,team2_id,winning_team_id,description,team1_start_date,team2_start_date,created_on,modified_on,match_outcome")] match match)
        {
            if (ModelState.IsValid)
            {
                match.created_on = DateTime.Now;
                db.matches.Add(match);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.round_id = new SelectList(db.rounds, "id", "name", match.round_id);
            ViewBag.match_type_id = new SelectList(db.match_type, "id", "name", match.match_type_id);
            ViewBag.team1_id = new SelectList(db.teams, "id", "name", match.team1_id);
            ViewBag.team2_id = new SelectList(db.teams, "id", "name", match.team2_id);
            ViewBag.winning_team_id = new SelectList(db.teams, "id", "name", match.winning_team_id);
            return View(match);
        }

        // GET: Match/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            ViewBag.round_id = new SelectList(db.rounds, "id", "name", match.round_id);
            ViewBag.match_type_id = new SelectList(db.match_type, "id", "name", match.match_type_id);
            ViewBag.team1_id = new SelectList(db.teams, "id", "name", match.team1_id);
            ViewBag.team2_id = new SelectList(db.teams, "id", "name", match.team2_id);
            ViewBag.winning_team_id = new SelectList(db.teams, "id", "name", match.winning_team_id);
            return View(match);
        }

        // POST: Match/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,match_type_id,round_id,team1_id,team2_id,winning_team_id,description,team1_start_date,team2_start_date,created_on,modified_on,match_outcome")] match match)
        {
            if (ModelState.IsValid)
            {
                match.modified_on = DateTime.Now;
                db.Entry(match).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.round_id = new SelectList(db.rounds, "id", "name", match.round_id);
            ViewBag.match_type_id = new SelectList(db.match_type, "id", "name", match.match_type_id);
            ViewBag.team1_id = new SelectList(db.teams, "id", "name", match.team1_id);
            ViewBag.team2_id = new SelectList(db.teams, "id", "name", match.team2_id);
            ViewBag.winning_team_id = new SelectList(db.teams, "id", "name", match.winning_team_id);
            return View(match);
        }

        // GET: Match/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // POST: Match/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            match match = db.matches.Find(id);
            db.matches.Remove(match);
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
