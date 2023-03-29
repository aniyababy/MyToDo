using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using MyToDoApi.Context;
using MyToDoApi.Context.Repository;

namespace MyToDoApi.Service
{
    public interface IToDoService:IBaseService<ToDoDto>
    {
        public Task<ApiResponse> GetAllAsync(ToDoParameter query);

        public Task<ApiResponse> Summary();
    }
}
