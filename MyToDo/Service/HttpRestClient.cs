using System;
using System.Threading.Tasks;

using MyToDoApi.Context;

using Newtonsoft.Json;

using RestSharp;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;
        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();
        }
        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;

            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else
                return new ApiResponse()
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
        }
        //public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        //{
        //    var request = new RestRequest(apiUrl + baseRequest.Route, baseRequest.Method);

        //    if (baseRequest.Parameter != null)
        //        request.AddParameter(JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

        //    var response = await client.ExecuteAsync(request);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

        //    else
        //        return new ApiResponse<T>()
        //        {
        //            Status = false,
        //            Message = response.ErrorMessage
        //        };
        //}

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;

            if (baseRequest.Parameter != null)
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };

        }
    }
}
