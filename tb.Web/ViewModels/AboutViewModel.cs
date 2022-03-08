 using System;
    
    namespace tb.Web.ViewModels
    {
       public class AboutViewModel
       {
          public string Title { get; set; }
          public string Message { get; set; }
          public DateOnly Formed { get; set; } = new System.DateOnly(2022,02,08);
         //  public string FormedString => Formed.ToLongDateString();             
         //  public int Days => (DateTime.Now - Formed).Days;
       }
    }
    