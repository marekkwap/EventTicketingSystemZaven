using System.Text.Json.Serialization;
using EventTicketingSystemZaven_Api.Services;
using EventTicketingSystemZaven_CommanProcessor.Models;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventTicketingSystemZaven_Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        services.AddSwaggerGen();

        services.AddSingleton(Configuration.GetSection(nameof(EventHubConfiguration)).Get<EventHubConfiguration>());
        services.AddSingleton(Configuration.GetSection(nameof(TicketingStorageAccountConfiguration)).Get<TicketingStorageAccountConfiguration>());

        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IEventsService, EventsService>();
        services.AddSingleton<ITicketsService, TicketsService>();
        services.AddSingleton<ITableStorageService, TableStorageService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
