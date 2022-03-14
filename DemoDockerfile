#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Demo.Service/Demo.Service.csproj", "Demo.Service/"]
COPY ["Demo.UnitOfWork/Demo.UnitOfWork.csproj", "Demo.UnitOfWork/"]
COPY ["Demo.EntityFramework/Demo.EntityFramework.csproj", "Demo.EntityFramework/"]
RUN dotnet restore "Demo.Service/Demo.Service.csproj"
COPY . .
WORKDIR "/src/Demo.Service"
RUN dotnet build "Demo.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Service.dll"]