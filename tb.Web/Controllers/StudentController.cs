using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginViewModel m)
        {
            var student = svc.AuthenticateStudent(m.Id, m.Password);
            // check if login was unsuccessful and add validation errors
            if (student == null)
            {
                ModelState.AddModelError("Email", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }

            // Login Successful, so sign user in using cookie authentication
            await SignInCookie(student);

            Alert("Successfully Logged in", AlertType.info);

            return RedirectToAction(nameof(Details));
        }

        
        // GET /student/index
        [Authorize]
        public IActionResult Index()
        {  
            IList<Student> students = new List<Student>();

            var userId = GetSignedInUserId(); //define route based on role of who is signed in        

            if (User.IsInRole(Role.Tutor.ToString())) //this could be admin if using hub system
            {
                students = svc.GetStudents();
            } 
            else if (User.IsInRole(Role.Pupil.ToString()))
            {
                students.Add(svc.GetStudentByUserId(userId));
                
            }          
            else // parent
            {
                students = svc.GetStudentsForUser(userId);
            }
            return View(students);
        }

        // GET /student/index/ordered
        [Authorize]
        public IActionResult OrderedIndex(string order = null)
        {
        IList<Student> students = new List<Student>();

        
         if (User.IsInRole(Role.Tutor.ToString())) //this could be admin if using hub system
            {
                students = svc.GetStudents(order);
            } 
            return View(students);

        }
        

        [Authorize(Roles="Pupil,Parent")]
        public IActionResult UserDetails()
         {
            var id = GetSignedInUserId();
            var student = svc.GetStudentByUserId( id );
            if (student == null) {
                Alert("User does not have a student record",AlertType.warning);
                return Redirect("/");
            }
            return View("Details",student);
        }


        // GET /student/details/{id}
        [Authorize]
        public IActionResult Details(int id)
        {

            // retrieve the student with specified id from the service
            var s = svc.GetStudentById(id);
            var user = svc.GetUser(GetSignedInUserId());
            var UserStudents = svc.GetStudentsForUser(user.Id);

            // check if s is null and return NotFound()
            if (s == null)
            {
                Alert("Student Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index), new { Id = id });
            }
            
            //additonal authorisation checks
            if (user.Role == Role.Tutor || UserStudents.Contains(s)|| user.Id == s.UserId)
            {
                // pass student as parameter to the view
            return View(s);
            }
            else 
             {
                Alert("Student cannot be accessed", AlertType.warning);
                return RedirectToAction(nameof(Index), new { Id = id });
            }
            

            
        }

        // GET: /student/create
        public IActionResult Create()
        {
            var userId = GetSignedInUserId();
            var user = svc.GetUser(userId);

           if( user.Adult != true )
            {
                
                Alert($"Creating may only be peformed by students aged over 18", AlertType.warning); 
                return RedirectToAction("About","Home");
            
            }
            // display blank form to create a student
            var s = new Student();
            return View(s);
        }

        // POST /student/create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Student student) 
        {
            var userId = GetSignedInUserId();
            var user = svc.GetUser(userId);

            // check email is unique for this student
            if (!svc.IsEmailAvailable(student.User.Email, student.Id))
            {
                // add manual validation error
                ModelState.AddModelError(nameof(student.User.Email),"The email address is already in use");  
            }

            // validate student
            if (ModelState.IsValid)
            {
                // pass data to service to store 
                student = svc.AddStudent(student);
                var userStudent = svc.AssignUserToStudent(user.Id,student.Id);
                Alert($"Student {student.Name} created successfully", AlertType.info);       
                
                return RedirectToAction(nameof(Index));
            }
            // redisplay the form for editing as there are validation errors
            return View(student);
        }

        // GET: /student/create
       
        public IActionResult CreateAdult()
        {
            // display blank form to create a student
            var s = new Student();
            return View(s);
        }

        // POST /student/create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult CreateAdult (Student student) 
        {
            
            // check email is unique for this student
            if (!svc.IsEmailAvailable(student.User.Email, student.Id))
            {
                // add manual validation error
                ModelState.AddModelError(nameof(student.User.Email),"The email address is already in use");  
            }

            

            // validate student
            if (ModelState.IsValid)
            {
                // pass data to service to store 
                student = svc.AddStudent(student);
                
                if (student == null) {
                Alert("There was a problem Registering. Please try again", AlertType.warning);
                return View(student);
            }
             if (student.User.Adult != true) {
                Alert("You must be over 18 to register", AlertType.warning);
                return View(student);
            }

            Alert("Successfully Registered. Now login", AlertType.info);

            return Redirect("/");
            }
            // redisplay the form for editing as there are validation errors
            return View(student);
        }


        // Change Password
        [Authorize]
        public IActionResult UpdatePassword()
        {
            // use BaseClass helper method to retrieve Id of signed in user 
            var user = svc.GetUser(GetSignedInUserId());
            var passwordViewModel = new UserPasswordViewModel { 
                Id = user.Id, 
                Password = user.Password, 
                PasswordConfirm = user.Password, 
            };
            return View(passwordViewModel);
        }
        

        // GET /student/edit/{id}
        [Authorize]
        public IActionResult Edit([Bind("Password")]int id) 
        {   
           //check user and what load students they have access to
            var user = svc.GetUser(GetSignedInUserId());
            var UserStudents = svc.GetStudentsForUser(user.Id);
        
            // load the student using the service
            var s = svc.GetStudentById(id);
            
        
            // check if s is null and return NotFound()
            if (s == null)
            {
                Alert($"No such student {id}", AlertType.warning); 
                return RedirectToAction(nameof(Index), new { Id = id });
            }   

            //check student to edit and signed in user are over 18
            if( s.User.Adult != true && user.Adult != true)
            {
                
                Alert($"Edits may only be peformed by students aged over 18", AlertType.warning); 
                return RedirectToAction(nameof(Details), new { Id = id });
            
            }
            
            //additional checks (prevents access through address bar)
            if (user.Role == Role.Tutor || UserStudents.Contains(s) || user.Id == s.UserId)
            {
            // pass student as parameter to the view
            return View(s);
            }

            else 
            {
                Alert("Student cannot be accessed", AlertType.warning);
                return RedirectToAction(nameof(Index), new { Id = id });
            }

            
        }

        // POST /student/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int id, Student s)
        {
            // check email is unique for this student
            if (!svc.IsEmailAvailable(s.User.Email, s.User.Id))
            {
                // add manual validation error
                ModelState.AddModelError("User.Email","The email address is already in use");
            } 
            
            // validate student
            if (ModelState.IsValid)
            {
                // pass data to service to update
                svc.UpdateStudent(s);
                Alert($"Student details for {s.Name} saved", AlertType.info);
                return RedirectToAction(nameof(Details), new { Id = id }); 
            }

            // redisplay the form for editing as validation errors
            return View(s);
        }

        // GET / student/delete/{id}
        [Authorize(Roles="Tutor")]       
        public IActionResult Delete(int id)
        {
            // load the student using the service
            var s = svc.GetStudentById(id);
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
        [Authorize(Roles="Tutor")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm( Student s, User u)
        {   
            var student = svc.GetStudentById(s.Id);
            // delete student via service
            svc.DeleteStudent(s, u);
         
            Alert($"Student {student.Name} deleted successfully", AlertType.success);
            // redirect to the index view
            return RedirectToAction(nameof(Index), new { Id = s.Id });
        }

       

        // GET /Student/Assign
        [Authorize(Roles="Tutor,Parent")]
        public IActionResult Assign()
        {

            var users = svc.GetUsers();

            var parents = users.Where( u => u.Role == Role.Parent)
                                .Select( u => u) ;

            // populate viewmodel select list property
            var avm = new AssignViewModel {
                Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(parents,"Id","Name") 
            };
            
            // render blank form
            return View( avm );
        }
       
        // POST /Student/assign
        [HttpPost]
        [Authorize(Roles="Tutor,Parent")]
        public IActionResult Assign(int id, AssignViewModel avm)
        {
            if (ModelState.IsValid)
            {   var student = svc.GetStudentByUserId(id);
                var assign = svc.AssignUserToStudent(avm.UserId, id);
     
                Alert($"Student {id} Assigned to User {avm.UserId}", AlertType.info);  
                return RedirectToAction(nameof(Index), new { UserId = avm.UserId, StudentId = avm.StudentId, Id= id });
            }
            
            // redisplay the form for editing
            return View(avm);
        }

          // GET /student/createQuery
        [Authorize(Roles="Tutor,Parent,Pupil")]
        public IActionResult CreateQuery(int id)
        {
            var s = svc.GetStudentById(id);
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
        [Authorize(Roles="Tutor,Parent,Pupil")]
        public IActionResult CreateQuery([Bind("StudentId, Issue")]QueryCreateViewModel qvm)
        {
            var s = svc.GetStudentById(qvm.StudentId);
             // check the returned student is not null and if so return NotFound()
            if (s == null)
            {
                Alert($"No such student {qvm.StudentId}", AlertType.warning);          
                return RedirectToAction(nameof(Index));
            }  
        
            // create the Query view model and populate the StudentId property
            svc.CreateQuery(qvm.StudentId, qvm.Issue);
            Alert($"Query from {s.Name} created successfully", AlertType.success);   

            return RedirectToAction("Details", new { Id = qvm.StudentId });
        }

        // GET /Student/createProgressLog
        [Authorize(Roles="Tutor")]
        public IActionResult CreateProgressLog(int id)
        {
            var s = svc.GetStudentById(id);
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
        [Authorize(Roles="Tutor")]
        public IActionResult CreateProgressLog (ProgressLog pl)
        {
            var s = svc.GetStudentById(pl.StudentId);
             // check the returned Student is not null and if so alert
            if (s == null)
            {
                Alert($"No such Student {pl.StudentId}", AlertType.warning);          
                return RedirectToAction(nameof(Details));
            }  
            
            Alert($"Progress Log for {s.Name} created successfully", AlertType.success);   
            // create the ProgressLog view model and populate the StudentId property
            svc.AddProgressLog(new ProgressLog { StudentId = pl.StudentId, Progress = pl.Progress});
 
            return RedirectToAction(nameof(Details), new { Id = pl.StudentId });
        }

        // GET /progressLog/edit/{id}
        [Authorize(Roles="Tutor")]
        public IActionResult EditProgressLog(int id)
        {
            // load the student using the service
            var pl = svc.GetProgressLogById(id);

            // check if s is null and return NotFound()
            if (pl == null)
            {
                Alert($"No such progress log {id}", AlertType.warning); 
                return RedirectToAction(nameof(Details), new { Id = pl.StudentId });
            }   

            // pass student to view for editing
            return View(pl);
        }

        // POST /progressLog/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Tutor")]
        public IActionResult EditProgressLog(int id, ProgressLog pl)
        {
            
            // validate progressLog
            if (ModelState.IsValid)
            {
                // pass data to service to update
                svc.UpdateProgressLog(pl);
                Alert($"Progress log {id} changes saved", AlertType.info);
                
                return RedirectToAction(nameof(Details), new { Id = pl.StudentId });
                
            }

            // redisplay the form for editing as validation errors
            return View(pl);
        }
     

           // GET / ProgressLog/delete/{id}
        [Authorize(Roles="Tutor")]
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
        [Authorize(Roles="Tutor")]
        public IActionResult DeleteProgressLogConfirm(int id, int studentId)
        {
            // delete ProgressLog via service
             svc.DeleteProgressLog(id);
         
            Alert($"Progress Log {id} deleted successfully", AlertType.success);
            
            // redirect to the details view
           return RedirectToAction(nameof(Details), new { Id = studentId });
            
        
        }
         // Sign user in using Cookie authentication scheme
        private async Task SignInCookie(Student student)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                AuthBuilder.BuildClaimsPrincipal(student)
            );
        }
        

    }
}
