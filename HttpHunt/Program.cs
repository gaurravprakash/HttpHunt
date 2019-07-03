using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace HttpHunt
{
    class Program
    {
        static void Main(string[] args)
        {
            StageFour();
        }

        static void StageOne()
        {
            var response = APIHelper.Get(Constants.URI + Constants.INPUT_ENDPOINT);
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic jo = JArray.Parse(result);
            int c = 0;
            foreach (var item in jo)
            {
                c++;
            }

            var bodyString = new
            {
                output = new { count = c }
            };

            var responseOut = JsonConvert.SerializeObject(bodyString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var next = APIHelper.Post(Constants.URI + Constants.OUTPUT_ENDPOINT, responseOut);
            Console.WriteLine(next.Content.ReadAsStringAsync().Result);
            Console.ReadKey();
        }

        static void StageTwo()
        {
            var response = APIHelper.Get(Constants.URI + Constants.INPUT_ENDPOINT);
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic jo = JArray.Parse(result);
            int c = 0;
            foreach (var item in jo)
            {
                var s = item["startDate"].Value;
                var e = item["endDate"].Value;
                if (IsProductActive(s, e))
                {
                    c++;
                }
            }

            var bodyString = new
            {
                output = new { count = c }
            };

            var responseOut = JsonConvert.SerializeObject(bodyString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var next = APIHelper.Post(Constants.URI + Constants.OUTPUT_ENDPOINT, responseOut);
            Console.WriteLine(next.Content.ReadAsStringAsync().Result);
            Console.ReadKey();
        }


        static bool IsProductActive(string stDt, string edDt)
        {
            var today = DateTime.Now;
            var startDt = DateTime.Parse(stDt);
            var endDt = !string.IsNullOrEmpty(edDt) ? DateTime.Parse(edDt) : DateTime.MaxValue;
            if (startDt <= today && endDt >= today)
            {
                return true;
            }
            return false;
        }

        static void StageThree()
        {
            Dictionary<string, int> activeCat = new Dictionary<string, int>();
            var response = APIHelper.Get(Constants.URI + Constants.INPUT_ENDPOINT);
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JArray.Parse(result);
            foreach (var item in items)
            {
                var s = item["startDate"].Value;
                var e = item["endDate"].Value;
                if (IsProductActive(s, e))
                {
                    var cat = item["category"].Value;
                    if (activeCat.ContainsKey(cat))
                    {
                        activeCat[cat] = activeCat[cat] + 1;
                    }
                    else
                    {
                        activeCat.Add(cat, 1);
                    }
                }
            }

            var bodyString = new
            {
                output = activeCat
            };
            var responseOut = JsonConvert.SerializeObject(bodyString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var next = APIHelper.Post(Constants.URI + Constants.OUTPUT_ENDPOINT, responseOut);
            Console.WriteLine(next.Content.ReadAsStringAsync().Result);
            Console.ReadKey();
        }

        static void StageFour()
        {
            var response = APIHelper.Get(Constants.URI + Constants.INPUT_ENDPOINT);
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JArray.Parse(result);
            int value  = 0;
            foreach (var item in items)
            {
                var s = item["startDate"].Value;
                var e = item["endDate"].Value;
                if (IsProductActive(s, e))
                {
                    value += Convert.ToInt32(item["price"].Value);
                }
            }

            var bodyString = new
            {
                output = new { totalValue = value }
            };

            var responseOut = JsonConvert.SerializeObject(bodyString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var next = APIHelper.Post(Constants.URI + Constants.OUTPUT_ENDPOINT, responseOut);
            Console.WriteLine(next.Content.ReadAsStringAsync().Result);
            Console.ReadKey();
        }
    }
}
