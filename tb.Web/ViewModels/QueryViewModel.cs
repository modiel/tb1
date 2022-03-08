using System;


namespace tb.Web.Models
{
    public class QueryViewModel
    {
        public int Id { get; set; }
       
        public string Issue { get; set; }
        public string Resolution { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ResolvedOn { get; set; } = DateTime.MinValue;
    
        public bool Active { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; } 
    }

}
