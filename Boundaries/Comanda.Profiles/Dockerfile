# use ASP.NET Core 9.0 runtime image as base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# use SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy project files to restore dependencies
COPY ["Source/Comanda.Profiles.WebApi/Comanda.Profiles.WebApi.csproj", "Comanda.Profiles.WebApi/"]

# copy the entire solution 'n related projects
COPY ["Comanda.Profiles.sln", "./"]

# restore dependencies for the project
RUN dotnet restore "Comanda.Profiles.WebApi/Comanda.Profiles.WebApi.csproj"

# copy all source code into the container
COPY Source/ ./Source/

# set working directory to the web project
WORKDIR "/src/Source/Comanda.Profiles.WebApi"

# build in Release mode
RUN dotnet build "Comanda.Profiles.WebApi.csproj" -c Release -o /app/build

# publish the project for production
FROM build AS publish
RUN dotnet publish "Comanda.Profiles.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# final image to run the app
FROM base AS final
WORKDIR /app

# copy published files from the publish stage
COPY --from=publish /app/publish .

# set the command to start the application
ENTRYPOINT ["dotnet", "Comanda.Profiles.WebApi.dll"]
