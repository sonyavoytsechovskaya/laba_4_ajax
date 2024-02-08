using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Linq;
using System.Net;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class MountainsController : Controller
    {
        private db_Voynova_6Entities db = new db_Voynova_6Entities();
        // GET: Books
        public ActionResult Index()
        {
            return View(db.Mountain.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mountain mountain = db.Mountain.Find(id);
            if (mountain == null)
            {
                return HttpNotFound();
            }
            return View(mountain);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title")] Mountain mountain)
        {
            if (ModelState.IsValid)
            {
                db.Mountain.Add(mountain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mountain);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mountain mountain = db.Mountain.Find(id);
            if (mountain == null)
            {
                return HttpNotFound();
            }
            return View(mountain);
        }

        // POST: Books/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title")] Mountain mountain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mountain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mountain);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mountain mountain = db.Mountain.Find(id);
            if (mountain == null)
            {
                return HttpNotFound();
            }
            return View(mountain);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mountain mountain = db.Mountain.Find(id);
            db.Mountain.Remove(mountain);
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