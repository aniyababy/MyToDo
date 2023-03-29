using AutoMapper;

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDoApi.Context.UnitOfWork;

using MyToDoApi.Context;
using Microsoft.AspNetCore.Mvc;

namespace MyToDoApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync([FromBody]UserDto model)
        {
            var user = _mapper.Map<User>(model);
            await _unitOfWork.GetRepository<User>().InsertAsync(user);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new ApiResponse(true, user);
            }
            else
            {
                return new ApiResponse("添加数据失败");
            }



        }
        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<User>();
            var model = await repository.GetFirstOrDefaultAsync(predicate: s => s.Id == id);
            if (model != null)
            {
                repository.Delete();
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse("success");
            }
            return new ApiResponse("failed");
        }
        public async Task<ApiResponse> GetAllAsync(QueryParameters parameter)
        {
            var repository = _unitOfWork.GetRepository<User>();
            var models = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.UserName.Equals(parameter.Search)
            , pageIndex: parameter.PageIndex
            , pageSize: parameter.PageSize
            , orderBy: x => x.OrderByDescending(t => t.CreateTime));
            return new ApiResponse(true, models);
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<User>();
            var model = await repository.GetFirstOrDefaultAsync(predicate: (s) => s.Id == id);
            if (model != null)
            {
                return new ApiResponse(true, model);
            }
            return new ApiResponse("没有该用户");
        }

        public async Task<ApiResponse> UpdateAsync(UserDto model)
        {
            var todo = _mapper.Map<User>(model);

            var repository = _unitOfWork.GetRepository<User>();
            var todomodel = await repository.GetFirstOrDefaultAsync(predicate: s => s.Id == todo.Id);
            if (todomodel != null)
            {
                todomodel.UserName = model.UserName;
                todomodel.Password = model.Password;
                repository.Update(todomodel);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse(true, todo);
            }
            else return new ApiResponse("异常！请检查您输入的id是否正确");
        }
    }
}
