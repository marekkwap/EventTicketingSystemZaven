using System.Threading.Tasks;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_Api.Services;

public interface IEventPublisher
{
    Task PublishEvent<T>(EventOperation<T> eventOperation, string partitionId);
}
