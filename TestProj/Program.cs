using PrintStationWebApi.Models.BL;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TestProj
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {

        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

            }
            catch
            {

            }
        }
        static async Task<Uri> CreateBookAsync(Book book)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Cashe", book);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
    }
}
