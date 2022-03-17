using System.Text.Json;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;

public interface ITicketOperationService
{
    Task ExecuteOperation(JsonElement jsonEvent, object operationType);
}
