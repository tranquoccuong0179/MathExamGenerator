# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MathExamGenerator.API/MathExamGenerator.API.csproj", "MathExamGenerator.API/"]
COPY ["MathExamGenerator.Service/MathExamGenerator.Service.csproj", "MathExamGenerator.Service/"]
COPY ["MathExamGenerator.Repository/MathExamGenerator.Repository.csproj", "MathExamGenerator.Repository/"]
COPY ["MathExamGenerator.Model/MathExamGenerator.Model.csproj", "MathExamGenerator.Model/"]
RUN dotnet restore "./MathExamGenerator.API/MathExamGenerator.API.csproj"
COPY . .
WORKDIR "/src/MathExamGenerator.API"
RUN dotnet build "./MathExamGenerator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MathExamGenerator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MathExamGenerator.API.dll"]