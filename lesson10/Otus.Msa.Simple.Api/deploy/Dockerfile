FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 8000
COPY /publish/ /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Otus.Msa.Simple.Api.dll"] 
