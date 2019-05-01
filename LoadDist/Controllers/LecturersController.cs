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
using LoadDist.Models.ViewModels;

namespace LoadDist.Controllers
{
    public class LecturersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lecturers
        public async Task<ActionResult> Index()
        {
            return View(await db.Lecturers.ToListAsync());
        }

        // GET: Lecturers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = db.Lecturers.Include(l => l.Subjects).SingleOrDefault(l => l.Id == id);
            
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            lecturer.Subjects = lecturer.Subjects ?? new List<Subject>();
            return View(lecturer);
        }

        // GET: Lecturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Surname,Patronymic")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Lecturers.Add(lecturer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lecturer);
        }

        // GET: Lecturers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Patronymic")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecturer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lecturer);
        }

        // GET: Lecturers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            db.Lecturers.Remove(lecturer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Lecturers/Create
        public ActionResult AddSubject(int? id)
        {
            Lecturer lecturer = db.Lecturers.Include(l => l.Subjects).SingleOrDefault(l => l.Id == id);
            var allSubjects = db.Subjects.ToList();
            var subjectsToAdd = allSubjects.Except(lecturer.Subjects);
            SelectList subjects = new SelectList(subjectsToAdd, "Id", "Name");
            ViewBag.Subjects = subjects;
            return View(new AddSubjectViewModel { LecturerId = lecturer.Id });
        }

        // POST: Lecturers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubject(AddSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                Lecturer thisLecturer = db.Lecturers.Find(model.LecturerId);
                Subject thisSubject = db.Subjects.Find(model.SubjectId);
                if (thisLecturer.Subjects != null)
                {
                    thisLecturer.Subjects.Add(thisSubject);
                }
                thisLecturer.Subjects = new List<Subject> { thisSubject };
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = thisLecturer.Id});
            }
            
            return View(model);
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
