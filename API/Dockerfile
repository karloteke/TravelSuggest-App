FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivo de solución
COPY *.sln .
# Copiar archivos .csproj de las distintas capas del proyecto
COPY Models/*.csproj Models/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY API/*.csproj API/
# Restaurar las dependencias
RUN dotnet restore TravelSuggest.sln
# Copiar el resto del código
COPY . .
# Publicar el proyecto
RUN dotnet publish TravelSuggest.sln -c Release -o API/out
# Imagen de runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
# Copiar el resultado del build
COPY --from=build /app/API/out .

EXPOSE 80
# Configurar el entrypoint para ejecutar la aplicación
ENTRYPOINT ["dotnet", "TravelSuggest.API.dll"]













