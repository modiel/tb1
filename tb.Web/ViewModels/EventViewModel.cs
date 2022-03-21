using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace tb.Web.ViewModels
{
	public class EventViewModel
	{
		public int id { get; set; }

        public String title { get; set; }

        public String start { get; set; }

        public String end {get; set; }

        public bool allDay { get; set; }
	}
}