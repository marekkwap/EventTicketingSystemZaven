using EventTicketingSystemZaven_Api.Models;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public interface IEventPublisher
    {
        Task PublishEvent<T>(EventOperation<T> eventOperation);
    }
}
