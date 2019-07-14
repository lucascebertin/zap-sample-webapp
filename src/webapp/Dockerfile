FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["./Vulnerable.Sample.WebApp/Vulnerable.Sample.WebApp.csproj", "Vulnerable.Sample.WebApp/"]
COPY ["./Vulnerable.Sample.Migration/Vulnerable.Sample.Migration.csproj", "Vulnerable.Sample.Migration/"]
RUN dotnet restore "Vulnerable.Sample.WebApp/Vulnerable.Sample.WebApp.csproj"
RUN dotnet restore "Vulnerable.Sample.Migration/Vulnerable.Sample.Migration.csproj"
COPY . .
RUN dotnet build --no-restore "Vulnerable.Sample.WebApp/Vulnerable.Sample.WebApp.csproj" -c Release -o /app
RUN dotnet build --no-restore "Vulnerable.Sample.Migration/Vulnerable.Sample.Migration.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore "Vulnerable.Sample.WebApp/Vulnerable.Sample.WebApp.csproj" -c Release -o /app
RUN dotnet publish --no-restore "Vulnerable.Sample.Migration/Vulnerable.Sample.Migration.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

RUN apt-get update && apt-get install wget
RUN wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh && \
    chmod +x wait-for-it.sh

ENV DB db:1433

ENTRYPOINT ./wait-for-it.sh -t 30 $DB -- \ 
    dotnet Vulnerable.Sample.Migration.dll && \ 
    dotnet Vulnerable.Sample.WebApp.dll
