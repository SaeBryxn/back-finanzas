# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solo el csproj para cachear restore
COPY CreditApp.Api.csproj ./
RUN dotnet restore

# Resto del código
COPY . .
RUN dotnet publish -c Release -o /app

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

# Render (Docker) necesita que el contenedor escuche en un puerto fijo.
# Usaremos 8080 y lo configuraremos en la UI de Render.
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CreditApp.Api.dll"]
