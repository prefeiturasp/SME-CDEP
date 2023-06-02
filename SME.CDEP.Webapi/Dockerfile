FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SME.CDEP.Webapi/SME.CDEP.Webapi.csproj", "SME.CDEP.Webapi/"]
RUN dotnet restore "SME.CDEP.Webapi/SME.CDEP.Webapi.csproj"

COPY . .
WORKDIR "/src/SME.CDEP.Webapi"
RUN dotnet build "SME.CDEP.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.CDEP.Webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV TZ=America/Sao_Paulo
RUN apt-get update \
    && apt-get install -yq tzdata locales -y \
    && dpkg-reconfigure --frontend noninteractive tzdata \
	  && locale-gen en_US.UTF-8
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.CDEP.Webapi.dll"]
