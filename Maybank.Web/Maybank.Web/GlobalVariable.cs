using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Maybank.Web
{
    public class GlobalVariable
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariable()
        {
            WebApiClient.BaseAddress = new Uri("http://localhost:49962/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}