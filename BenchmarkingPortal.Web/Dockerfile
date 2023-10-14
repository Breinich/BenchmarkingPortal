FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj
RUN dotnet build ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj -c Release -o /app/publish
COPY --from=build /src/BenchmarkingPortal.pfx /app/publish/

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BenchmarkingPortal.Web.dll"]