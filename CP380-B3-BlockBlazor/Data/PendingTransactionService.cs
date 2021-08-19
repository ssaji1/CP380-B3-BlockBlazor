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
    public class PendingTransactionService
    {
        // TODO: Add variables for the dependency-injected resources
        //       - httpClient
        //       - configuration
        //
        static HttpClient _HttpclientFactory;
        private readonly IConfiguration _IConfiguration;
        private readonly JsonSerializerOptions JSO = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        //
        // TODO: Add a constructor with IConfiguration and IHttpClientFactory arguments
        //
        public PendingTransactionService() { }
        public PendingTransactionService(IConfiguration config, IHttpClientFactory httpClient)
        {
            _HttpclientFactory = httpClient.CreateClient();
            _IConfiguration = config.GetSection("PayloadService");
        }
        //
        // TODO: Add an async method that returns an IEnumerable<Payload> (list of Payloads)
        //       from the web service
        //
        public async Task<IEnumerable<Payload>> ListPayloads()
        {
            var responce = await _HttpclientFactory.GetAsync(_IConfiguration["url"]);

            //JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            if (responce.IsSuccessStatusCode)
            {
                
                return await JsonSerializer.DeserializeAsync<IEnumerable<Payload>>(
                        await responce.Content.ReadAsStreamAsync(), JSO
                    );
            }

            return Array.Empty<Payload>();
        }
        //
        // TODO: Add an async method that returns an HttpResponseMessage
        //       and accepts a Payload object.
        //       This method should POST the Payload to the web API server
        //
        public async Task<HttpResponseMessage> SubmitPayload(Payload payload)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload, JSO),
                System.Text.Encoding.UTF8,
                "application/json"
                );

            var res = await _HttpclientFactory.PostAsync(_IConfiguration["url"], content);
            return res;
        }
    }
}
