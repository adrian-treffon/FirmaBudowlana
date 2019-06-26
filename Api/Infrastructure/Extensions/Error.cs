using Microsoft.AspNetCore.Http;

namespace FirmaBudowlana.Infrastructure.Error
{
    public static class Error
    {
        public static void AddAplicationError(this HttpResponse response,string message)
        {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Application-Control-Response-Headers", "Application-Error");
            response.Headers.Add("Application-Control-Allow-Origin", "*");
        }
    }
}
