using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsservice.Domain.Infrastructure.ExternalServices;
using Newtonsoft.Json;

namespace Microsservice.Infrastructure.ExternalServices
{
    public class MicrosserviceExternalService: IMicrosserviceExternalService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory clientFactory;
        public MicrosserviceExternalService(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            this.clientFactory = clientFactory;
        }
        public async Task<object> Delete(object query)
        {
            var client = clientFactory.CreateClient("project-name");
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{client.BaseAddress}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
                if (error is null)
                    throw new System.NotImplementedException();
                else
                    throw new System.NotImplementedException();
            }
            return JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
        }
        public async Task<object> Get(object query)
        {
            var client = clientFactory.CreateClient("project-name");

            IDictionary<string, string> queryString = JsonConvert.DeserializeObject<IDictionary<string, string>>
                                                                     (JsonConvert.SerializeObject(query));

            var requestUri = QueryHelpers.AddQueryString($"{client.BaseAddress}", queryString);
            
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                return null;

            var message = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(message))
                return null;

            var result = JsonConvert.DeserializeObject<object>(message);
            
            return result;
        }
        public async Task<object> Post(object query)
        {
            var client = clientFactory.CreateClient("project-name");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseAddress}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            var response = await client.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
                if (error is null)
                    throw new System.NotImplementedException();
                else
                    throw new System.NotImplementedException();            
            }
            return JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
        }
        public async Task<object> Put(object query)
        {
            var client = clientFactory.CreateClient("project-name");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{client.BaseAddress}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
                if (error is null)
                    throw new System.NotImplementedException();
                else
                    throw new System.NotImplementedException();  
            }
            return JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
        }
    }        
    
}