FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 11111
EXPOSE 8888

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy our projects into the container
COPY ["PayMe.Server/PayMe.Server.csproj", "PayMe.Server/"]
COPY ["PayMe.Shared/PayMe.Shared.csproj", "PayMe.Shared/"]
COPY ["PayMe.Grains/PayMe.Grains.csproj", "PayMe.Grains/"]

# Restore on the server, which should restore the others
RUN dotnet restore "PayMe.Server/PayMe.Server.csproj"
COPY . .

WORKDIR "/src/PayMe.Server"
RUN dotnet build "PayMe.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayMe.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayMe.Server.dll"]
