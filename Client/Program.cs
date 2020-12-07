using PrintStationWebApi.Models.BL;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "xxxxxxxxxxxxxxxxxxxx");


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

        static void Test()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("[your request url string]"),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "X-Version", "1" }, // HERE IS HOW TO ADD HEADERS,
                    { HttpRequestHeader.Authorization.ToString(), "[your authorization token]" },
                    { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" },//use this content type if you want to send more than one content type
                },
                Content = new MultipartContent
                {
                }
            };
        }
    }
}
