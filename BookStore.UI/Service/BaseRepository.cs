using BookStore.UI.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookStore.UI.Service
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory client;
        public BaseRepository(IHttpClientFactory client)
        {
            this.client = client;
        }
        public async Task<bool> Create(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (obj == null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj));
            var client = this.client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<bool> Delete(string url, int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(string url, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> Get(string url)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string url, T obj)
        {
            throw new NotImplementedException();
        }
    }
}
