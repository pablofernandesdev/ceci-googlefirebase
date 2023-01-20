using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services.External
{
    public class FirebaseService : IFirebaseService
    {
        private readonly HttpClient _httpClient;

        public FirebaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultResponse> SendNotificationAsync(string token, string title, string body)
        {
            var response = new ResultResponse();

            var data = new
            {
                to = token,
                notification = new
                {
                    body = body,
                    title = title,
                },
                priority = "high"
            };

            var json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync("/fcm/send", httpContent);

            response.StatusCode = result.StatusCode;

            if (!result.IsSuccessStatusCode)
            {
                response.Message = await result.Content.ReadAsStringAsync();
            }

            return response;
        }
    }
}
