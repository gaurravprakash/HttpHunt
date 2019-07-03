using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HttpHunt
{
    public class APIHelper
    {
        static HttpClient httpClient = new HttpClient();
        public static HttpResponseMessage Get(string uri)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("userId", Constants.USERID);
            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            return response;
        }

        public static HttpResponseMessage Post(string uri, string bodyString)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("userId", Constants.USERID);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.PostAsync(uri, new StringContent(bodyString, Encoding.UTF8, "application/json")).Result;
            return response;
        }
    }
}
