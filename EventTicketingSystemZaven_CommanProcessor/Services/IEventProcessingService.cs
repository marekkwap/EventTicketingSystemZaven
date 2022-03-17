using System;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommanProcessor.Services;

public interface IEventProcessingService
{
    Task ProcessEvent(BinaryData eventBody);
}
