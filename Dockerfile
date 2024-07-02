#######################################################
# Step 1: Build the application in a container        #
#######################################################
# Download the official ASP.NET Core SDK image
# to build the project while creating the docker image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Build the application
COPY . .
RUN dotnet build MC.ProductService.sln -c Release

# Publish the application
RUN dotnet publish MC.ProductService.sln --no-restore -c Release --output /out/

#######################################################
# Step 2: Run the build outcome in a container        #
#######################################################
# Download the official ASP.NET Core Runtime image
# to run the compiled application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=56508
EXPOSE 56508

# Copy the build output from the SDK image
COPY --from=build /out .

# Start the application
ENTRYPOINT ["dotnet", "MC.ProductService.API.dll"]
