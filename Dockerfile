#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["P_Cloud_API/P_Cloud_API.csproj", "P_Cloud_API/"]
RUN dotnet restore "P_Cloud_API/P_Cloud_API.csproj"
COPY . .
WORKDIR "/src/P_Cloud_API"
RUN dotnet build "P_Cloud_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "P_Cloud_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "P_Cloud_API.dll"]