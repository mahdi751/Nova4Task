# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY Nova4/*.csproj ./
RUN dotnet restore

# Copy the rest of the app and build
COPY Nova4/ ./Nova4/
RUN dotnet publish ./Nova4/Nova4.csproj -c Release -o out

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Nova4Task.dll"]
