FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["./src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["./src/Domain/Domain.csproj", "src/Domain/"]
COPY ["./src/Api/Api.csproj", "src/Api/"]
COPY ["./src/Application/Application.csproj", "src/Application/"]

RUN dotnet restore "src/Api/Api.csproj"

COPY ./src/ .

WORKDIR "/src/Api/"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app/publish

COPY --from=publish /app/publish .
RUN ls -l
EXPOSE 5234
ENTRYPOINT [ "dotnet", "StudyZen.Api.dll" ]
