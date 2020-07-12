FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ENV ASPNETCORE_URLS https://+:5001
EXPOSE 80
EXPOSE 443
EXPOSE 5001

ENTRYPOINT ["dotnet run", " \loggrpc\regGRPC.sln"]
