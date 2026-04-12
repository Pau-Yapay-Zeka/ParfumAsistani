# 🌸 Parfüm Asistanı: Yapay Zeka Destekli Koku Rehberi

![Project Version](https://img.shields.io/badge/version-1.0.0-blue)
![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-purple)
![JavaScript](https://img.shields.io/badge/JavaScript-ES6+-yellow)
![Tailwind](https://img.shields.io/badge/Tailwind-CSS%20CLI-38B2AC)
![AI](https://img.shields.io/badge/AI-Gemini%20Pro%20API-red)

Parfüm Asistanı, kullanıcıların kişisel tercihlerine, yaşlarına ve koku zevklerine göre en uygun parfümleri yapay zeka desteğiyle analiz eden ve 3 parfüm önerisinde bulunan modern bir web uygulamasıdır.

## 🚀 Proje Özeti
Bu proje, karmaşık koku notalarını ve kullanıcı profillerini birleştirerek kişiselleştirilmiş bir kullanıcı deneyimi sunar. Kullanıcılar, etkileşimli 3D kartlar aracılığıyla koku notalarını keşfedebilir ve yapay zeka motorumuzdan saniyeler içinde nokta atışı tavsiyeler alabilirler.

## 🛠️ Teknoloji Yığını
* **Backend:** ASP.NET Core Web API
* **Frontend:** Single Page Application (SPA) mimarisi, Vanilla JavaScript
* **Veritabanı:** SQL Server (Entity Framework Core ile)
* **Tasarım:** Tailwind CSS (CLI tabanlı derleme) & Bootstrap 5
* **Yapay Zeka:** Gemini Pro API

## 🏗️ Mimari Yapı (Web API + SPA)
Proje, **Kavramların Ayrılığı (Separation of Concerns)** prensibine uygun olarak iki ana katmandan oluşur:
1. **Arka Uç (Backend):** Veritabanı yönetimi ve AI servisleri ile iletişimi sağlayan RESTful API.
2. **Ön Yüz (Frontend):** Sunucu yükünü minimize eden, hızlı ve akıcı bir deneyim sunan modern SPA yapısı.

## 🧠 Yapay Zeka Entegrasyonu
Proje, öneri motoru olarak gelişmiş Gemini dil modelini kullanır. 
* **Süreç:** Kullanıcı verileri (yaş, cinsiyet, favori notalar) JSON formatında AI servisine iletilir.
* **Analiz:** AI, binlerce parfüm kombinasyonunu tarayarak en uyumlu 3 sonucu yapılandırılmış veri olarak döndürür.
* **Çıktı Kontrolü:** Gelen yanıtlar, `SafeParseAPIResponse` metodolojisi ile parse edilerek hatalı veya eksik veri oluşumu engellenir.

## ✨ Öne Çıkan Teknik Detaylar
* **SOLID & Clean Code:** Proje, tek sorumluluk (Single Responsibility) ve bağımlılıkların tersine çevrilmesi (Dependency Inversion) prensiplerine sadık kalınarak geliştirilmiştir.
* **3D Flip Card UI:** Koku notaları, kullanıcı etkileşimini artırmak için CSS 3D transformasyonları kullanılarak tasarlanmıştır.
* **Teknik Borç Yönetimi:** Tailwind CSS'in CDN yerine **CLI (Command Line Interface)** üzerinden derlenmesi sağlanarak projenin teknik borcu %5'in altına çekilmiş ve üretim (production) standartlarına getirilmiştir.
* **Asenkron Programlama:** Tüm API ve AI süreçleri `Task-based Asynchronous Pattern (TAP)` ile yönetilerek kullanıcı arayüzünün donması engellenmiştir.

## 📈 Karşılaşılan Zorluklar ve Çözümler
* **JSON Veri Tutarlılığı:** Yapay zekadan gelen yanıtların bazen standart dışı formatlarda olması sorunu, gelişmiş regex cımbızlama ve `JsonDocument` analiz metodolojileri ile aşılmıştır.
* **Tasarım Performansı:** Sayfa yüklenme hızını optimize etmek amacıyla CSS ve JS dosyaları parçalanmış modüler (Modular Design) yapıya geçirilmiştir.

## 📝 Kurulum ve Kullanım

1. **Repoyu Klonlayın:**
   ```bash
   git clone [https://github.com/sudemhosek/ParfumAsistani.git](https://github.com/sudemhosek/ParfumAsistani.git)
   ```

2. **Veritabanını Güncelleyin:**
   `appsettings.json` içindeki bağlantı dizesini düzenleyin ve Migration'ları uygulayın:
   ```bash
   dotnet ef database update
   ```

3. **Tailwind CSS'i Derleyin:**
   ```bash
   npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/tailwind.css --watch
   ```

4. **Uygulamayı Çalıştırın:**
   ```bash
   dotnet run
   ```
