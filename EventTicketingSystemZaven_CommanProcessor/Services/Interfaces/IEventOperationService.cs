using System.Text.Json;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;

public interface IEventOperationService
{
    Task ExecuteOperation(JsonElement jsonEvent, object operationType);
}
