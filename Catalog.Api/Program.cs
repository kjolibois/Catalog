using System.Net.Mime;
using System.Text.Json;
using Catalog.Api.Data;
using Catalog.Api.Repositories;
using Catalog.Api.Settings;
using Catalog.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
var mongoDbSettings=builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IItemsRepository,MongoDbItemsRepository>();
builder.Services.AddSingleton<IBakersRepository,MongoDbBakersRepository>();
builder.Services.AddSingleton<ISeasonsRepository,MongoDbSeasonsRepository>();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("LiteConnection"));
});

builder.Services.AddSingleton<IMongoClient>(servicesProvider =>
{
    return new MongoClient(mongoDbSettings.ConnectionString);
}
);
Console.WriteLine(mongoDbSettings.ConnectionString);
builder.Services.AddHealthChecks().AddMongoDb(
    mongoDbSettings.ConnectionString,
    name:"mongodb",
    timeout:TimeSpan.FromSeconds(3),
    tags: new[] {"ready"}
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseHttpsRedirection();
}

//

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{ 
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new{
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new {
                    name = entry.Key,
                    status= entry.Value.Status.ToString(),
                    exception = entry.Value.Exception !=  null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            }
        ); 
        context.Response.ContentType= MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
 }
);
app.MapHealthChecks("/health/live", new HealthCheckOptions{ Predicate = (_) => false});
PrepDb.PrepPopulation(mongoDbSettings.ConnectionString);

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try{
        //context.Database.Migrate();
        DbInitializer.Initialize(dbContext);
    }
    catch(Exception ex){
        logger.LogError(ex.Message);
    }

}
//var serviceScope= app.Services.CreateScope();
//var context= app.Services.GetRequiredService<StoreContext>();
//ApplicationServices.CreateScope();

app.Run();
//PrepDb.PrepPopulation(app, app.Environment.IsProduction());
