# build state
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /source
COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a x64 --use-current-runtime --self-contained false -o /app ./RecognizerCovers/Covers/Covers.csproj


# state w/o build files
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
EXPOSE 50003

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Covers.dll"]