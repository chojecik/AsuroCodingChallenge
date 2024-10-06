# Use the official ASP.NET Core runtime as a base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AsuroCodingChallenge.API/AsuroCodingChallenge.API.csproj", "AsuroCodingChallenge.API/"]
COPY ["AsuroCodingChallenge.BusinessLogic/AsuroCodingChallenge.BusinessLogic.csproj", "AsuroCodingChallenge.BusinessLogic/"]
COPY ["AsuroCodingChallenge.DataAccess/AsuroCodingChallenge.DataAccess.csproj", "AsuroCodingChallenge.DataAccess/"]
RUN dotnet restore "AsuroCodingChallenge.API/AsuroCodingChallenge.API.csproj"
COPY . .
WORKDIR "/src/AsuroCodingChallenge.API"
RUN dotnet build "AsuroCodingChallenge.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsuroCodingChallenge.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AsuroCodingChallenge.API.dll"]
