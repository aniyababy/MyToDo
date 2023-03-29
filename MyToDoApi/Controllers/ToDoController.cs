using Microsoft.AspNetCore.Mvc;

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

using MyToDoApi.Service;

namespace MyToDoApi.Controllers
{
    /// <summary>
    /// 待办事项控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService service)
        {
            _toDoService = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id)
        {
            return await _toDoService.GetSingleAsync(id);
        }

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] ToDoParameter param)
        {
            return await _toDoService.GetAllAsync(param);
        }

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto model)
        {
            return await _toDoService.AddAsync(model);
        }

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ToDoDto model)
        {
            return await _toDoService.UpdateAsync(model);
        }
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _toDoService.DeleteAsync(id);
        }
        [HttpGet]
        public async Task<ApiResponse> Summary()
        {
            return await _toDoService.Summary();
        }
    }
}
