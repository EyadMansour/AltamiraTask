using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.HttpClient
{
    public static class HttpClientHelper<TResponseType>
    {
        private static System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        private static TResponseType _result = default(TResponseType);
        public static async Task<TResponseType> GetAsync(string url)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                var z = await response.Content.ReadAsStringAsync();
                
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) => {
                    if (x.IsFaulted)
                        throw x.Exception;
                    _result = JsonConvert.DeserializeObject<TResponseType>(x.Result);
                });
                return _result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static async Task<TResponseType> PostAsync<TPostObjectType>(string url, TPostObjectType postObject)
        {
            try
            {
                var json = JsonConvert.SerializeObject(postObject);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = null;
                using (var response = await httpClient.PostAsync(url, data).ConfigureAwait(false))
                {
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) => {
                        if (x.IsCanceled)
                            throw x.Exception;
                        _result = JsonConvert.DeserializeObject<TResponseType>(x.Result);
                    });
                }
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<TResponseType> UpdateAsync<TPostObjectType>(string url, TPostObjectType postObject)
        {
            try
            {
                using (var response = await httpClient.PutAsJsonAsync(url, postObject))
                {
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                        {
                            throw x.Exception;
                        }
                        _result = JsonConvert.DeserializeObject<TResponseType>(x.Result);
                    });
                }
                return _result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
