
using System.Collections.Generic;
using tb.Core.Models;

namespace tb.Web.ViewModels
{   
    public class QuerySearchViewModel
    {
      
      

      //result set
      public IList<Query> Queries { get; set; } = new List<Query>();//initialise by default
      public string Query { get; set; } = "";// string property called Query with default of "" 

      public QueryRange Range { get; set; } = QueryRange.OPEN; //OPEN, CLOSED, ALL
      // QueryRange property called Range with default of QueryRange.OPEN
      
      

    }
}
 