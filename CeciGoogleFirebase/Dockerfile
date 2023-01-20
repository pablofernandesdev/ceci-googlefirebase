#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CeciGoogleFirebase.WebApplication/CeciGoogleFirebase.WebApplication.csproj", "CeciGoogleFirebase.WebApplication/"]
COPY ["CeciGoogleFirebase.Service/CeciGoogleFirebase.Service.csproj", "CeciGoogleFirebase.Service/"]
COPY ["CeciGoogleFirebase.Infra.Data/CeciGoogleFirebase.Infra.Data.csproj", "CeciGoogleFirebase.Infra.Data/"]
COPY ["CeciGoogleFirebase.Infra.CrossCutting/CeciGoogleFirebase.Infra.CrossCutting.csproj", "CeciGoogleFirebase.Infra.CrossCutting/"]
COPY ["CeciGoogleFirebase.Domain/CeciGoogleFirebase.Domain.csproj", "CeciGoogleFirebase.Domain/"]
COPY ["CeciGoogleFirebase.WebApplication/CeciGoogleFirebase.Domain.xml", "CeciGoogleFirebase.WebApplication/"]
COPY ["CeciGoogleFirebase.WebApplication/CeciGoogleFirebase.WebApplication.xml", "CeciGoogleFirebase.WebApplication/"]
RUN dotnet restore "CeciGoogleFirebase.WebApplication/CeciGoogleFirebase.WebApplication.csproj"
COPY . .
WORKDIR "/src/CeciGoogleFirebase.WebApplication"
RUN dotnet build "CeciGoogleFirebase.WebApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CeciGoogleFirebase.WebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CeciGoogleFirebase.WebApplication.dll"]