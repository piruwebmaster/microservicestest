using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CommandsService.Data;
using CommandsService.Validators;
using CommandsService.EventProcessing;
using Microsoft.Extensions.Options;
using CommandsService.AsyncDataService;
using CommandsService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);


#region CONFIGURATION

builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection(RabbitMQConfiguration.NAME));
builder.Services.AddOptions<RabbitMQConfiguration>().Configure(configuration =>
    new RabbitMQConfiguration() { Host = "localhost", Port = 5672 }
);
#endregion CONFIGURATION



// Add services to the container.
builder.Services.AddSingleton(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<RabbitMQConfiguration>>();
    if (options is not null)
    {
        return options.Value;
    }
    return new RabbitMQConfiguration();
});

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseInMemoryDatabase("InMem"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMvc()
    .AddFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


#region SERVICES
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<IRepository<Command>, Repository<Command>>();
//builder.Services.AddScoped<IRepository<Platform>, Repository<Platform>>();
builder.Services.AddScoped<UnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

builder.Services.AddValidatorsFromAssemblyContaining<CommandCreateDtoValidator>();
#endregion SERVICES


var app = builder.Build();
//app.Services.GetService<IMapper>()?.ConfigurationProvider
//    .AssertConfigurationIsValid();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UsePrepPopulation();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoint =>
    endpoint.MapControllers()
);

//app.UseRouting();


app.Run();
