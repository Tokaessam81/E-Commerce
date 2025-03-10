using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ImageService : IImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> ExtractMetadataFromUrlAsync(string fileUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(fileUrl);

            return response;
        }
    }
}
