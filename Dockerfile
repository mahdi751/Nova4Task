# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY Nova4/*.csproj ./Nova4/
RUN dotnet restore ./Nova4/Nova4.csproj

# Copy the rest of the app and build
COPY Nova4/ ./Nova4/
RUN dotnet publish ./Nova4/Nova4.csproj -c Release -o out

# Use the .NET 6 runtime image instead of .NET 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set the environment URL to listen on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet"]
CMD ["Nova4.dll"]
