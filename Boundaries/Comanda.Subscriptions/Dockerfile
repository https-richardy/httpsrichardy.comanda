# use ASP.NET Core 9.0 runtime image (alpine) as base
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8085

# use SDK image (Alpine) for build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# copy project files to restore dependencies
COPY ["Source/Comanda.Subscriptions.WebApi/Comanda.Subscriptions.WebApi.csproj", "Comanda.Subscriptions.WebApi/"]

# copy the entire solution 'n related projects
COPY ["Comanda.Subscriptions.sln", "./"]

# restore dependencies for the project
RUN dotnet restore "Comanda.Subscriptions.WebApi/Comanda.Subscriptions.WebApi.csproj"

# copy all source code into the container
COPY Source/ ./Source/

# set working directory to the web project
WORKDIR "/src/Source/Comanda.Subscriptions.WebApi"

# build in Release mode
RUN dotnet build "Comanda.Subscriptions.WebApi.csproj" -c Release -o /app/build

# publish the project for production
FROM build AS publish
RUN dotnet publish "Comanda.Subscriptions.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# final image to run the app
FROM base AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# copy published files from the publish stage
COPY --from=publish /app/publish .

# set the command to start the application
ENTRYPOINT ["dotnet", "Comanda.Subscriptions.WebApi.dll"]
