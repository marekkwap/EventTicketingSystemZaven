using System.Text.Json;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommandProcessor.Services.Interfaces;

public interface ITicketOperationService
{
    Task ExecuteOperation(JsonElement jsonEvent, object operationType);
}
