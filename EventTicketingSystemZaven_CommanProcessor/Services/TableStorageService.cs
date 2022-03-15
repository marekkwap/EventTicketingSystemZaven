using EventTicketingSystemZaven_CommanProcessor.Models;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_CommanProcessor.Services
{
    internal class TableStorageService
    {
        private readonly CloudTable _eventsTable;

        public TableStorageService(TicketingStorageAccountConfiguration storageAccountConfiguration)
        {
            
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageAccountConfiguration.ConnectionString);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tblclient.GetTableReference("Events");
        }

        public async Task AddRow(string tableName, object row)
        {
            
            TableOperation tableOperation = TableOperation.Insert()
        }
    }
}
