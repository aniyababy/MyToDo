using AutoMapper;

using MyToDo.Shared.Dtos;
using MyToDoApi.Context.UnitOfWork;

using MyToDoApi.Context;
using MyToDo.Shared.Parameters;

namespace MyToDoApi.Service
{
    /// <summary>
    /// 备忘录的实现
    /// </summary>
    public class MeMoService : IMeMoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MeMoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(MeMoDto model)
        {
            var todo = _mapper.Map<Memo>(model);
            await _unitOfWork.GetRepository<Memo>().InsertAsync(todo);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, todo);
            }
            else
            {
                return new ApiResponse("添加数据失败");
            }



        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var model = await repository.GetFirstOrDefaultAsync(predicate: s => s.Id == id);
            if (model != null)
            {
                repository.Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse(true,"success");
            }
            return new ApiResponse("failed");
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameters parameter)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            if (string.IsNullOrEmpty(parameter.Search))
            {
                var models = await repository.GetAll().ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
                return new ApiResponse(true, models);
            }
            else
            {
                var models = await repository.GetPagedListAsync(predicate: x => x.Title.Contains(parameter.Search), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize);
                return new ApiResponse(true, models);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var model = await repository.GetFirstOrDefaultAsync(predicate: (s) => s.Id == id);
            if (model != null)
            {
                return new ApiResponse(true, model);
            }
            return new ApiResponse("没有该用户");
        }

        public async Task<ApiResponse> UpdateAsync(MeMoDto model)
        {
            var todo = _mapper.Map<Memo>(model);

            var repository = _unitOfWork.GetRepository<Memo>();
            var todomodel = await repository.GetFirstOrDefaultAsync(predicate: s => s.Id == todo.Id);
            if (todomodel != null)
            {
                todomodel.Title = todo.Title;
                todomodel.UpdateTime = DateTime.Now;
                todomodel.Content = todo.Content;
                repository.Update(todomodel);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse(true, todo);
            }
            else return new ApiResponse("异常！请检查您输入的id是否正确");
        }
    }
}
