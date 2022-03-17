using System.Text.Json;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommandProcessor.Services.Interfaces;

public interface IEventOperationService
{
    Task ExecuteOperation(JsonElement jsonEvent, object operationType);
}
