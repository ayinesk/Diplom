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
            return View();
        }

        [HttpPost]
        public ActionResult LoadsSearch(int term, int year)
        {
            var loadsGroups = db.Loads
                .Where(l => l.Term == term && l.Year == year)
                .Include(l => l.Subject)
                .Include(l => l.Lecturer)
                .Include(l => l.Group)
                .Include(l => l.Stream)
                .Include(l => l.SyllabusContent)
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

        // GET: Loads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Loads/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StreamsCount,SubgroupsCount")] Load load)
        {
            if (ModelState.IsValid)
            {
                db.Loads.Add(load);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
