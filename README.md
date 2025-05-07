# DotNetMicroservicesWorkshop

Bu proje, .NET Core 5.0, Docker, RabbitMQ, Ocelot API Gateway, MongoDB, SQL Server, SignalR ve Microsoft Identity Server gibi teknolojileri temel alan bir **Microservices (Mikroservis) mimarisi** eğitiminde geliştirilmiştir.

## 🚀 Hedef
Modern mikroservis mimarisini kullanarak, ölçeklenebilir ve dağıtık sistemlerin geliştirilmesini deneyimlemek.

## 🧩 Kullanılan Teknolojiler

| Teknoloji            | Açıklama                                            |
|----------------------|-----------------------------------------------------|
| **.NET Core 8.0**     | Mikroservis API'lerin geliştirilmesi               |
| **Web API**           | RESTful servisler                                  |
| **Docker**            | Uygulamaların container içinde çalıştırılması      |
| **RabbitMQ**          | Mesajlaşma altyapısı                               |
| **Ocelot API Gateway**| API geçidi, routing ve yetkilendirme               |
| **MongoDB**           | NoSQL veritabanı                                   |
| **SQL Server**        | Relational veritabanı                              |
| **SignalR**           | Gerçek zamanlı haberleşme                          |
| **Microsoft Identity**| Kimlik doğrulama ve yetkilendirme çözümü          |


## ⚙️ Kurulum

### Gereksinimler

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio 2022](https://visualstudio.microsoft.com/tr/vs/)

### Başlatma

```bash
# 
git clone https://github.com/erdincdegirmenci/netcore-microservices.git

# Tüm servisleri docker ile ayağa kaldırın
docker-compose up --build

