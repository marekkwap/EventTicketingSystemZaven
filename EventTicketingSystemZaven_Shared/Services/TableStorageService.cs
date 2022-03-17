using System;
using System.Collections.Generic;
using System.Linq;
using EventTicketingSystemZaven_CommanProcessor.Models;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_Shared.Services;

public class TableStorageService : ITableStorageService
{
    public CloudTable EventsTable { get; }

    private readonly ILogger<TableStorageService> _logger;

    public TableStorageService(
        TicketingStorageAccountConfiguration storageAccountConfiguration,
        ILogger<TableStorageService> logger)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConfiguration.ConnectionString);
        CloudTableClient tableStorageClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

        EventsTable = tableStorageClient.GetTableReference(Constants.EventsTable);
        _logger = logger;
    }

    public IEnumerable<TableEntity> QueryEvents(string partitionKey, string rowKey = null)
    {
        return QueryEvents<TableEntity>(partitionKey, rowKey);
    }

    public IEnumerable<T> QueryEvents<T>(string partitionKey, string rowKey = null) where T : ITableEntity, new()
    {
        IEnumerable<T> result = Enumerable.Empty<T>();

        try
        {
            var partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            TableQuery<T> query;

            if (rowKey != null)
            {
                var rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
                var combinedFilter = TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter);

                query = new TableQuery<T>().Where(combinedFilter);
            }
            else
            {
                query = new TableQuery<T>().Where(partitionFilter);
            }

            result = EventsTable.ExecuteQuery(query);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        return result;
    }

    public IEnumerable<EventEntity> QueryEvents(IEnumerable<string> eventsIds)
    {
        IEnumerable<EventEntity> result = Enumerable.Empty<EventEntity>();

        try
        {
            var partitionFilters = eventsIds.Select(key => TableQuery.GenerateFilterCondition(nameof(TableEntity.PartitionKey), QueryComparisons.Equal, key));
            var rowKeyFilter = TableQuery.GenerateFilterCondition(nameof(TableEntity.RowKey), QueryComparisons.Equal, Constants.EventRowKey);

            var indexFilters = partitionFilters.Any() 
                ? partitionFilters.Aggregate((string f1, string f2) => TableQuery.CombineFilters(
                    TableQuery.CombineFilters(f1, TableOperators.And, rowKeyFilter), TableOperators.Or, f2))
                : null;

            var fullFilter = indexFilters ?? rowKeyFilter;

            result = EventsTable.ExecuteQuery(new TableQuery<EventEntity>().Where(fullFilter));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        return result;
    }

    public IEnumerable<TicketPurchaseEntity> GetTickets(Guid eventId)
    {
        IEnumerable<TicketPurchaseEntity> result = Enumerable.Empty<TicketPurchaseEntity>();

        try
        {
            var partitionFilters = TableQuery.GenerateFilterCondition(nameof(TableEntity.PartitionKey), QueryComparisons.Equal, eventId.ToString());
            var rowKeyFilter = TableQuery.GenerateFilterCondition(nameof(TableEntity.RowKey), QueryComparisons.NotEqual, Constants.EventRowKey);

            var indexFilter = TableQuery.CombineFilters(partitionFilters, TableOperators.And, rowKeyFilter);

            result = EventsTable.ExecuteQuery(new TableQuery<TicketPurchaseEntity>().Where(indexFilter));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        return result;
    }
}
