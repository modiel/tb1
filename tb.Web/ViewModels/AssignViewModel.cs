using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using tb.Core.Models;

namespace tb.Web.ViewModels
{
    public class AssignViewModel
    {
        // selectlist of students (id, name)       
        public SelectList Users { set; get; }

        // Collecting UserId  in Form
        [Required]
        [Display(Name = "Select User")]
        public int StudentId { get; set; }

        public int UserId { get; set; }

    
    }

}