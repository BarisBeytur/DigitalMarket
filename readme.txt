
# Proje Kurulumu ve Calistirma

## Gereksinimler
- Docker ve Docker Compose yuklu olmali.

## Redis'i Baslatma
1 - Projeyi klonladiktan sonra, Docker Compose kullanarak Redis'i baslatin:

VS uzerinden gelistirici powershell acildiktan sonra: 
2 - docker-compose up -d

VS harici bir IDE icin:
2 - cd ile proje dizinine gidin ve asagidaki komutu calistirin: docker-compose up -d

3 - AppSetting uzerinden ConnectionStrings altindaki "DigitalMarketDbConnection" alanina connection stringinizi girin.

4 - Gelistirici powershell uzerinden: 
Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data  -> komutunu calistirarak veritabanini olusturun.

5 - Projeyi calistirabilirsiniz.

Project Tech Stack : 
.NET Core 
Microsoft SQL Server
Entity Framework Core
Identity
JWT
AutoMapper
FluentValidation
AutoFac
Redis
Docker
MediatR

Project Architecture : 
CQRS
Mediator
Generic Repository
Unit Of Work


Database Migration Command:
Add-Migration mig_init -StartupProject DigitalMarket.Api -Project DigitalMarket.Data

Database Update Command:
Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data


## EKSTRA

Servisleri Durdurmak icin:
docker-compose down

Servislerin Durumunu Goruntulemek icin:
docker-compose ps



PROJE POSTMAN DÖKÜMANTASYONU : https://documenter.getpostman.com/view/28601501/2sA3s4mVqH

