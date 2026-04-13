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
* **Süreç:** Kullanıcı verileri (yaş, cinsiyet, favori notalar ve favori parfümler) JSON formatında AI servisine iletilir.
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
2. Bağımlılıkları (Paketleri) Yükleyin:
Projeyi klonladıktan sonra, C# ve Tailwind CSS için gerekli olan paketleri indirin:
 ```bash
  dotnet restore
   npm install
   ```
3. Yapılandırma Dosyasını Oluşturun 
Güvenlik nedeniyle appsettings.json dosyası repoya dahil edilmemiştir. Ana dizinde appsettings.json adında bir dosya oluşturun ve aşağıdaki şablonu içine yapıştırın:
```bash
JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ParfumDb;Trusted_Connection=True;"
  },
  "Gemini": {
    "ApiKey": "BURAYA_GEMINI_API_ANAHTARINIZI_YAZIN"
  }
}
```
Not: Gemini API anahtarınızı Google AI Studio üzerinden ücretsiz olarak alabilirsiniz.

4. **Veritabanını Güncelleyin:**
   `appsettings.json` içindeki bağlantı dizesini düzenleyin ve Migration'ları uygulayın:
   ```bash
   dotnet ef database update
   ```
5. Örnek Verileri Yükleyin:
Tablolar oluştuktan sonra, arayüzdeki 3D nota kartlarının ve arama motorunun boş görünmemesi için SQL Server'ı açın ve aşağıdaki örnek verileri çalıştırın (Execute):
 ```bash
-- Notalar Tablosu İçin Örnek Veriler
INSERT INTO Notalar (Ad, GorselUrl, Aciklama) VALUES 
('Vanilya', 'https://royalmond.com/wp-content/uploads/Vanilla-06-1000x675.jpg', 'Sıcak, tatlı ve rahatlatıcı klasik bir gurme lezzeti.'),
('Gül', 'https://www.zengardentr.com/shop/ir/87/myassets/products/497/kirmizi-yerli-gul-fidani-1.jpg?revision=1770117976', 'Romantizmin, tutkunun ve zarafetin pudralı, zamansız kokusu.'),
('Karamel', 'https://imgrosetta.mynet.com.tr/file/12201709/12201709-860x480.jpg', 'Gourmand kokuların yanık şekerli, baştan çıkarıcı notası.'),
('Bergamot', 'https://www.kendiparfumunuyap.com/wp-content/uploads/2024/06/Bergamot-Akoru-Bergamot-Kokusu.webp', 'Earl Grey çayına da kokusunu veren, taze ve hafif narenciye dokunuşu.'),
('Sandal Ağacı', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQnl_gQPyzxfJ7Slms3KTx-evkVfgbe1VhviQ&s', 'Kremsi, yumuşak, ruhani ve sütlü bir odunsu doku.'),
('Lavanta', 'https://cdn.karaca.com/cms/lavanta_1_9b004fc0fd.jpg', 'Temiz, aromatik, sabunsu ve sakinleştirici bir ferahlık.');

-- Parfumler Tablosu İçin Örnek Veriler (Arama Motoru Testi İçin)
INSERT INTO Parfumler (Ad, Marka) VALUES 
('Goddess', 'Burberry'),
('Chanel No. 5', 'Chanel'),
('Coco Mademoiselle', 'Chanel'),
('Chance Eau Tendre', 'Chanel'),
('Black Opium', 'Yves Saint Laurent'),
('Libre', 'Yves Saint Laurent'),
('J''adore', 'Dior'),
('Miss Dior', 'Dior'),
('Hypnotic Poison', 'Dior'),
('La Vie Est Belle', 'Lancôme'),
('Idôle', 'Lancôme'),
('Alien', 'Mugler'),
('Angel', 'Mugler'),
('Sauvage', 'Dior'),
('Bleu de Chanel', 'Chanel'),
('Aventus', 'Creed');
   ```
💡 Bilgi: Projenin orijinal veritabanında arama deneyimini zenginleştirmek için en çok tercih edilen 100 kadın ve 100 erkek parfümü bulunmaktadır. Yukarıdaki veriler, uygulamanın arama motorunu test edebilmeniz için eklenmiş kısa bir listedir.

6. **Tailwind CSS'i Derleyin:**
   ```bash
   npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/tailwind.css --watch
   ```

7. **Uygulamayı Çalıştırın:**
   ```bash
   dotnet run
   ```
## 🎨 Modern Arayüz ve Kullanıcı Deneyimi
Parfüm Asistanı, kullanıcıları sıradan filtreleme ekranlarından kurtarıp onlara adım adım rehberlik eden interaktif bir deneyim sunar. Süreç; kullanıcının ismini, yaşını ve temel koku tercihini sisteme girmesiyle başlar. Ardından, parfüm dünyasına ve notalara yabancı olan kullanıcılar düşünülerek özel olarak tasarlanmış 3D dönebilen bilgilendirme kartları ile nota seçimi ekranına geçilir. Dileyen kullanıcılar, yapay zekanın tarzlarını daha iyi analiz edebilmesi için daha önce kullandıkları favori bir parfümü de sisteme ekleyebilirler.

Tüm bu veriler işlendikten sonra sistem, kullanıcıya özel olarak seçilmiş 3 farklı parfüm önerisini, bu seçimleri neden yaptığını açıklayan detaylı sebepleriyle birlikte sunar. Eğer önerilen bir parfüm kullanıcının ilgisini çekmezse, kartın üzerindeki çarpı işaretine tıklaması yeterlidir; sistem reddedilen seçeneği anında yepyeni bir alternatifle değiştirir. Aranan o mükemmel koku bulunduğunda ise, satın alma işlemini kolaylaştırmak için doğrudan Google Alışveriş merkezine yönlendirme yapılır ve güvenli bir satın alma deneyimi sağlanır.

<img width="1870" height="898" alt="image" src="https://github.com/user-attachments/assets/0318d027-0c6e-4919-b88c-8ba6df7206ee" />
<img width="1030" height="909" alt="image" src="https://github.com/user-attachments/assets/b843b134-3581-4203-9c6a-b9ffb08fefaa" />
<img width="1004" height="925" alt="image" src="https://github.com/user-attachments/assets/cc33cd12-78d1-4ec1-93a6-9405287db0bc" />
<img width="1690" height="903" alt="image" src="https://github.com/user-attachments/assets/b5ed4168-d82f-4520-9bd4-30c2c9ae22b6" />
<img width="1412" height="910" alt="image" src="https://github.com/user-attachments/assets/7873d023-900c-430c-979c-068c205643af" />
<img width="1366" height="904" alt="image" src="https://github.com/user-attachments/assets/174a4f29-1608-4ece-90c0-4df29fca0d0a" />
<img width="1742" height="915" alt="image" src="https://github.com/user-attachments/assets/c341882f-23a8-4912-a714-81d6bcb038a9" />






