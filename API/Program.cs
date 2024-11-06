
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TravelSuggest.Business;
using TravelSuggest.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

 //Inyecto dependencia de User
 builder.Services.AddScoped<IUserRepository, UserRepository>();
 builder.Services.AddScoped<IUserService, UserService>();

//Inyecto UserEF
 builder.Services.AddScoped<IUserRepository, UserEFRepository>();

 //Inyecto dependencia de Destino
 builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
 builder.Services.AddScoped<IDestinationService, DestinationService>();

//Inyecto DestinationEF
 builder.Services.AddScoped<IDestinationRepository, DestinationEFRepository>();

//Inyecto dependencia de sugerencia
 builder.Services.AddScoped<ISuggestionRepository, SuggestionRepository>();
 builder.Services.AddScoped<ISuggestionService, SuggestionService>();

 //Inyecto SuggestionEF
 builder.Services.AddScoped<ISuggestionRepository, SuggestionEFRepository>();


 // Cadena de conexión BBDD
var connectionString = builder.Configuration.GetConnectionString("ServerDB_localhost");
// var connectionString = builder.Configuration.GetConnectionString("ServerAzure");
// var connectionString = builder.Configuration.GetConnectionString("ServerDB");

builder.Services.AddDbContext<TravelSuggestContext>(options =>
    options.UseSqlServer(connectionString)
);


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
