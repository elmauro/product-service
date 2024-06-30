#######################################################
# Step 1: Build the application in a container        #
#######################################################
# Download the official ASP.NET Core SDK image
# to build the project while creating the docker image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Install Sonar Scanner, Coverlet and Java (required for Sonar Scanner)
#RUN apt-get update && \
#    apt-get install -y openjdk-17-jdk curl iputils-ping
#RUN dotnet tool install --global dotnet-sonarscanner
#RUN dotnet tool install --global coverlet.console
#ENV PATH="$PATH:/root/.dotnet/tools"

# Start Sonar Scanner
#RUN dotnet sonarscanner begin \
#  /k:insurance \
#  /d:sonar.host.url=http://sonar-server:9000 \
#  /d:sonar.login=admin \
#  /d:sonar.password=admin \
#  /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml

# Restore NuGet packages and build the application
COPY . .
RUN dotnet build MC.ProductService.sln -c Release

# Publish the application
RUN dotnet publish MC.ProductService.sln -c Release --output /out/

#RUN dotnet test \
#  /p:CollectCoverage=true \
#  /p:CoverletOutputFormat=opencover \
#  /p:CoverletOutput="/coverage"

# End Sonar Scanner
#RUN dotnet sonarscanner end

#ENTRYPOINT ["bash", "curl http://sonar-server:9000/api/server/version"]

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
