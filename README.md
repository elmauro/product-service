# Product-Service API Documentation
## Assumptions

Before you begin, ensure that your system meets the following prerequisites:

- Windows 10 installed
- NET 8 SDK installed
- Docker Desktop installed
- Visual Studio 2022 Community Edition installed

## Installation

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

![image](https://github.com/elmauro/product-service/assets/9219845/d76f5338-0a28-4627-a9bc-25eec308b6b4)


**Creating a Product**

To create a product, use the POST method with the desired parameters in the JSON object

![image](https://github.com/elmauro/product-service/assets/9219845/08e0e17d-5def-4df9-b94b-0f95befc9dec)

Execute the request and the method will respond with information about the created product, including the productId in the response headers' location field.

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



