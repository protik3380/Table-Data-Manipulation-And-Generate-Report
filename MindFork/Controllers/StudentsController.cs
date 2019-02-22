using CrystalDecisions.CrystalReports.Engine;
using MindFork.Migrations;
using MindFork.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MindFork.Controllers
{
    public class StudentsController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Subject);
            return View(students.ToList());
        }

        public ActionResult ExpertPdf()
        {
            //List<Student> allCustomer = new List<Student>();
            //allCustomer = db.Students.Include(s => s.Subject).ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport.rpt"));

            rd.SetDataSource(db.Students.Select(p => new
            {
                S_Id = p.S_Id,
                StudentId = p.StudentId,
                StudentName = p.StudentName,
                SubjectId = p.SubjectId,
                SubjectName = p.Subject.SubjectName,
                Mark = p.Mark
            }).ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "CustomerList.pdf");

        }
        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", student.SubjectId);
            return View(student);
        }

        public JsonResult GetStudentById(string studentId)
        {
            var student = db.Students.Where(s => s.StudentId == studentId).ToList();
            return Json(student, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMarksByIdandSubId(Student student)
        {
            var stu = db.Students.Where(s => s.SubjectId == student.SubjectId && s.StudentId == student.StudentId).ToList();
            return Json(stu, JsonRequestBehavior.AllowGet);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "S_Id,StudentId,StudentName,SubjectId,Mark")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", student.SubjectId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public JsonResult InsertCustomers(List<StudentDto> studentDto)
        {
            // var insertedRecords;
            //Check for NULL.
            if (studentDto == null)
            {
                studentDto = new List<StudentDto>();
            }

            //Loop and insert records.
            foreach (var student1 in studentDto)
            {
                var subject = student1.Subject;
                Subject subject1 = db.Subjects.Single(s => s.SubjectName == subject);
                var previousInfoStudent = db.Students.Where(s => s.StudentId == student1.StudentId
                                                                 && s.StudentName == student1.StudentName
                                                                 && s.SubjectId == subject1.SubjectId
                                                                 && s.Mark == student1.Mark).ToList();
                int isExists = previousInfoStudent.Count();

                if (isExists != 0) continue;

                var finalStudent = new Student
                {
                    StudentId = student1.StudentId,
                    StudentName = student1.StudentName,
                    SubjectId = subject1.SubjectId,
                    Mark = student1.Mark
                };
                db.Students.Add(finalStudent);
            }
            var insertedRecords = db.SaveChanges();
            return Json(insertedRecords);

        }




        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
