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
    public class SyllabusContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SyllabusContents
        public async Task<ActionResult> Index()
        {
            return View(await db.SyllabusContents.ToListAsync());
        }

        // GET: SyllabusContents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SyllabusContent syllabusContent = await db.SyllabusContents.FindAsync(id);
            if (syllabusContent == null)
            {
                return HttpNotFound();
            }
            return View(syllabusContent);
        }

        // GET: SyllabusContents/Create
        public ActionResult Create(int? syllabusId)
        {
            ViewBag.Subjects = new SelectList(db.Subjects, "Id", "Name");
            var newItem = new SyllabusContent { Syllabus = db.Syllabi.Include(s => s.Specialty).FirstOrDefault(s => s.Id == syllabusId) };
            return View(newItem);
        }

        // POST: SyllabusContents/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SyllabusContent syllabusContent)
        {
            if (ModelState.IsValid)
            {
                syllabusContent.Subject = db.Subjects.Find(syllabusContent.Subject.Id);
                syllabusContent.Syllabus = db.Syllabi.Find(syllabusContent.Syllabus.Id);
                db.SyllabusContents.Add(syllabusContent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(syllabusContent);
        }

        // GET: SyllabusContents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Subjects = new SelectList(db.Subjects, "Id", "Name");
            SyllabusContent syllabusContent = await db.SyllabusContents
                .Include(sc => sc.Syllabus).Include(sc => sc.Subject)
                .FirstOrDefaultAsync(sc => sc.Id == id);
            if (syllabusContent == null)
            {
                return HttpNotFound();
            }
            return View(syllabusContent);
        }

        // POST: SyllabusContents/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SyllabusContent syllabusContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(syllabusContent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Syllabus", new { id = syllabusContent.Syllabus.Id });
            }
            return View(syllabusContent);
        }

        // GET: SyllabusContents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SyllabusContent syllabusContent = db.SyllabusContents.Include(s => s.Syllabus).FirstOrDefault(s => s.Id == id);
            if (syllabusContent == null)
            {
                return HttpNotFound();
            }
            return View(syllabusContent);
        }

        // POST: SyllabusContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SyllabusContent syllabusContent = db.SyllabusContents.Include(s => s.Syllabus).FirstOrDefault(s => s.Id == id);
            db.SyllabusContents.Remove(syllabusContent);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Syllabus", new { id = syllabusContent.Syllabus.Id });
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
