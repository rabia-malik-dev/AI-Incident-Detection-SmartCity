FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SmartCity.Api/SmartCity.Api.csproj SmartCity.Api/
RUN dotnet restore SmartCity.Api/SmartCity.Api.csproj
COPY SmartCity.Api/ SmartCity.Api/
RUN dotnet publish SmartCity.Api/SmartCity.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SmartCity.Api.dll"]
