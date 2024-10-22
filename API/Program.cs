
using Microsoft.OpenApi.Models;
using TravelSuggest.Business;
using TravelSuggest.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

 //Inyecto dependencia de User
 builder.Services.AddScoped<IUserRepository, UserRepository>();
 builder.Services.AddScoped<IUserService, UserService>();

 //Inyecto dependencia de Destino
 builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
 builder.Services.AddScoped<IDestinationService, DestinationService>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelSuggest API", Version = "v1" });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});



var app = builder.Build();


// Configurar el pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    // Habilitar Swagger y el middleware de Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // Hace que Swagger UI se cargue en la ruta raíz (localhost:5146/).
    });
}

app.UseDeveloperExceptionPage();
app.UseCors("MyAllowedOrigins");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
