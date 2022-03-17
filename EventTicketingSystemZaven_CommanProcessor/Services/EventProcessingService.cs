using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_CommanProcessor.Services;

public class EventProcessingService : IEventProcessingService
{
    private readonly Dictionary<OperationType, IOperationProcessor> _operationProcessors;

    public EventProcessingService(IEnumerable<IOperationProcessor> operationProcessors)
    {
        _operationProcessors = operationProcessors.ToDictionary(p => p.OperationType);
    }

    public async Task ProcessEvent(BinaryData eventBody)
    {
        var jsonEvent = eventBody.ToObjectFromJson<JsonElement>(
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

        if (!(jsonEvent.TryGetProperty(nameof(EventOperation<object>.OperationType), out var operationTypeValue) && Enum.TryParse<OperationType>(operationTypeValue.GetString(), true, out var operationType)))
        {
            throw new ValidationException("Missing operation type.");
        }

        if (!_operationProcessors.TryGetValue(operationType, out var processor))
        {
            throw new ValidationException($"Not supported operation: \'{operationType}\'");
        }

        await processor.ExecuteOperation(jsonEvent);
    }
}
