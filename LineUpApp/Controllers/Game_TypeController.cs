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
    public class Game_TypeController : Controller
    {
        private AdminEntities db = new AdminEntities();

        // GET: Game_Type
        public ActionResult Index()
        {
            return View(db.game_type.ToList());
        }

        // GET: Game_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game_type game_type = db.game_type.Find(id);
            if (game_type == null)
            {
                return HttpNotFound();
            }
            return View(game_type);
        }

        // GET: Game_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Game_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,admin_user_id,number_of_rounds,created_on,modified_on")] game_type game_type)
        {
            if (ModelState.IsValid)
            {
                game_type.created_on = DateTime.Now;
                db.game_type.Add(game_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game_type);
        }

        // GET: Game_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game_type game_type = db.game_type.Find(id);
            if (game_type == null)
            {
                return HttpNotFound();
            }
            return View(game_type);
        }

        // POST: Game_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,admin_user_id,number_of_rounds,created_on,modified_on")] game_type game_type)
        {
            if (ModelState.IsValid)
            {
                game_type.modified_on = DateTime.Now;
                db.Entry(game_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game_type);
        }

        // GET: Game_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game_type game_type = db.game_type.Find(id);
            if (game_type == null)
            {
                return HttpNotFound();
            }
            return View(game_type);
        }

        // POST: Game_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            game_type game_type = db.game_type.Find(id);
            db.game_type.Remove(game_type);
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
