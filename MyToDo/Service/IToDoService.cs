using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyToDo.Common.Models;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using MyToDoApi.Context;

namespace MyToDo.Service
{
    public interface IToDoService:IBaseService<ToDoDto>
    {
        public Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter toDoParameter);

        public Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
