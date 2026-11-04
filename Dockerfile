# syntax=docker/dockerfile:1

# ---- Build aşaması ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Önce sadece csproj'u kopyala; restore katmanı cache'lenebilsin
COPY SendNotificationAPI.csproj ./
RUN dotnet restore SendNotificationAPI.csproj

# Kalan kaynak kodu kopyala (Tests, .dockerignore ile hariç tutuluyor)
COPY . .
RUN dotnet publish SendNotificationAPI.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---- Runtime aşaması ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# .NET 8+ imajlarında varsayılan port 8080'dir; root olmayan "app" kullanıcısı ile çalıştır
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app

ENTRYPOINT ["dotnet", "SendNotificationAPI.dll"]
