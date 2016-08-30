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
    public class RoundController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: Round
        public ActionResult Index()
        {
            var rounds = db.rounds.Include(r => r.game_type);
            return View(rounds.ToList());
        }

        // GET: Round/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            round round = db.rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // GET: Round/Create
        public ActionResult Create()
        {
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name");
            return View();
        }

        // POST: Round/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,short_name,round_number,start_date,end_date,lock_date,game_type_id,created_on,modified_on")] round round)
        {
            if (ModelState.IsValid)
            {
                round.created_on = DateTime.UtcNow;
                db.rounds.Add(round);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", round.game_type_id);
            return View(round);
        }

        // GET: Round/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            round round = db.rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", round.game_type_id);
            return View(round);
        }

        // POST: Round/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,short_name,round_number,start_date,end_date,lock_date,game_type_id,created_on,modified_on")] round round)
        {
            if (ModelState.IsValid)
            {
                round.modified_on = DateTime.UtcNow;
                db.Entry(round).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.game_type_id = new SelectList(db.game_type, "id", "name", round.game_type_id);
            return View(round);
        }

        // GET: Round/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            round round = db.rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // POST: Round/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            round round = db.rounds.Find(id);
            db.rounds.Remove(round);
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
