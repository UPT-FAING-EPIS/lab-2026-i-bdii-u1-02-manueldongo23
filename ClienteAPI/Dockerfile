FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar csproj y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo lo demás y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# Imagen de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ClienteAPI.dll"]
