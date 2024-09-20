# Product-Service API Documentation

## Introduction

This API helps manage product information for businesses. It makes it easier to add, update, and get data about products.

In this document, we will talk about:

- **Design Patterns Used**
- **Basic Requirements**
- **How to Install**

## Design Patterns Used

- Repository Pattern: Abstraction of the logic Access Data
- Cqrs Pattern: Separates read and write operations in an application
- Mediator Pattern: Reduce direct dependencies between objects

## Basic Requirements

Before you begin, ensure that your system meets the following prerequisites:

**Running locally with Visual Studio 2022**
- Windows 10 installed
- At least 4 GB of RAM (8 GB or more recommended)
- Sufficient disk space to install the .NET SDK, development tools, and project dependencies (at least 10 GB of free space recommended)
- NET 8 SDK installed. Download and install the .NET 8 SDK from the official .NET website: https://dotnet.microsoft.com/download/dotnet/8.0
- dotnet tool install --global dotnet-ef
- Docker or Docker Desktop installed https://www.docker.com/
- docker-compose
- Git (version control): [Download from Git](https://git-scm.com/)
- Visual Studio 2022 Community Edition installed

**Running locally with Docker Desktop**
- Docker or Docker Desktop installed https://www.docker.com/
- docker-compose
- Git (version control): [Download from Git](https://git-scm.com/)

## How to Install

**Clone the repository**
```sh
git clone https://github.com/elmauro/product-service.git
```

Use Docker Compose to launch the database

```sh
docker-compose -f compose.yml up --build -d product-postgres
winpty docker exec -it product-postgres psql -U product -d product -f scripts/idempotent-migration.sql
```

![image](https://github.com/elmauro/product-service/assets/9219845/25c8a155-0aee-47d1-87d9-e5d2b7edac4b)

## Starting the Service

You can start the service using Visual Studio 2022 or Docker Compose

**With Docker Compose**

```sh
docker-compose -f compose.yml up --build -d product
```

![image](https://github.com/elmauro/product-service/assets/9219845/b737f711-ecd1-4a4b-9d03-4590d132775e)

## Using the Product Service API

**API Access**

The API can be accessed locally at:

```sh
http://localhost:56508/product/index.html
```

The API is fully documented using Swagger, which provides a detailed overview of available endpoints, request models, and response statuses. Please review the Swagger documentation to validate response HTTP statuses and understand the expected behavior of the API

![image](https://github.com/elmauro/product-service/assets/9219845/d76f5338-0a28-4627-a9bc-25eec308b6b4)


**Creating a Product**

To create a product, use the POST method with the desired parameters in the JSON object

![image](https://github.com/elmauro/product-service/assets/9219845/08e0e17d-5def-4df9-b94b-0f95befc9dec)

Execute the request and the method will respond with information about the created product

**Important!**
The response includes the product ID in the location section of the response headers. You need the product ID to update and access product information.

![image](https://github.com/elmauro/product-service/assets/9219845/349ef59a-7b21-4e87-b163-56bc1c962704)


**Retrieving a Product**

To retrieve product data, use the GET method with the productId parameter

```sh
http://localhost:56508/v1/Product?productId=c5f8e2e8-e4ed-4eef-8088-8d2684f4e71b
```

![image](https://github.com/elmauro/product-service/assets/9219845/dd68633f-82bb-4cd2-853c-5a5315f1470b)


**Updating a Product**

To update product details, use the PUT method with the new parameters in the JSON object

![image](https://github.com/elmauro/product-service/assets/9219845/3e4b6dbd-92a3-4c88-aa51-dbff8622d21d)

Execute, and then repeat the GET method to see the updated product information.

## Logs

You can view the logs from within the Docker container using the following command

```sh
docker logs [container_id]
```

![image](https://github.com/elmauro/product-service/assets/9219845/ddc76ad8-facd-45ca-af64-5f464af6620b)

![image](https://github.com/elmauro/product-service/assets/9219845/0c46bcb0-1595-4e82-b22a-2f9c002927cd)



## Mock Service

Access the Mock Discount Service at the following URL:

https://6680a0be56c2c76b495c7127.mockapi.io/v1/product

The API returns data based on the first discount value provided in the query

![image](https://github.com/elmauro/product-service/assets/9219845/7470de9f-3088-43dc-a9a5-deaf7a5df402)



