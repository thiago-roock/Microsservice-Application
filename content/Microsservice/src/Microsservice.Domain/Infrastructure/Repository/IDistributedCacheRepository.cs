using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsservice.Domain.Infrastructure.Repository.Models;

namespace Microsservice.Domain.Infrastructure.Repository
{
    public interface IDistributedCacheRepository
    {
        Task InsertItemCache(string keyIdentify, MicrosserviceExternalServiceModel cache);

        Task<MicrosserviceExternalServiceModel> GetItemCache(string keyIdentify);
    }
}
