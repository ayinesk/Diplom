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
    public class LoadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Loads
        public async Task<ActionResult> Index()
        {
            var lecturersSelectList = new List<object>();
            foreach (var lecturer in db.Lecturers.ToList())
            {
                lecturersSelectList.Add(new
                {
                    id = lecturer.Id,
                    displayValue = $"{lecturer.Surname} {lecturer.Name} {lecturer.Patronymic}"
                });
            }
            ViewBag.Lecturers = new SelectList(lecturersSelectList, "id", "displayValue");
            ViewBag.Streams = new SelectList(db.Streams, "Id", "Title");
            ViewBag.Groups = new SelectList(db.Groups, "Id", "GroupNumber");            
            ViewBag.Subjects = new SelectList(db.Subjects, "Id", "Name");
            var syllabusContentsSelectList = new List<object>();
            var syllabusContents = db.SyllabusContents
                .Include(sc => sc.Syllabus)
                .Include(sc => sc.Syllabus.Specialty)
                .Include(sc => sc.Subject).ToList();
            foreach (var sContent in syllabusContents)
            {
                syllabusContentsSelectList.Add(new
                {
                    id = sContent.Id,
                    displayValue = $"{sContent.Syllabus.Specialty.Name} ({sContent.Syllabus.AdmissionYear}) {sContent.Subject.Name}"
                });
            }
            ViewBag.SyllabusContent = new SelectList(syllabusContentsSelectList, "id", "displayValue");
            return View();
        }

        [HttpPost]
        public ActionResult LoadsSearch(int term, int year)
        {
            var loadsGroups = db.Loads
                .Include(l => l.Subject)
                .Include(l => l.Lecturer)
                .Include(l => l.Group)
                .Include(l => l.Stream)
                .Include(l => l.SyllabusContent)
                .Where(l => l.Term == term && l.Year == year)
                .AsEnumerable()
                .GroupBy(l => l.Lecturer);
            var loadsModels = new List<LoadsViewModel>();
            foreach (IGrouping<Lecturer, Load> group in loadsGroups)
            {
                loadsModels.Add(new LoadsViewModel
                {
                    Lecturer = group.Key,
                    LecturerLoads = group.ToList(),
                    Term = term,
                    Year = year,
                    TotalExamHours = group.Sum(l => l.ExamHours),
                    TotalLabsHours = group.Sum(l => l.LabsHours),
                    TotalLectureHours = group.Sum(l => l.LectureHours),
                    TotalPracticalHours = group.Sum(l => l.PracticalHours),
                    TotalTestHours = group.Sum(l => l.TestHours)
                });
            }
            return PartialView("_LoadsTable", loadsModels); ;
        }

        // GET: Loads/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = await db.Loads.FindAsync(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            return View(load);
        }

        // POST: Loads/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(int year, int term, int lecturerId, 
            int streamId, int groupId, int subjectId, int syllabusContentId, int lections,
            int labs, int practicals, int exam, int test)
        {
            var load = new Load
            {
                Year = year,
                Term = term,
                Lecturer = db.Lecturers.Find(lecturerId),
                Stream = db.Streams.Find(streamId),
                Group = db.Groups.Find(groupId),
                Subject = db.Subjects.Find(subjectId),
                SyllabusContent = db.SyllabusContents.Find(syllabusContentId),
                LectureHours = lections,
                LabsHours = labs,
                PracticalHours = practicals,
                ExamHours = exam,
                TestHours = test
            };
            if (ModelState.IsValid)
            {
                db.Loads.Add(load);
                await db.SaveChangesAsync();
                return RedirectToAction("LoadsSearch", new { term, year});
            }

            return View(load);
        }

        // GET: Loads/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = await db.Loads.FindAsync(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            return View(load);
        }

        // POST: Loads/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StreamsCount,SubgroupsCount")] Load load)
        {
            if (ModelState.IsValid)
            {
                db.Entry(load).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(load);
        }

        // GET: Loads/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = await db.Loads.FindAsync(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            return View(load);
        }

        // POST: Loads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Load load = await db.Loads.FindAsync(id);
            db.Loads.Remove(load);
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
