#######################################################
# Step 1: Build the application in a container        #
#######################################################
# Use the official ASP.NET Core SDK image from Microsoft to build our app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container to /app
WORKDIR /app

# Copy all files from the current folder on the host to /app in the container
COPY . .

# Compile the application
RUN dotnet build MC.ProductService.sln -c Release

# Package the compiled application into a folder for deployment
RUN dotnet publish MC.ProductService.sln --no-restore -c Release --output /out/

#######################################################
# Step 2: Run the built application in a container    #
#######################################################
# Use the official ASP.NET Core Runtime image from Microsoft to run our app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Tell the runtime to use port 56508 for web traffic
ENV ASPNETCORE_HTTP_PORTS=56508
EXPOSE 56508

# Copy the packaged application from the previous stage to the current container
COPY --from=build /out .

# Set the command to run the application when the container starts
ENTRYPOINT ["dotnet", "MC.ProductService.API.dll"]
