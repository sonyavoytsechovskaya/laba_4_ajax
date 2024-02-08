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
    public class AlpsController : Controller
    {
        private db_Voynova_6Entities db = new db_Voynova_6Entities();


        public ActionResult Index()
        {
            return View(db.Alp.ToList());
        }


        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alp alp = db.Alp.Find(id);
            if (alp == null)
            {
                return HttpNotFound();
            }
            return View(alp);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Alp alp)
        {
            if (ModelState.IsValid)
            {
                db.Alp.Add(alp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alp);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alp alp = db.Alp.Find(id);
            if (alp == null)
            {
                return HttpNotFound();
            }
            return View(alp);
        }

        // POST: Clients/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name")] Alp alp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alp);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alp alp = db.Alp.Find(id);
            if (alp == null)
            {
                return HttpNotFound();
            }
            return View(alp);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alp alp = db.Alp.Find(id);
            db.Alp.Remove(alp);
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