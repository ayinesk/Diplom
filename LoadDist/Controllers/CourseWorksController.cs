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
    public class CourseWorksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseWorks
        public async Task<ActionResult> Index()
        {
            var courseWorks = await db.CourseWorks.Include(cw => cw.Subject).ToListAsync();
            return View(courseWorks);
        }

        // GET: CourseWorks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseWork courseWork = await db.CourseWorks.Include(cw => cw.Subject).FirstOrDefaultAsync(cw => cw.Id == id);
            if (courseWork == null)
            {
                return HttpNotFound();
            }
            return View(courseWork);
        }

        // GET: CourseWorks/Create
        public ActionResult Create()
        {
            var subjects = db.Subjects;
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
            return View();
        }

        // POST: CourseWorks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CourseWork courseWork)
        {
            if (ModelState.IsValid)
            {
                courseWork.Subject = await db.Subjects.FindAsync(courseWork.Subject.Id);
                db.CourseWorks.Add(courseWork);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseWork);
        }

        // GET: CourseWorks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseWork courseWork = await db.CourseWorks.Include(cw => cw.Subject).FirstOrDefaultAsync(cw => cw.Id ==id);
            if (courseWork == null)
            {
                return HttpNotFound();
            }
            return View(courseWork);
        }

        // POST: CourseWorks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CourseWork courseWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseWork).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseWork);
        }

        // GET: CourseWorks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseWork courseWork = await db.CourseWorks.FindAsync(id);
            if (courseWork == null)
            {
                return HttpNotFound();
            }
            return View(courseWork);
        }

        // POST: CourseWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseWork courseWork = await db.CourseWorks.FindAsync(id);
            db.CourseWorks.Remove(courseWork);
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
