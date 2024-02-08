using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class AlpsMountainsController : Controller
    {
        private db_Voynova_6Entities db = new db_Voynova_6Entities();
        public ActionResult Index()
        {
            var mountainAlp = db.AlpsMountains.Include(Par => Par.Alp).Include(Par => Par.Mountain);
            return View(mountainAlp.ToList());
        }

        // GET: BooksClients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlpsMountains mountainAlp = db.AlpsMountains.Find(id);
            if (mountainAlp == null)
            {
                return HttpNotFound();
            }
            return View(mountainAlp);
        }

        // GET: BooksClients/Create
        public ActionResult Create()
        {
            ViewBag.Mountain_id = new SelectList(db.Mountain, "Id", "Name");
            ViewBag.Alp_id = new SelectList(db.Alp, "Id", "Name");
            return View();
        }

        // POST: BooksClients/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,client_Id,book_Id")] AlpsMountains alpsMountains)
        {
            if (ModelState.IsValid)
            {
                db.AlpsMountains.Add(alpsMountains);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Mountain_id = new SelectList(db.Mountain, "Id", "Name", alpsMountains.Mountain_Id);
            ViewBag.Alp_id = new SelectList(db.Alp, "Id", "Name", alpsMountains.Alp_Id);

            return View(alpsMountains);
        }

        // GET: BooksClients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlpsMountains alpsMountains = db.AlpsMountains.Find(id);
            if (alpsMountains == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mountain_id = new SelectList(db.Mountain, "Id", "Name", alpsMountains.Mountain_Id);
            ViewBag.Alp_id = new SelectList(db.Alp, "Id", "Name", alpsMountains.Alp_Id);
            return View(alpsMountains);
        }

        // POST: BooksClients/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,client_Id,book_Id")] AlpsMountains alpsMountains)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alpsMountains).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Mountain_id = new SelectList(db.Mountain, "Id", "Name", alpsMountains.Mountain_Id);
            ViewBag.Alp_id = new SelectList(db.Alp, "Id", "Name", alpsMountains.Alp_Id);
            return View(alpsMountains);
        }

        // GET: BooksClients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlpsMountains alpsMountains = db.AlpsMountains.Find(id);
            if (alpsMountains == null)
            {
                return HttpNotFound();
            }
            return View(alpsMountains);
        }

        // POST: BooksClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlpsMountains alpsMountains = db.AlpsMountains.Find(id);
            db.AlpsMountains.Remove(alpsMountains);
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