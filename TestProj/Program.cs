﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataBaseApi.Models;
using ServerCashe.Services;

namespace TestProj
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

            try
            {
                await CreateBookAsync(new Book
                {
                    Barcode = 9785458756982
                });
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
