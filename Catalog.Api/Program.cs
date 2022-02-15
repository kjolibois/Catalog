using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Catalog.Api.Data;
using Catalog.Api.Entities;
using Catalog.Api.Middleware;
using Catalog.Api.Repositories;
using Catalog.Api.Services;
using Catalog.Api.Settings;
using Catalog.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
//var mongoDbSettings=builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{
        Description="jwt auth Header",
        Name = "Authorization",
        In= ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name= "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
      }
    });
});
//builder.Services.AddSingleton<IItemsRepository,MongoDbItemsRepository>();
//builder.Services.AddSingleton<IBakersRepository,MongoDbBakersRepository>();
//builder.Services.AddSingleton<ISeasonsRepository,MongoDbSeasonsRepository>();
//builder.Services.AddDbContext<StoreContext>(opt =>
//{
  //  opt.UseNpgsql(builder.Configuration.GetConnectionString("PGConnection"));
//});
builder.Services.AddDbContext<StoreContext>(options =>
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        string connStr;
        
        if (env == "Development")
        {
            // Use connection string from file.
            connStr = builder.Configuration.GetConnectionString("PGConnection");
        }
        else
        {
            // Use connection string provided at runtime by Heroku.
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Parse connection URL to connection string for Npgsql
            connUrl = connUrl.Replace("postgres://", string.Empty);
            var pgUserPass = connUrl.Split("@")[0];
            var pgHostPortDb = connUrl.Split("@")[1];
            var pgHostPort = pgHostPortDb.Split("/")[0];
            var pgDb = pgHostPortDb.Split("/")[1];
            var pgUser = pgUserPass.Split(":")[0];
            var pgPass = pgUserPass.Split(":")[1];
            var pgHost = pgHostPort.Split(":")[0];
            var pgPort = pgHostPort.Split(":")[1];

            connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;Trust Server Certificate=true";
        }

        // Whether the connection string came from the local development configuration file
        // or from the environment variable from Heroku, use it to set up your DbContext.
        options.UseNpgsql(connStr);
    });
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail= true;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<StoreContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => 
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer= false,
            ValidateAudience=false,
            ValidateLifetime=true,
            ValidateIssuerSigningKey= true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    });

builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<PaymentService>();

//builder.Services.AddSingleton<IMongoClient>(servicesProvider =>
//{
  //  return new MongoClient(mongoDbSettings.ConnectionString);
///}
//);
//Console.WriteLine(mongoDbSettings.ConnectionString);
//builder.Services.AddHealthChecks().AddMongoDb(
  //  mongoDbSettings.ConnectionString,
   // name:"mongodb",
    //timeout:TimeSpan.FromSeconds(3),
  //  tags: new[] {"ready"}
 //   );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseHttpsRedirection();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(opt=>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});
//
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToController("Index","Fallback");

/*app.MapHealthChecks("/health/ready", new HealthCheckOptions
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
app.UseMiddleware<ExceptionMiddleware>();
app.MapHealthChecks("/health/live", new HealthCheckOptions{ Predicate = (_) => false});
//PrepDb.PrepPopulation(mongoDbSettings.ConnectionString);
*/
using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    try{
        //context.Database.Migrate();
         await DbInitializer.Initialize(dbContext,userManager);
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
