# Digital Market

Patika & Papara .NET Bootcamp Final Project

---

## Proje Hakkında
Digital Market, .NET Core kullanılarak geliştirilmiş bir dijital market platformu projesidir. Proje kullanıcıların dijital olarak satılabilen ürünler satın alabilmesini ve her satın alım sonucu cashback sistemi ile puan ve indirimler kazanması üzerine kurgulanmıştır. Proje, modern yazılım geliştirme prensipleri ve teknolojileriyle tasarlanmıştır. 

## Postman Dökümantasyonu
Proje API dokümantasyonu için aşağıdaki Postman linkini kullanabilirsiniz:
[Postman Dökümantasyonu](https://documenter.getpostman.com/view/28601501/2sA3s4mVqH)

---

## Proje Kurulumu ve Çalıştırma

### Gereksinimler
- Docker ve Docker Compose kurulu olmalıdır.

### Redis'i Başlatma
1. Projeyi klonladıktan sonra, Docker Compose kullanarak Redis'i başlatın.

#### Visual Studio üzerinden:
```bash
# Geliştirici PowerShell'i açın ve aşağıdaki komutu çalıştırın:
docker-compose up -d
```

#### Diğer IDE'ler için:
```bash
# Proje dizinine gidin ve aşağıdaki komutu çalıştırın:
cd <proje-dizini>
docker-compose up -d
```

2. `appsettings.json` dosyasındaki **ConnectionStrings** bölümünde yer alan `DigitalMarketDbConnection` alanına veritabanı bağlantı stringinizi ekleyin.

3. Veritabanını oluşturmak için aşağıdaki komutu çalıştırın:
```bash
Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data
```

4. Artık projeyi çalıştırabilirsiniz.

---

## Kullanılan Teknolojiler

- **.NET Core**  
- **Microsoft SQL Server**  
- **Entity Framework Core**  
- **Identity**  
- **JWT** (JSON Web Tokens)  
- **AutoMapper**  
- **FluentValidation**  
- **AutoFac**  
- **Redis**  
- **Docker**  
- **MediatR**

---

## Proje Mimarisi

- **CQRS** (Command Query Responsibility Segregation)  
- **Mediator** (MediatR kullanımı)  
- **Generic Repository**  
- **Unit Of Work**

---

## Veritabanı İşlemleri

### Migration Oluşturma Komutu:
```bash
Add-Migration mig_init -StartupProject DigitalMarket.Api -Project DigitalMarket.Data
```

### Veritabanını Güncelleme Komutu:
```bash
Update-Database -StartupProject DigitalMarket.Api -Project DigitalMarket.Data
```

---

## Ekstra Komutlar

### Servisleri Durdurmak:
```bash
docker-compose down
```

### Servislerin Durumunu Görüntülemek:
```bash
docker-compose ps
```

---
