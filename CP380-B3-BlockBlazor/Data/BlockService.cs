using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CP380_B1_BlockList.Models;

namespace CP380_B3_BlockBlazor.Data
{
    public class BlockService
    {
        // TODO: Add variables for the dependency-injected resources
        //       - httpClient
        //       - configuration
        //
        static HttpClient _HttpclientFactory;
        private readonly IConfiguration _IConfiguration;
        //
        // TODO: Add a constructor with IConfiguration and IHttpClientFactory arguments
        //
        public BlockService(IConfiguration config, IHttpClientFactory httpClient)
        {
            _HttpclientFactory = httpClient.CreateClient();
            _IConfiguration = config.GetSection("BlockService");
        }
        //
        // TODO: Add an async method that returns an IEnumerable<Block> (list of Blocks)
        //       from the web service
        //
        public async Task<IEnumerable<Block>> ListBlocks()
        {
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, _IConfiguration["url"]);
            HttpRequestMessage request = httpRequestMessage;
            using (HttpResponseMessage httpResponseMessage = await _HttpclientFactory.SendAsync(request))
            {
                var response = httpResponseMessage;
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<IEnumerable<Block>>(responseStream);
                }
            }
            return Array.Empty<Block>();
        }
        private readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
        public async Task<Block> SubmitNewBlockAsync(Block block)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(block, JsonSerializerOptions),
               System.Text.Encoding.UTF8,
               "application/json"
               );

            HttpResponseMessage res = await _HttpclientFactory.PostAsync(_IConfiguration["url"], content);
            if (res.IsSuccessStatusCode)
                return block;
            else
                return null;
        }
    }
}


