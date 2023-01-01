using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;
using PlatformService.SyncDataService.Http;
using PlatformService.Validators;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("using in memory data base");
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMem"));
}
else
{
    Console.WriteLine("using mssql database");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("PlatformsConn");
        Console.WriteLine($"using connection = {connectionString}");
        options.UseSqlServer(connectionString);
    });
}

builder.Services.AddMvc()
    .AddFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


#region CONFIGURE
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection(RabbitMqConfiguration.NAME));

// for add a default configuration
builder.Services.AddOptions<RabbitMqConfiguration>().Configure((options) =>
new RabbitMqConfiguration() { Host = "localhost", Port = 5672 });
#endregion CONFIGURE

#region SERVICES

builder.Services.AddSingleton(serviceProvider =>
{
    var options = serviceProvider.GetService<IOptions<RabbitMqConfiguration>>();
    if (options is not null)
        return options.Value;
    else return new RabbitMqConfiguration();
});
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddValidatorsFromAssemblyContaining<PlatformCreateDtoValidator>();

builder.Services.AddGrpc();

#endregion SERVICES

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UsePrepPopulation(app.Environment.IsProduction());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
    endpoint.MapGrpcService<GrpcPlatformService>();

    endpoint.MapGet("/protos/platforms.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
    });
});

//app.UseRouting();


app.Run();
