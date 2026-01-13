FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9090
EXPOSE 9091

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ASS-Di-Stroid-API/ASS-Di-Stroid-API.csproj", "ASS-Di-Stroid-API/"]
RUN dotnet restore "ASS-Di-Stroid-API/ASS-Di-Stroid-API.csproj"
COPY . .
WORKDIR "/src/ASS-Di-Stroid-API"
RUN dotnet build "./ASS-Di-Stroid-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ASS-Di-Stroid-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASS-Di-Stroid-API.dll"]
