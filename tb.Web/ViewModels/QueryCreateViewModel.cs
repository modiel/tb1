using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using tb.Core.Models;

namespace tb.Web.ViewModels
{
    public class QueryCreateViewModel
    {
        // selectlist of students (id, name)       
        public SelectList Students { set; get; }

        // Collecting StudentId and Issue in Form
        [Required]
        [Display(Name = "Select Student")]
        public int StudentId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Issue { get; set; }
    }

}
