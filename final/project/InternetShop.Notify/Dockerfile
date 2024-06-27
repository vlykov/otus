#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InternetShop.Notify/InternetShop.Notify.csproj", "InternetShop.Notify/"]
COPY ["InternetShop.Common/InternetShop.Common.csproj", "InternetShop.Common/"]
RUN dotnet restore "./InternetShop.Notify/InternetShop.Notify.csproj"
COPY . .
WORKDIR "/src/InternetShop.Notify"
RUN dotnet build "./InternetShop.Notify.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InternetShop.Notify.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternetShop.Notify.dll"]