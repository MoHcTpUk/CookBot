#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CookBot/CookBot.csproj", "CookBot/"]
COPY ["CookBot.App/CookBot.App.csproj", "CookBot.App/"]
COPY ["CookBot.BLL/CookBot.BLL.csproj", "CookBot.BLL/"]
COPY ["Core/Core.BLL/Core.BLL.csproj", "Core/Core.BLL/"]
COPY ["Core/Core.DAL/Core.DAL.csproj", "Core/Core.DAL/"]
COPY ["CookBot.DAL/CookBot.DAL.csproj", "CookBot.DAL/"]
RUN dotnet restore "CookBot/CookBot.csproj"
COPY . .
WORKDIR "/src/CookBot"
RUN dotnet build "CookBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CookBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookBot.dll"]
