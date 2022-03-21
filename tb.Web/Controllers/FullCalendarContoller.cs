using Microsoft.AspNetCore.Mvc;
using tb.Web.ViewModels;


namespace tb.Web.Controllers
{
    public class FullCalendarController : BaseController
	{
		// [HttpGet]
		public IActionResult Index()
		{
			return View(new EventViewModel());
		}

        public IActionResult GetEvents(DateTime start, DateTime end)
        {
            var viewModel = new EventViewModel();
            var events = new List<EventViewModel>();
			start =  DateTime.Today.AddDays(-14);
			end = DateTime.Today.AddDays(-11);
			
			for (var i = 1; i <= 5; i++)
			{
				events.Add(new EventViewModel() 
				{ 
					id = i, 
					title = "Event " + i,
					start = start.ToString(), 
					end = end.ToString(), 
					allDay = false 
				});
				
				start = start.AddDays(7);
				end = end.AddDays(7);
			}
			
			return View(new EventViewModel());
            // return Json(events.ToArray());
        }
	}
}