using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace tb.Core.Models
{
    public class ProgressLog
    {     
       
       //progress log id
        public int Id { get; set; }      
   
        // date of lesson        
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // progress comments
        [StringLength(5000, MinimumLength = 5)]        
        public string Progress { get; set; }
        
        // EF Dependant Relationship Progress belongs to a Student
        public int StudentId { get; set; }

        // Navigation property
        public Student Student { get; set; }
 
    }
}