namespace EventTicketingSystemZaven_CommandProcessor.Models;

public class TicketingStorageAccountConfiguration
{
    public string ConnectionString { get; set; }

    public string CheckpointContainerName { get; set; }
}
