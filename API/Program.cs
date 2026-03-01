
using Microsoft.OpenApi.Models;
using TravelSuggest.Business;
using TravelSuggest.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuración local con secretos (no se sube a GitHub)
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();

// Obtener la clave secreta desde la configuración
var secretKey = builder.Configuration["JwtSettings:Secret"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("La clave secreta para JWT no está configurada.");
}

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelSuugest API", Version = "v1" });

    // Configure the security scheme for JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
                }
            },
            Array.Empty<string>()
        }
    });
});

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
// Se puede cambiar con la variable de entorno CONNECTION_STRING_NAME (por defecto: ServerDB_localhost)
var connectionStringName = Environment.GetEnvironmentVariable("CONNECTION_STRING_NAME") ?? "ServerDB_localhost";
var connectionString = builder.Configuration.GetConnectionString(connectionStringName);

builder.Services.AddDbContext<TravelSuggestContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure())
);

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

// Aplicar migraciones automáticamente al arrancar (crea la BD y aplica seed data)
// Reintenta varias veces porque SQL Server puede tardar en arrancar en Docker
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TravelSuggestContext>();
    var retries = 10;
    for (int i = 0; i < retries; i++)
    {
        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas correctamente.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Intento {i + 1}/{retries} - Error conectando a la BD: {ex.Message}");
            if (i == retries - 1) throw;
            Thread.Sleep(5000); // Espera 5 segundos entre reintentos
        }
    }
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseCors("MyAllowedOrigins");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelSuggest API V1");
    c.RoutePrefix = string.Empty;  
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
