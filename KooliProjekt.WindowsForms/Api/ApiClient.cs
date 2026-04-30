using System.Net.Http.Json;
using Newtonsoft.Json;

namespace KooliProjekt.WindowsForms.Api
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Projects/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<Project>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<Project>>>(body);
            return result;
        }

        public async Task<OperationResult> Save(Project project)
        {
            var url = _baseUrl + "Save";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(project)
            };

            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }

        public async Task<OperationResult> Add(Project project)
        {
            var url = _baseUrl + "Add";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(project)
            };

            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete";

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = id })
            };

            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }
    }
}