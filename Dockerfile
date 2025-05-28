FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish

WORKDIR /src/Web
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app
COPY --from=publish /app/publish .
COPY ./root.crt ./root.crt

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.dll"]