using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.ServiceContract
{
    public interface IImageService
    {
        Task<HttpResponseMessage> ExtractMetadataFromUrlAsync(string fileUrl);
    }
}
