FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /source
EXPOSE 8080

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "CartoonCaps.Referral.Api.dll"]
