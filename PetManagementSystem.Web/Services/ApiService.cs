using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ApiService(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _options);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[API POST] {endpoint} → {(int)response.StatusCode}: {responseContent}");

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

            try 
            {
                // Always deserialize — even error responses contain useful JSON (success:false, message, errors)
                return JsonSerializer.Deserialize<TResponse>(responseContent, _options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API DESERIALIZATION ERROR]: {ex.Message} \n {responseContent}");
                return default;
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync(endpoint, content);
            if (!response.IsSuccessStatusCode) return default;
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent, _options);
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }
}
