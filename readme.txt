

Database Migration Command:

Add-Migration mig_init -StartupProject DigitalMarket.Api -Project DigitalMarket.Data


Database Update Command:

Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data


# Proje Kurulumu ve Calistirma

## Gereksinimler
- Docker ve Docker Compose yuklu olmali.

## Redis'i Baslatma
Projeyi klonladiktan sonra, Docker Compose kullanarak Redis'i baslatin:

Gelistirici powershell acildiktan sonra: 
Servisleri Baslatin:
docker-compose up -d

Servisleri Durdurmak icin:
docker-compose down

Servislerin Durumunu Goruntulemek icin:
docker-compose ps



