using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tb.Core.Models;
using tb.Core.Services;
using tb.Web.ViewModels;

namespace tb.Web.Controllers
{   
    public class StudentController : BaseController
    {

        private IUserService svc;
        public StudentController(IUserService ss)
        {
            svc = ss;
        }
        
        // GET /student/index
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Index()
        {
            var students = svc.GetStudents();
           
            return View(students);
        }


        // GET /student/details/{id}
        public IActionResult Details(int id)
        {
            // retrieve the student with specified id from the service
            var s = svc.GetStudent(id);

            // check if s is null and return NotFound()
            if (s == null)
            {
                Alert("Student Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index), new { Id = id });
            }

            // pass student as parameter to the view
            return View(s);
        }

         // GET: /student/create
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Create()
        {
            // display blank form to create a student
            var s = new Student();
            return View(s);
        }

        // POST /student/create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Tutor")]

        public IActionResult Create([Bind] Student s) //Check here
        {
            // check email is unique for this student
            if (svc.IsDuplicateEmail(s.Email, s.Id))
            {
                // add manual validation error
                ModelState.AddModelError(nameof(s.Email),"The email address is already in use");  
            }

            // validate student
            if (ModelState.IsValid)
            {
                // pass data to service to store 
                var added = svc.AddStudent(s);
                Alert($"Student {s.FirstName} {s.LastName} created successfully", AlertType.info);
                
                return RedirectToAction(nameof(Index));
            }
            // redisplay the form for editing as there are validation errors
            return View(s);
        }

        // GET /student/edit/{id}
        [Authorize(Roles="Admin,Tutor,Parent,AdultStudent")]
        public IActionResult Edit(int id)
        {
            // load the student using the service
            var s = svc.GetStudent(id);

            // check if s is null and return NotFound()
            if (s == null)
            {
                Alert($"No such student {id}", AlertType.warning); 
                return RedirectToAction(nameof(Index), new { Id = id });
            }   

            // pass student to view for editing
            return View(s);
        }

        // POST /student/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Tutor,Parent,AdultStudent")]
        public IActionResult Edit(int id, Student s)
        {
            // check email is unique for this student
            if (svc.IsDuplicateEmail(s.Email, s.Id))
            {
                // add manual validation error
                ModelState.AddModelError(nameof(s.Email),"The email address is already in use");
            } 
            
            // validate student
            if (ModelState.IsValid)
            {
                // pass data to service to update
                svc.UpdateStudent(s);
                Alert($"Student details for {s.FirstName} {s.LastName} saved", AlertType.info);
                return RedirectToAction(nameof(Details), new { Id = id }); 
            }

            // redisplay the form for editing as validation errors
            return View(s);
        }

        // GET / student/delete/{id}
        [Authorize(Roles="Admin,Tutor")]       
        public IActionResult Delete(int id)
        {
            // load the student using the service
            var s = svc.GetStudent(id);
            // check the returned student is not null and if so return NotFound()
            if (s == null)
            {
                Alert("Student Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }     
            
            // pass student to view for deletion confirmation
            return View(s);
        }

        // POST /student/delete/{id}
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            // delete student via service
            svc.DeleteStudent(id);
         
            Alert($"Student {id} deleted successfully", AlertType.success);
            // redirect to the index view
            return RedirectToAction(nameof(Index), new { Id = id });
        }

         // GET /student/createQuery
        [Authorize(Roles="Admin,Tutor,Parent,AdultStudent,Pupil")]
        public IActionResult CreateQuery(int id)
        {
            var s = svc.GetStudent(id);
            // check the returned student is not null and if so alert
            if (s == null)
            {
                Alert($"No such student {id}", AlertType.warning);          
                return RedirectToAction(nameof(Index));
            }   
            // create the Query view model and populate the StudentId property
            var q = new QueryCreateViewModel {
                StudentId = id
            };
            
            return View("CreateQuery", q);
            
        }

        // POST /student/createQuery
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Tutor,Parent,AdultStudent,Pupil")]
        public IActionResult CreateQuery([Bind("StudentId, Issue")]QueryCreateViewModel qvm)
        {
            var s = svc.GetStudent(qvm.StudentId);
             // check the returned student is not null and if so return NotFound()
            if (s == null)
            {
                Alert($"No such student {qvm.StudentId}", AlertType.warning);          
                return RedirectToAction(nameof(Index));
            }  
        
            // create the Query view model and populate the StudentId property
            svc.CreateQuery(qvm.StudentId, qvm.Issue);
            Alert($"Query from {s.FirstName} {s.LastName} created successfully", AlertType.success);   

            return RedirectToAction("Details", new { Id = qvm.StudentId });
        }

        // GET /Student/createProgressLog
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult CreateProgressLog(int id)
        {
            var s = svc.GetStudent(id);
             // check the returned Student is not null and if so alert
            if (s == null)
            {
                Alert($"Sorry! No such Student {id}", AlertType.warning);          
                return RedirectToAction(nameof(Index));
            }  
            // create the ProgressLog and populate the StudentId property
            var pl = new ProgressLog {
                StudentId = id
            };
 
            return View("CreateProgressLog", pl);
        }

        // POST /Student/createProgressLog
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult CreateProgressLog (ProgressLog pl)
        {
            var s = svc.GetStudent(pl.StudentId);
             // check the returned Student is not null and if so alert
            if (s == null)
            {
                Alert($"No such Student {pl.StudentId}", AlertType.warning);          
                return RedirectToAction(nameof(Details));
            }  
            
            Alert($"ProgressLog for {s.FirstName} {s.LastName} created successfully", AlertType.success);   
            // create the ProgressLog view model and populate the StudentId property
            svc.AddProgressLog(new ProgressLog { StudentId = pl.StudentId, Progress = pl.Progress});
 
            return RedirectToAction("Details", new { Id = pl.StudentId });
        }

        // GET /progressLog/edit/{id}
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult EditProgressLog(int id)
        {
            // load the student using the service
            var pl = svc.GetProgressLogById(id);

            // check if s is null and return NotFound()
            if (pl == null)
            {
                Alert($"No such progress log {id}", AlertType.warning); 
                return RedirectToAction(nameof(Details), new { Id = id });
            }   

            // pass student to view for editing
            return View(pl);
        }

        // POST /progressLog/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult EditProgressLog(int id, ProgressLog pl)
        {
            
            // validate progressLog
            if (ModelState.IsValid)
            {
                // pass data to service to update
                svc.UpdateProgressLog(pl);
                Alert($"Progress log {id} saved", AlertType.info);
                return RedirectToAction(nameof(Details), new { Id = id }); 
            }

            // redisplay the form for editing as validation errors
            return View(pl);
        }

        // GET / student/delete/{id}
        [Authorize(Roles="Admin,Tutor")]       

           // GET / ProgressLog/delete/{id}
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult DeleteProgressLog(int id)
        {
            // load the ProgressLog using the service
            var pl = svc.GetProgressLogById(id);
            // check the returned ProgressLog is not null and if so alert
            if (pl == null)
            {
                Alert($"No such ProgressLog {pl.Id}", AlertType.warning);          
                return RedirectToAction(nameof(Details));
            }     
            
            // pass ProgressLog to view for deletion confirmation
            return View(pl);
        }

        // POST /ProgressLog/delete/{id}
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult DeleteProgressLogConfirm(int id, int studentId)
        {
            // delete ProgressLog via service
             svc.DeleteProgressLog(id);
         
            Alert($"Progress Log {id} deleted successfully", AlertType.success);
            
            // redirect to the details view
           return RedirectToAction(nameof(Details), new { Id = studentId });
            
        
        }

    }
}
