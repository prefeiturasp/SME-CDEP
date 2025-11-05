FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SME.CDEP.Worker/SME.CDEP.Worker.csproj", "SME.CDEP.Worker/"]
RUN dotnet restore "SME.CDEP.Worker/SME.CDEP.Worker.csproj"

COPY . .
WORKDIR "/src/src/SME.CDEP.Worker"
RUN dotnet build "SME.CDEP.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.CDEP.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV TZ=America/Sao_Paulo
RUN apt-get update \
    && apt-get install -yq tzdata locales -y \
    && dpkg-reconfigure --frontend noninteractive tzdata \
	  && locale-gen en_US.UTF-8
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.CDEP.Worker.dll"]
