#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Web.Facade/Web.Facade.csproj", "Web.Facade/"]
COPY ["Infrastructure.Auth/Infrastructure.Auth.csproj", "Infrastructure.Auth/"]
COPY ["Infrastructure.Database/Infrastructure.Database.csproj", "Infrastructure.Database/"]
COPY ["Infrastructure.Core/Infrastructure.Core.csproj", "Infrastructure.Core/"]
COPY ["News.Service/News.Service.csproj", "News.Service/"]
COPY ["Messaging.Service/Messaging.Service.csproj", "Messaging.Service/"]
RUN dotnet restore "Web.Facade/Web.Facade.csproj"
COPY . .
WORKDIR "/src/Web.Facade"
RUN dotnet build "Web.Facade.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Facade.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Facade.dll"]