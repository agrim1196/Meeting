using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sieve.Services;
using WebApi.Helpers;
using WebApi.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(Log.Logger, Serilog.Events.LogEventLevel.Verbose).CreateLogger();

try
{
    Log.Information("Starting Web Host ");

    var builder = WebApplication.CreateBuilder(args);

    string sample = "mypolicy";

    builder.Services.AddDbContext<DatabaseContext>
        (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

    builder.Services.AddCors(options =>
    {

        options.AddPolicy(
            sample,
                          policy =>
                          {
                              policy.AllowAnyOrigin();
                              policy.AllowAnyHeader();
                              policy.AllowAnyMethod();
                          });
    });

    builder.Services.AddControllers();
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<SieveProcessor>();
    builder.Services.AddTransient<IUsersService, UsersService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<JwtMiddleware>();
    app.UseHttpsRedirection();
    app.UseCors(sample);
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

