

Database Migration Command:

Add-Migration mig_init -StartupProject DigitalMarket.Api -Project DigitalMarket.Data


Database Update Command:

Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data




# Proje Kurulumu ve Calistirma

## Gereksinimler
- Docker ve Docker Compose yuklu olmali.

## Redis'i Baslatma
Projeyi klonladiktan sonra, Docker Compose kullanarak Redis'i baslatin:

VS uzerinden gelistirici powershell acildiktan sonra: 
docker-compose up -d

VS harici bir IDE icin:
cd ile proje dizinine gidin ve asagidaki komutu calistirin:
docker-compose up -d

AppSetting uzerinden ConnectionStrings altindaki "DigitalMarketDbConnection" alanina connection stringinizi girin.

Gelistirici powershell uzerinden:
Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data  -> komutunu calistirarak veritabanini olusturun.

Projeyi calistirabilirsiniz.


## EKSTRA

Servisleri Durdurmak icin:
docker-compose down

Servislerin Durumunu Goruntulemek icin:
docker-compose ps



