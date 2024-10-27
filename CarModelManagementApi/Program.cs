using CarModelManagementApi.Data;
using CarModelManagementApi.Middlewares;
using CarModelManagementApi.Services;

namespace CarModelManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("AllowClientApplicationCore", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Load configuration
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Register services for dependency injection
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
            builder.Services.AddScoped<ICarModelService, CarModelService>();

            // Add controllers
            builder.Services.AddControllers();

            // Configure Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowClientApplicationCore");

            app.UseAuthorization();
            app.MapControllers();

            app.Run();


        }
    }
}
