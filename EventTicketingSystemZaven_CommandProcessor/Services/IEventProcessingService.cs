using System;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommandProcessor.Services;

public interface IEventProcessingService
{
    Task ProcessEvent(BinaryData eventBody);
}
