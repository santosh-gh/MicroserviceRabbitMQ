FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Order/Order.csproj", "Order/"]
RUN dotnet restore "Order\Order.csproj"
COPY . .
WORKDIR "/src/Order"
RUN dotnet build "Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.dll"]
