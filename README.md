# OutboxInboxPatternSample

A sample of the transactional Outbox and Inbox patterns using C#, SQL Server and RabbitMQ.

> Note: This project is just a proof of concept and is not production ready, in production you may use frameworks such as [MassTransit](https://github.com/MassTransit/MassTransit) or [NServiceBus](https://github.com/Particular/NServiceBus).

## Requirements

1. Docker
2. Docker compose

## Run sample

1. Set-up the SQL Server database and RabbitMQ:

```BASH
docker compose up
```

2. Apply database schemas:

```BASH
dotnet ef database update -p .\SenderApi\
dotnet ef database update -p .\Receiver\
```

## Usage

1. Run **SenderApi** project:

```BASH
dotnet run --project .\SenderApi\
```

2. Run **OutboxProcessor** project:

```BASH
dotnet run --project .\OutboxProcessor\
```

3. Run **Receiver** project:

```BASH
dotnet run --project .\Receiver\
```

4. Launch swagger in your web browser:
   https://localhost:7278/swagger/index.html

5. Using Swagger or Postman make a HTTP POST to **/Users**.

### Parts:

This sample consists of 3 projects:

1. **SenderApi**: API project, when a new user is created, it stores an Outbox message in the same transaction than the business database using the Unit of Work pattern.

2. **OutboxProcessor**: Worker, it read the Outbox database table and publish the unpublished ones to a RabbitMQ queue.

3. **Receiver**: Worker, it is subscribed to the outbox RabbitMQ queue, and save them into the Inbox database table and handle them from there.
