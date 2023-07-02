using System.Net.Http.Json;

namespace AttendanceManagerGenerator.Handlers
{
    public class RequestHandler
    {
        private string _controller;
        private HttpClient _http;
        public RequestHandler(HttpClient httpClient, string controller)
        {
            _controller = controller;
            _http = httpClient;
        }

        public async Task<T?> GetAsync<T>(string method, string newController = null) where T : class
        {
            try
            {
                return newController == null ? await _http.GetFromJsonAsync<T>($"api/{_controller}/{method}") : await _http.GetFromJsonAsync<T>($"api/{newController}/{method}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;

        }
        public async Task<TOut?> PatchAsync<TIn, TOut>(string method, TIn param) where TIn : class
        {
            try
            {
                var response = await _http.PatchAsJsonAsync($"api/{_controller}/{method}", param);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TOut>();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;

        }
        public async Task<TOut?> PostAsync<TIn, TOut>(string method, TIn param) where TIn : class
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"api/{_controller}/{method}", param);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TOut>();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;

        }
    }
}
