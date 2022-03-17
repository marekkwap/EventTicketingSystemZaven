using System.Text.Json;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;

public interface IOperationProcessor
{
    OperationType OperationType { get; }

    Task ExecuteOperation(JsonElement operationBody);
}
