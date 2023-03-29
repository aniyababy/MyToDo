using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DryIoc;

using MyToDo.Shared.Contact;
using MyToDo.Shared.Parameters;

using MyToDoApi.Context;

using RestSharp;

namespace MyToDo.Service
{
    public class BaseService<TEnity> :IBaseService<TEnity> where TEnity : class 
    {
        private readonly HttpRestClient _client;
        private readonly string serviceName;
        public BaseService(HttpRestClient client, string serviceName)
        {
            _client = client;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEnity>> AddAsync(TEnity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Add";
            request.Parameter = entity;
            return await _client.ExecuteAsync<TEnity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"api/{serviceName}/Delete?id={id}";
            return await _client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<PagedList<TEnity>>> GetAllAsync(QueryParameters parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetAll?PageIndex={parameter.PageIndex}" +
                $"&PageSize={parameter.PageSize}" +
                $"&Search={parameter.Search}";
            return await _client.ExecuteAsync<PagedList<TEnity>>(request);
        }

        public async Task<ApiResponse<TEnity>> GetFirstOrDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/Get?id={id}";
            return await _client.ExecuteAsync<TEnity>(request);
        }

        public async Task<ApiResponse<TEnity>> UpdateAsync(TEnity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Update";
            request.Parameter = entity;
            return await _client.ExecuteAsync<TEnity>(request);
        }
    }
}
