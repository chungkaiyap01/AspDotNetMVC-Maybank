using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.UIConsole
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
