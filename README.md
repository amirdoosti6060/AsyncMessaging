# AsyncMessaging

## Introduction
This example describe how to implement Asynchronus Messaging Design Pattern In Microservices  
I've used Kafka as a message brocker in this project.  
I wrote an article in Linkedin which completely cover **Asynchronous Messaging** design pattern:  
https://www.linkedin.com/pulse/asynchronous-messaging-design-pattern-amir-doosti  

## Structure of solution
In this solution there are two projects. Both of them are ASP.NET Core Web API  
They are going to communicate with each other via async messaging.  
- CommandAPI: Using an API it get a command and an array for operands and execute that command and return the result. It also send a message that contains command, parameters and result to another microservice using Async Messaging based on Kafka. I return the value return by Kafka API to be familiar with it but in production it is not a good idea.
- CommandHistory: This microservice receive the message from CommandAPI microservice and store it in an in memory structure. By use of an API you can get the history of all executed commands. In production the messages normaly persist in a database and the message may contains more information but to keep everything simple I avoided it.

The commands defined in CommandAPI are as follow:
- add {array of int} -> Ex:  {command: "add", parameters: [10, 2, 3]} -> Result: 15
- subtract {array of int} -> Ex:  {command: "add", parameters: [10, 2, 3]} -> Result: 5
- multiply {array of int} -> Ex:  {command: "add", parameters: [10, 2, 3]} -> Result: 60
- divide {array of int} -> Ex:  {command: "add", parameters: [10, 2, 3]} -> Result: 1
- sort {array of int} -> Ex: {command: "sort", parameters: [10, 2, 3]} -> Result: [2, 3, 10]
- save {array of int} -> Ex: {command: "save", parameters: [10, 2, 3]} -> Result: "saved 10, 2, 3 in history!"

Please note that this example provide for educational purpose and in many aspects needs modification for production environments.

## Technology stack
- OS: Windows 10 Enterprise - 64 bits
- IDE: Visual Studio Enterprise 2022 (64 bits) - version 17.2.5
- Broker: Kafka (https://hub.docker.com/r/bitnami/kafka)
- Framework: .Net 6
- Language: C#

## How to install Kafka?
To install Kafka you can use docker or install it directly or even use cloud version but I used the bitnami/kafka from DockerHub. You can get it from this link:
https://hub.docker.com/r/bitnami/kafka  
As mentioned in above link, it is easier to download and start docker using the docker-compose.yml but I make a changes in it to have access to Kafka from outside of docker compose. Here is the changes I make:  
Change from  
      - KAFKA_CFG_ADVERTISED_LISTENERS=PLAINTEXT://:9092  
to  
      - KAFKA_CFG_ADVERTISED_LISTENERS=PLAINTEXT://kafka:9092  
  
Based on the location of Kafka, you need to change its address in appsettings.json in both projects.  

## What you may learn from this project
- Asynchronous Messaging Design Pattern in Microservices. 
- How to send and recieve message using Kafka in .Net (Confluent.Kafka)
- How can register a serializer and deserializer in Kafka
- How can config and do some settings programatically
- Create abstract layer and service to use Kafka
- Strategy design pattern for command
- Error Handling using 
- Using ArrayList in multi threading
- Using extensions to have a cleaner code
