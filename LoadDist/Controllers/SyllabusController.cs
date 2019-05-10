using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoadDist.Models;
using LoadDist.Models.DataModels;

namespace LoadDist.Controllers
{
    public class SyllabusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Syllabus
        public async Task<ActionResult> Index()
        {
            return View(await db.Syllabi.Include(s => s.Specialty).ToListAsync());
        }

        // GET: Syllabus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SyllabusContents = db.SyllabusContents.Include(sc => sc.Subject).Where(sc => sc.Syllabus.Id == id).ToList();
            Syllabus syllabus = db.Syllabi.Include(s => s.Specialty).FirstOrDefault(s => s.Id == id);
            if (syllabus == null)
            {
                return HttpNotFound();
            }
            return View(syllabus);
        }

        // GET: Syllabus/Create
        public ActionResult Create()
        {
            ViewBag.Specialties = new SelectList(db.Specialties, "Id", "Name");
            return View();
        }

        // POST: Syllabus/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Syllabus syllabus)
        {
            if (ModelState.IsValid)
            {
                syllabus.Specialty = db.Specialties.Find(syllabus.Specialty.Id);
                db.Syllabi.Add(syllabus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(syllabus);
        }

        // GET: Syllabus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Syllabus syllabus = await db.Syllabi.FindAsync(id);
            if (syllabus == null)
            {
                return HttpNotFound();
            }
            return View(syllabus);
        }

        // POST: Syllabus/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AdmissionYear")] Syllabus syllabus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(syllabus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(syllabus);
        }

        // GET: Syllabus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Syllabus syllabus = await db.Syllabi.FindAsync(id);
            if (syllabus == null)
            {
                return HttpNotFound();
            }
            return View(syllabus);
        }

        // POST: Syllabus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Syllabus syllabus = await db.Syllabi.FindAsync(id);
            db.Syllabi.Remove(syllabus);
            await db.SaveChangesAsync();
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
