using System;
using System.Collections.Generic;
using System.Text;

namespace MyToDo.Shared.Parameters
{
    public class QueryParameters
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
