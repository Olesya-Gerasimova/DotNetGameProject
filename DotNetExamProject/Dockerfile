FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DotNetExamProject/DotNetExamProject.csproj", "DotNetExamProject/"]
RUN dotnet restore "DotNetExamProject/DotNetExamProject.csproj"
COPY . .
WORKDIR "/src/DotNetExamProject"
RUN dotnet build "DotNetExamProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetExamProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetExamProject.dll"]
