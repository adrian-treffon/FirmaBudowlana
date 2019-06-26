using Microsoft.AspNetCore.Http;
using System;

namespace Api.Infrastructure.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            Console.WriteLine("HALOOO");
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Response-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}