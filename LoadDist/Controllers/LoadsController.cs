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
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using LoadDist.Reports;

namespace LoadDist.Controllers
{
    public class LoadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        LoadsReportDataSet ds = new LoadsReportDataSet();

        // GET: Loads
        public async Task<ActionResult> Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSylContentDDL(int lecturerId)
        {
            var selectedLecturer = db.Lecturers.Include(lect => lect.Subjects).FirstOrDefault(lect => lect.Id == lecturerId);
            var subjects = db.Subjects.Include(subj => subj.Lecturers).ToList();
            var lecturerSubjects = selectedLecturer.Subjects;
            var syllabusContents = db.SyllabusContents
                .Include(sc => sc.Syllabus)
                .Include(sc => sc.Syllabus.Specialty)
                .Include(sc => sc.Subject)
                .ToList();
            var sylContent = new List<SyllabusContent>();
            foreach (var s in syllabusContents)
            {
                if (lecturerSubjects.Contains(s.Subject))
                {
                    sylContent.Add(s);
                }
            }
            var options = sylContent.Select(sc => $"<option value='{sc.Id}'>" +
                $"{sc.Syllabus.Specialty.Name} ({sc.Syllabus.AdmissionYear}) {sc.Subject.Name}" +
                $"</option>");
            return Content("<option> </option>" + String.Join("", options));
        }

        [HttpPost]
        public ActionResult UpdateGroupDDL(int streamId)
        {
            var selectedStream = db.Streams.Include(str => str.Groups).FirstOrDefault(str =>str.Id == streamId);
            var options = selectedStream.Groups.Select(g => $"<option value='{g.Id}'>{g.GroupNumber}</option>");
            return Content(String.Join("", options));
        }

        [HttpPost]
        public ActionResult UpdateHoursData(int scId, int groupId)
        {
            var selectedGroup = db.Groups.Find(groupId);
            var selectedSc = db.SyllabusContents.Find(scId);
            var standards = db.Standards.FirstOrDefault();
            selectedSc.ExamHours = Convert.ToInt32(selectedSc.ExamHours * selectedGroup.StudentsCount * standards.ExamStandard);
            selectedSc.TestHours = Convert.ToInt32(selectedSc.TestHours * selectedGroup.StudentsCount * standards.TestStandard);
            selectedSc.Consultation = Convert.ToInt32(selectedSc.Consultation * selectedGroup.StudentsCount * standards.ConsultationStandard);
            var json = new JavaScriptSerializer().Serialize(selectedSc);
            return Json( json, JsonRequestBehavior.AllowGet);
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
                .GroupBy(l => l.Lecturer).ToList();
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
                    TotalTestHours = group.Sum(l => l.TestHours),
                    TotalConsultationHours = group.Sum(l => l.Consultation)
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

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Lecturers = GetLecturersSL();
            ViewBag.Streams = new SelectList(db.Streams, "Id", "Title");
            ViewBag.Groups = new SelectList(db.Groups, "Id", "GroupNumber");
            ViewBag.Subjects = new SelectList(db.Subjects, "Id", "Name");
            var syllabusContents = db.SyllabusContents
                .Include(sc => sc.Syllabus)
                .Include(sc => sc.Syllabus.Specialty)
                .Include(sc => sc.Subject).ToList();           
            ViewBag.SyllabusContent = GetSyllabusContentsSL(syllabusContents);
            return View();
        }

        // POST: Loads/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(Load load)
        {                
            if (ModelState.IsValid)
            {
                load.Lecturer = db.Lecturers.Find(load.Lecturer.Id);
                load.Stream = db.Streams.Find(load.Stream.Id);
                load.Group = db.Groups.Find(load.Group.Id);                
                load.SyllabusContent = db.SyllabusContents.Find(load.SyllabusContent.Id);
                load.Subject = load.SyllabusContent.Subject;
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
            Load load = await db.Loads.FindAsync(id);
            db.Loads.Remove(load);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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

        private SelectList GetSyllabusContentsSL(List<SyllabusContent> syllabusContents)
        {
            var syllabusContentsSelectList = new List<object>();
            foreach (var sContent in syllabusContents)
            {
                syllabusContentsSelectList.Add(new
                {
                    id = sContent.Id,
                    displayValue = $"{sContent.Syllabus.Specialty.Name} ({sContent.Syllabus.AdmissionYear}) {sContent.Subject.Name}"
                });
            }
            return new SelectList(syllabusContentsSelectList, "id", "displayValue"); ;
        }

        public ActionResult GenerateReport()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1000);
            reportViewer.Height = Unit.Percentage(1000);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


            SqlConnection conx = new SqlConnection(connectionString);
            SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM LoadsView", conx);

            adp.Fill(ds, ds.LoadsView.TableName);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\LoadsReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LoadsReportDataSet", ds.Tables[0]));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        private SelectList GetLecturersSL()
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
            return new SelectList(lecturersSelectList, "id", "displayValue");
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
