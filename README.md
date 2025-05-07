# DotNetMicroservices

This project was developed as part of a **Microservices architecture** training, based on technologies such as .NET Core 8.0, Docker, RabbitMQ, Ocelot API Gateway, MongoDB, SQL Server, SignalR, and Microsoft Identity Server.

## 🚀 Goal

To gain hands-on experience in building scalable and distributed systems using modern microservices architecture.

## 🧩 Technologies Used

| Technology             | Description                                           |
|------------------------|-------------------------------------------------------|
| **.NET Core 8.0**       | Development of microservice APIs                     |
| **Web API**             | RESTful services                                     |
| **Docker**              | Containerization of applications                     |
| **RabbitMQ**            | Messaging infrastructure                             |
| **Ocelot API Gateway**  | API gateway for routing and authorization            |
| **MongoDB**             | NoSQL database                                       |
| **SQL Server**          | Relational database                                  |
| **SignalR**             | Real-time communication                              |
| **Microsoft Identity**  | Authentication and authorization solution            |

## ⚙️ Setup

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Getting Started

```bash
# Clone the repository
git clone https://github.com/erdincdegirmenci/netcore-microservices.git

# Navigate into the project folder
cd netcore-microservices

# Start all services using Docker
docker-compose up --build
