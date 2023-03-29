using System;
using System.Collections.Generic;
using System.Text;

namespace MyToDo.Shared.Parameters
{
    public class ToDoParameter:QueryParameters
    {
        public int? Status { get; set; }
    }
}
