using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Clients.Interfaces
{
    public interface IHttpClientBase
    {
        Task<T> GetAsync<T>(string url, string accessKey = default);
    }
}
