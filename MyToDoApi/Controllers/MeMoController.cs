using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDoApi.Service;

namespace MyToDoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeMoController : ControllerBase
    {
        private readonly IMeMoService _meMoService;
        public MeMoController(IMeMoService meMoService)
        {
            _meMoService = meMoService;
        }
        [HttpGet]
        public async Task<Service.ApiResponse> Get(int id)
        {
            return await _meMoService.GetSingleAsync(id);
        }

        [HttpGet]
        public async Task<Service.ApiResponse> GetAll([FromQuery] QueryParameters param)
        {
            return await _meMoService.GetAllAsync(param);
        }

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MeMoDto model)
        {
            return await _meMoService.AddAsync(model);
        }

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MeMoDto model)
        {
            return await _meMoService.UpdateAsync(model);
        }
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _meMoService.DeleteAsync(id);
        }
    }
}
