#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 3412


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RageshakeWebhookHandler.csproj", "."]
RUN dotnet restore "./RageshakeWebhookHandler.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RageshakeWebhookHandler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RageshakeWebhookHandler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER 65534
EXPOSE 3412
ENV ASPNETCORE_URLS=http://*:3412
ENTRYPOINT ["dotnet", "RageshakeWebhookHandler.dll"]