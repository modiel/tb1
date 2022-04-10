using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using tb.Core.Models;

namespace tb.Web.ViewModels
{
    public class AssignViewModel
    {
        // selectlist of Users (id, studentid, name)       
        public SelectList Users { set; get; }

        [Required]
        [Display(Name = "Select Parent/Caregiver")]
        public int StudentId { get; set; } //to link to student

        public int UserId { get; set; } // Collecting UserId  in Form

    
    }

}