using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyToDo.Shared.Contact;
using MyToDo.Shared.Parameters;

using MyToDoApi.Context;

using RestSharp;

namespace MyToDo.Service
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        public  Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
        public  Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);

        public  Task<ApiResponse> DeleteAsync(int id);
        public  Task<ApiResponse<TEntity>> GetFirstOrDefaultAsync(int id);
        public  Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameters parameter);


    }
}
