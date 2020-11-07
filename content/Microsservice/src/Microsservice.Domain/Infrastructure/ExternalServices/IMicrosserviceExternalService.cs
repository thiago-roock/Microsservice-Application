using System.Threading.Tasks;

namespace Microsservice.Domain.Infrastructure.ExternalServices
{
    public interface IMicrosserviceExternalService
    {
         Task<object> Get(object query);
         Task<object> Post(object query);
         Task<object> Put(object query);
         Task<object> Delete(object query);
    }
}