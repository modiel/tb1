using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using tb.Core.Models;
using tb.Core.Services;
using tb.Web.ViewModels;

namespace tb.Web.Controllers
{
    [Authorize]
    public class QueryController : BaseController
    {
        private readonly IUserService svc;
  
        // configured via DI
        public QueryController(IUserService ss)
        {
            svc = ss;
        }

        public IActionResult Search()
        {
            return View("LitSearch");
        }
        
        // GET /Query/index
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Index()
        {
            // retrieve all OPEN Queries   
            var search  =  new QuerySearchViewModel {
                Queries = svc.SearchQueries(QueryRange.OPEN, "")
            };
            return View(search);
        }  

        // POST /Query/index   
        [HttpPost] 
    
        public IActionResult Index(QuerySearchViewModel search)
        {            
            // perform search query and assign results to viewmodel Queries property
            search.Queries = svc.SearchQueries(search.Range, search.Query);

            // build custom alert message if post           
            var alert = $"{search.Queries.Count} result(s) found searching '{search.Range}' Queries";
            if (search.Query != null && search.Query != "")
            {
                alert += $" for '{search.Query}'"; 
            }

            // display custom info alert
            Alert(alert, AlertType.info); 

            return View("Index", search);
        }     
             
        // GET/Query/{id}
        [Authorize(Roles="Admin,Tutor,Parent,Adult Student,Pupil")]
        public IActionResult Details(int id)
        {
            var query = svc.GetQuery(id);
            if (query == null)
            {
                Alert("Query Not Found", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(query);
        }

        // POST /query/close/{id}
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Close([Bind("Id, Resolution")] Query q)
        {
            // close query via service
            var query = svc.CloseQuery(q.Id, q.Resolution);
            if (query == null)
            {
                Alert("Query Not Found", AlertType.warning);                               
            }
            else
            {
                Alert($"Query {q.Id } closed", AlertType.info);  
            }

            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }
       
        // GET /Query/create
        [Authorize(Roles="Admin,Tutor,Parent,Adult Student,Pupil")]
        public IActionResult Create()
        {
            var students = svc.GetStudents();
            // populate viewmodel select list property
            var qvm = new QueryCreateViewModel {
                Students = new SelectList(students,"Id","FirstName","LastName") 
            };
            
            // render blank form
            return View( qvm );
        }
       
        // POST /Query/create
        [HttpPost]
        [Authorize(Roles="Admin,Tutor,Parent,Adult Student,Pupil")]
        public IActionResult Create(QueryCreateViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                svc.CreateQuery(qvm.StudentId, qvm.Issue);
     
                Alert($"Query Created", AlertType.info);  
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(qvm);
        }

           // GET / Query/delete/{id}
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult DeleteQuery(int id)
        {
            // load the query using the service
            var query = svc.GetQuery(id);
            if (query == null)
            {
                Alert("Query Not Found", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }
            
            // pass Query to view for deletion confirmation
            return View(query);
        }

        // POST /ProgressLog/delete/{id}
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult DeleteQueryConfirm(int id, int studentId)
        {
            // delete Query via service
            svc.DeleteQuery(id);
         
            Alert($"Query {id} deleted successfully", AlertType.success);
            
            // redirect to the details view
           return RedirectToAction(nameof(Index), new { Id = studentId });
            
        
        }


    }
}
