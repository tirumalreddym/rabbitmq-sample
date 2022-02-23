#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM nexusprod.corp.intranet:4567/devbaseimages/development_base_images/dotnetsdk3.1:2021Q2 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000


FROM nexusprod.corp.intranet:4567/devbaseimages/development_base_images/dotnetsdk3.1:2021Q2 AS build
USER root
WORKDIR /src
COPY ["rabbitmq-sample.csproj", "."]
RUN dotnet restore "./rabbitmq-sample.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "rabbitmq-sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rabbitmq-sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rabbitmq-sample.dll"]