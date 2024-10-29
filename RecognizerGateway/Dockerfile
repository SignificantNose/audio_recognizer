# build state
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /source
COPY . .


# P.S. invoke docker build from outer directory to be able to see other projects
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a x64 --use-current-runtime --self-contained false -o /app ./RecognizerGateway/Gateway/Gateway.csproj


# state w/o build files
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
EXPOSE 50002

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Gateway.dll"]