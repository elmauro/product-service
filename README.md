# product-service API

## Asumptions

1. Windows 10
2. Net 8 sdk installed
3. Docker Desktop installed
4. Visual Studio 2022 Community 

## Instalation

1. Clone the code repository https://github.com/elmauro/product-service.git


## Running Locally

1. Launch Database with docker-compose and run the follwing commands
    
    a. docker-compose -f compose.yml up --build -d product-postgres

    b. winpty docker exec -it product-postgres psql -U product -d product -f scripts/idempotent-migration.sql


![image](https://github.com/elmauro/product-service/assets/9219845/25c8a155-0aee-47d1-87d9-e5d2b7edac4b)



2. Run the project solution with Visual Studio 2022 or Run with docker-compose

   a. With docker-compose. Run the following command
      
      docker-compose -f compose.yml up --build -d product


![image](https://github.com/elmauro/product-service/assets/9219845/b737f711-ecd1-4a4b-9d03-4590d132775e)



## Using the Product Service API

1. You can see the API running locally on http://localhost:56508/product/index.html


![image](https://github.com/elmauro/product-service/assets/9219845/d76f5338-0a28-4627-a9bc-25eec308b6b4)


2. Using the POST method

Add the desired parameters on the json object and Execute


![image](https://github.com/elmauro/product-service/assets/9219845/08e0e17d-5def-4df9-b94b-0f95befc9dec)

The method response provide information about the Created Product. 

You can see the productId created for queries on the response headers location: 

For this sccenario http://localhost:56508/v1/Product?productId=c5f8e2e8-e4ed-4eef-8088-8d2684f4e71

productId=c5f8e2e8-e4ed-4eef-8088-8d2684f4e71


![image](https://github.com/elmauro/product-service/assets/9219845/349ef59a-7b21-4e87-b163-56bc1c962704)


3. Using the GET method

Add the productId parameter and Execute


![image](https://github.com/elmauro/product-service/assets/9219845/dd68633f-82bb-4cd2-853c-5a5315f1470b)


You can see the Product Data inserted previously


4. Using the PUT method

Add the desired parameters on the json object and Execute


![image](https://github.com/elmauro/product-service/assets/9219845/3e4b6dbd-92a3-4c88-aa51-dbff8622d21d)


Note: Repeat the step 3 to see the new Product information









