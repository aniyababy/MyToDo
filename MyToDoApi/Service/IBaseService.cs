 using MyToDo.Shared.Parameters;
using MyToDoApi.Context;

namespace MyToDoApi.Service
{
    public interface IBaseService<T>
    {
        public Task<ApiResponse> GetAllAsync(QueryParameters query);

        public Task<ApiResponse> GetSingleAsync(int id);

        public Task<ApiResponse> AddAsync(T model);

        public Task<ApiResponse> UpdateAsync(T model);
        public Task<ApiResponse> DeleteAsync(int id);
    }
}
