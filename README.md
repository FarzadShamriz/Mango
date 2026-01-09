# E-Commerce Microservices Platform

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Azure](https://img.shields.io/badge/Azure%20Service%20Bus-0078D4?style=for-the-badge&logo=microsoft-azure&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)

A modern, scalable e-commerce platform built with microservices architecture using .NET 8.0

[Features](#-features) • [Architecture](#-architecture) • [Getting Started](#-getting-started) • [Services](#-services) • [Technologies](#-technologies)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Architecture](#-architecture)
- [Services](#-services)
- [Technologies](#-technologies)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [API Documentation](#-api-documentation)
- [Project Structure](#-project-structure)
- [Database Migrations](#-database-migrations)
- [Running the Application](#-running-the-application)
- [Branches](#-branches)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

**Mango** is a comprehensive e-commerce microservices platform designed to demonstrate modern software architecture patterns. The application is built using .NET 8.0 and follows microservices principles, enabling scalability, maintainability, and independent deployment of services.

The platform provides a complete shopping experience with user authentication, product catalog management, shopping cart functionality, order processing with payment integration, coupon system, email notifications, and reward points.

---

## ✨ Features

- 🔐 **Authentication & Authorization** - JWT-based authentication with ASP.NET Core Identity
- 🛍️ **Product Management** - Complete CRUD operations for product catalog
- 🛒 **Shopping Cart** - Persistent shopping cart with session management
- 💳 **Order Processing** - Order management with Stripe payment integration
- 🎫 **Coupon System** - Discount coupon management and validation
- 📧 **Email Notifications** - Asynchronous email service for order confirmations
- 🎁 **Reward Points** - Loyalty program with reward points tracking
- 📡 **Message Bus** - Dual messaging support: Azure Service Bus and RabbitMQ for asynchronous communication
- 📚 **API Documentation** - Swagger/OpenAPI documentation for all services
- 🎨 **Web Frontend** - ASP.NET Core MVC web application

---

## 🏗️ Architecture

The application follows a **microservices architecture** pattern where each service is independently deployable and responsible for a specific business domain. Services communicate through:

- **Synchronous Communication**: HTTP/REST APIs for direct service-to-service calls
- **Asynchronous Communication**: Message bus integration supporting both **Azure Service Bus** (main branch) and **RabbitMQ** (feature branch) for event-driven messaging

> **Note**: The project supports both messaging providers. Switch branches to use different message bus implementations:
> - **Main branch**: Azure Service Bus implementation
> - **RabbitMQ branch**: RabbitMQ implementation with the same message bus abstraction

```
┌─────────────────┐
│   Mango.Web     │  (Frontend - ASP.NET Core MVC)
└────────┬────────┘
         │
         ├─────────────────────────────────────┐
         │                                     │
    ┌────▼────┐                          ┌────▼────┐
    │ AuthAPI │                          │ProductAPI│
    └─────────┘                          └─────────┘
         │                                     │
    ┌────▼────┐                          ┌────▼────┐
    │CartAPI  │                          │CouponAPI│
    └────┬────┘                          └─────────┘
         │
    ┌────▼────┐
    │OrderAPI │
    └────┬────┘
         │
    ┌────▼──────────────────────────────┐
    │     Message Bus Layer             │
    │  ┌──────────────┐  ┌────────────┐│
    │  │Azure Service │  │  RabbitMQ  ││
    │  │     Bus      │  │  (Branch)  ││
    │  └──────────────┘  └────────────┘│
    └────┬──────────────────────────────┘
         │
    ┌────┴────┐    ┌────┴────┐
    │EmailAPI │    │RewardAPI│
    └─────────┘    └─────────┘
```

---

## 🔧 Services

### 1. **AuthAPI** 🔐
- User registration and authentication
- JWT token generation and validation
- Role-based authorization
- ASP.NET Core Identity integration

### 2. **ProductAPI** 📦
- Product catalog management
- CRUD operations for products
- Product search and filtering
- JWT-protected endpoints

### 3. **ShoppingCartAPI** 🛒
- Shopping cart management
- Add/remove/update cart items
- Cart persistence
- Integration with ProductAPI

### 4. **OrderAPI** 📋
- Order creation and management
- Stripe payment processing
- Order status tracking
- Integration with CartAPI and CouponAPI

### 5. **CouponAPI** 🎫
- Coupon/discount code management
- Coupon validation
- Discount calculation

### 6. **EmailAPI** 📧
- Asynchronous email processing
- Order confirmation emails
- Message bus consumer (Azure Service Bus or RabbitMQ)
- Supports multiple message types: order creation, cart checkout, user registration

### 7. **RewardAPI** 🎁
- Reward points management
- Points calculation and tracking
- Message bus consumer (Azure Service Bus or RabbitMQ)
- Processes reward points based on order events

### 8. **Mango.Web** 🌐
- ASP.NET Core MVC frontend
- User interface for all services
- Cookie-based authentication
- Service integration layer

### 9. **Mango.MessageBus** 📡
- Shared library for message bus operations
- Azure Service Bus integration (main branch)
- RabbitMQ integration (feature branch)
- Message publishing abstraction layer
- Supports multiple messaging providers through unified interface

---

## 🛠️ Technologies

### Core Technologies
- **.NET 8.0** - Latest .NET framework
- **C#** - Primary programming language
- **ASP.NET Core Web API** - RESTful API framework
- **ASP.NET Core MVC** - Web application framework

### Data & Persistence
- **Entity Framework Core 8.0** - ORM framework
- **SQL Server** - Relational database
- **Entity Framework Migrations** - Database versioning

### Authentication & Security
- **ASP.NET Core Identity** - User management
- **JWT Bearer Authentication** - Token-based authentication
- **Cookie Authentication** - Web frontend authentication

### Messaging & Integration
- **Azure Service Bus** - Cloud message queue service (main branch)
- **Azure.Messaging.ServiceBus** - Service Bus SDK
- **RabbitMQ** - Open-source message broker (feature branch)
- **RabbitMQ.Client** - RabbitMQ .NET client library

### Payment Processing
- **Stripe** - Payment gateway integration

### Utilities & Libraries
- **AutoMapper** - Object-to-object mapping
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI documentation
- **Newtonsoft.Json** - JSON serialization

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- **Message Bus** (choose one):
  - [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) namespace (main branch)
  - [RabbitMQ](https://www.rabbitmq.com/download.html) server (RabbitMQ branch) - Can run locally via Docker
- [Stripe Account](https://stripe.com/) (for payment processing)
- [Docker](https://www.docker.com/products/docker-desktop/) (optional, for running RabbitMQ locally)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/Mango.git
cd Mango
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Connection Strings

Update the `appsettings.json` files in each service with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MangoAuthDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Configure Message Bus

#### Option A: Azure Service Bus (Main Branch)

Update the connection string in `Mango.MessageBus/MessageBus.cs` or use configuration:

```csharp
private string connectionString = "YOUR_AZURE_SERVICE_BUS_CONNECTION_STRING";
```

#### Option B: RabbitMQ (RabbitMQ Branch)

1. **Install RabbitMQ** (using Docker recommended):

```bash
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

2. **Access RabbitMQ Management UI**: `http://localhost:15672` (default credentials: `guest`/`guest`)

3. **Configure RabbitMQ connection** in `appsettings.json`:

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "TopicAndQueueNames": {
    "OrderCreateTopic": "MangoOrderCreation",
    "emailshoppingcarttopic": "MangoEmailShoppingCart",
    "registeruser": "MangoRegisterUser"
  }
}
```

The RabbitMQ implementation uses background services (`RabbitMQOrderConsumer`, `RabbitMQCartConsumer`, `RabbitMQAuthConsumer`) that automatically connect and consume messages.

### 5. Configure Stripe (for OrderAPI)

Add your Stripe keys to `Mango.Services.OrderAPI/appsettings.json`:

```json
{
  "Stripe": {
    "SecretKey": "YOUR_STRIPE_SECRET_KEY",
    "PublishableKey": "YOUR_STRIPE_PUBLISHABLE_KEY"
  }
}
```

### 6. Run Database Migrations

Migrations are automatically applied when each service starts. Alternatively, you can run them manually:

```bash
cd Mango.Services.AuthAPI
dotnet ef database update

cd ../Mango.Services.ProductAPI
dotnet ef database update

# Repeat for other services...
```

### 7. Run the Services

You can run all services from Visual Studio using the solution file, or run them individually:

```bash
# Terminal 1 - AuthAPI
cd Mango.Services.AuthAPI
dotnet run

# Terminal 2 - ProductAPI
cd Mango.Services.ProductAPI
dotnet run

# Terminal 3 - ShoppingCartAPI
cd Mango.Services.ShoppingCartAPI
dotnet run

# Terminal 4 - OrderAPI
cd Mango.Services.OrderAPI
dotnet run

# Terminal 5 - CouponAPI
cd Mango.Services.Coupon
dotnet run

# Terminal 6 - EmailAPI
cd Mango.Services.EmailAPI
dotnet run

# Terminal 7 - RewardAPI
cd Mango.Services.RewardAPI
dotnet run

# Terminal 8 - Web Frontend
cd Mongo.Web
dotnet run
```

---

## ⚙️ Configuration

### Service URLs

Configure service URLs in `Mongo.Web/appsettings.json`:

```json
{
  "ServiceUrls": {
    "AuthAPI": "https://localhost:7001",
    "ProductAPI": "https://localhost:7002",
    "ShoppingCartAPI": "https://localhost:7003",
    "OrderAPI": "https://localhost:7004",
    "CouponAPI": "https://localhost:7005"
  }
}
```

### JWT Configuration

Configure JWT settings in `Mango.Services.AuthAPI/appsettings.json`:

```json
{
  "ApiSettings": {
    "JwtOptions": {
      "Secret": "YOUR_SECRET_KEY_HERE",
      "Issuer": "MangoAuth",
      "Audience": "Mango"
    }
  }
}
```

### Message Bus Configuration

#### For RabbitMQ Branch

Configure RabbitMQ settings in each service's `appsettings.json`:

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "TopicAndQueueNames": {
    "OrderCreateTopic": "MangoOrderCreation",
    "emailshoppingcarttopic": "MangoEmailShoppingCart",
    "registeruser": "MangoRegisterUser"
  }
}
```

#### For Azure Service Bus (Main Branch)

Configure Azure Service Bus connection string in `Mango.MessageBus/MessageBus.cs`:

```csharp
private string connectionString = "Endpoint=sb://your-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YOUR_KEY";
```

---

## 📚 API Documentation

Each API service includes Swagger documentation. Once a service is running, access the Swagger UI at:

- **AuthAPI**: `https://localhost:7001/swagger`
- **ProductAPI**: `https://localhost:7002/swagger`
- **ShoppingCartAPI**: `https://localhost:7003/swagger`
- **OrderAPI**: `https://localhost:7004/swagger`
- **CouponAPI**: `https://localhost:7005/swagger`
- **EmailAPI**: `https://localhost:7006/swagger`
- **RewardAPI**: `https://localhost:7007/swagger`

---

## 📁 Project Structure

```
Mango/
├── Mango.MessageBus/              # Shared message bus library
├── Mango.Services.AuthAPI/        # Authentication service
├── Mango.Services.ProductAPI/     # Product management service
├── Mango.Services.ShoppingCartAPI/# Shopping cart service
├── Mango.Services.OrderAPI/       # Order processing service
├── Mango.Services.Coupon/         # Coupon management service
├── Mango.Services.EmailAPI/       # Email notification service
├── Mango.Services.RewardAPI/      # Reward points service
├── Mongo.Web/                     # Web frontend application
└── Mango.sln                      # Visual Studio solution file
```

Each service follows a consistent structure:

```
ServiceName/
├── Controllers/       # API controllers
├── Data/              # DbContext and database configuration
├── Models/            # Domain models and DTOs
├── Services/          # Business logic services
├── Migrations/        # Entity Framework migrations
├── Program.cs         # Application entry point
└── appsettings.json   # Configuration file
```

---

## 🗄️ Database Migrations

The application uses Entity Framework Core Code-First migrations. Migrations are automatically applied when services start in development mode.

To create a new migration:

```bash
cd Mango.Services.ServiceName
dotnet ef migrations add MigrationName
```

To apply migrations manually:

```bash
dotnet ef database update
```

---

## 🏃 Running the Application

### Prerequisites for Messaging

**For RabbitMQ Branch:**
- Start RabbitMQ server (if using Docker: `docker start some-rabbit` or use the Docker command from configuration section)

**For Azure Service Bus (Main Branch):**
- Ensure Azure Service Bus namespace is accessible
- Connection string is configured correctly

### Option 1: Visual Studio

1. Open `Mango.sln` in Visual Studio 2022
2. Set multiple startup projects:
   - Right-click solution → Properties → Multiple startup projects
   - Set all services to "Start"
3. Press F5 to run all services

### Option 2: Command Line

Run each service in a separate terminal window as described in the [Getting Started](#-getting-started) section.

**Important**: Make sure RabbitMQ server is running if you're using the RabbitMQ branch:
```bash
# Check if RabbitMQ is running
docker ps | grep rabbit

# If not running, start it
docker start some-rabbit
```

### Option 3: Docker (Future Enhancement)

Docker support can be added for containerized deployment. Currently, RabbitMQ can be run via Docker as shown in the configuration section.

---

## 🔒 Security Considerations

- **JWT Tokens**: Use strong secret keys in production
- **Connection Strings**: Store sensitive data in User Secrets or Azure Key Vault
- **HTTPS**: All services use HTTPS in production
- **CORS**: Configure CORS policies appropriately
- **API Keys**: Never commit API keys or secrets to version control

---

## 🌿 Branches

This repository contains multiple branches with different message bus implementations:

- **`main`** - Uses Azure Service Bus for asynchronous messaging
- **`rabbitmq`** or **`feature/rabbitmq`** - Uses RabbitMQ for asynchronous messaging

Both implementations provide the same functionality using different message bus technologies. Choose the branch that suits your infrastructure requirements.

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Working with Different Message Bus Implementations

If you're working on messaging features:
- Test your changes with both Azure Service Bus and RabbitMQ implementations
- Ensure the message bus abstraction remains consistent across implementations
- Update documentation if adding new messaging features

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Farzad Shamriz**

- GitHub: [@FarzadShamriz](https://github.com/FarzadShamriz)
- LinkedIn: [Farzad Shamriz](https://linkedin.com/in/farzad-shamriz-8460a2a9)

---

## 🙏 Acknowledgments

- Built with [.NET](https://dotnet.microsoft.com/)
- Inspired by microservices architecture patterns
- Thanks to the open-source community

---

<div align="center">

**⭐ If you find this project helpful, please consider giving it a star! ⭐**

Made with ❤️ using .NET 8.0

</div>

