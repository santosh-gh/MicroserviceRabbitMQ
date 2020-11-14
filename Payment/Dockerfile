FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 7002

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Payment/Payment.csproj", "Payment/"]
RUN dotnet restore "Payment\Payment.csproj"
COPY . .
WORKDIR "/src/Payment"
RUN dotnet build "Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.dll"]
