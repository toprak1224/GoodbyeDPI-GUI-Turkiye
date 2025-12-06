# GoodbyeDPI V2 GUI • Turkey Edition

Modern WPF arayüzü, GoodbyeDPI Türkiye sürümündeki tüm CMD modlarını tek tıkla yönetir. Geçici çalıştırma ve kalıcı servis kurulumu desteklenir. Türkçe mod açıklamaları, canlı log, gizli kaydırma çubuğu.

<img width="559" height="900" alt="GoodbyeDPI GUI Ekran Görüntüsü" src="https://github.com/user-attachments/assets/c2a50d68-b131-4720-82f1-3fc84ef80543" />

## Özellikler
- Tüm `turkey_dnsredir*.cmd` ve `service_install*.cmd` modları gömülü
- Geçici mod (program kapanınca durur) ve kalıcı mod (Windows servisi olarak kurulur)
- Servis kaldırma (service_remove) butonu
- Canlı log, gizli scrollbar, sabit imza/GitHub butonu
- Yönetici izni gerektirir

## Gereksinimler
- Windows 10/11
- Yönetici olarak çalıştırma
- `goodbyedpi.exe`, `WinDivert.dll`, `WinDivert64.sys` (aynı klasörde veya `x86_64/` içinde)

### Önerilen Klasör Yapısı
```text
goodbyedpi-0.2.3rc3-turkeyy/
│
├── GoodbyeDPI-GUI.exe
├── x86_64/
│   ├── goodbyedpi.exe
│   ├── WinDivert.dll
│   └── WinDivert64.sys
└── x86/
    ├── goodbyedpi.exe
    ├── WinDivert.dll
    └── WinDivert32.sys
```


Kullanım
GoodbyeDPI-GUI.exe dosyasını yukarıdaki yapıyla aynı klasöre koyun.

Sağ tık → Yönetici olarak çalıştır.

Geçici Mod sekmesinden mod seçip BAŞLAT (program kapanınca durur).

Kalıcı Mod sekmesinden mod seçip SERVİS OLARAK KUR (Windows servisi; açılışta otomatik başlar).

Servisi kaldırmak için Servisi Kaldır (service_remove.cmd) butonunu kullanın.

Modlar (Özet)
turkey_dnsredir: Mod 5 + TTL 5 + Yandex DNS

turkey_dnsredir_alternative_superonline: TTL 3

turkey_dnsredir_alternative2_superonline: Mod 5

turkey_dnsredir_alternative3_superonline: TTL 3 + Yandex DNS

turkey_dnsredir_alternative4_superonline: Mod 5 + Yandex DNS

turkey_dnsredir_alternative5_superonline: Mod 9 + Yandex DNS (Önerilen)

turkey_dnsredir_alternative6_superonline: Mod 9

Aynı adlarla service_install_* modları kalıcı servis kurulumudur.

Sorun Giderme
SmartScreen Uyarısı (Windows 10/11)
Kod imzası olmadığı için “Yayıncı doğrulanamadı” uyarısı görebilirsiniz.

Çalıştırmak için: Dosyaya sağ tık → Özellikler → “Engellemeyi kaldır” işaretle → Uygula. Açılışta uyarı gelirse “More info” → “Run anyway” diyebilirsiniz.

SmartScreen’i geçici kapatmak (önerilmez): Windows Güvenliği ayarlarından "Uygulama ve tarayıcı denetimi" kısmından SmartScreen'i kapatabilirsiniz.

Antivirüs Notu (Kaspersky vb.)
Kaspersky gibi bazı antivirüsler bu aracı engelleyebilir veya sürücüyü (WinDivert) çalıştırmayabilir.

Çözüm: Antivirüsü tamamen kaldırmak veya Windows Defender kullanmak gerekebilir. Sadece devre dışı bırakmak bazen yeterli olmamaktadır.

Derleme (Opsiyonel)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
Teşekkürler
Bu proje, açık kaynak topluluğunun değerli projeleri üzerine inşa edilmiştir. Aşağıdaki geliştiricilere ve projelere katkılarından dolayı teşekkür ederiz:

- [ValdikSS/GoodbyeDPI](https://github.com/ValdikSS/GoodbyeDPI): Orijinal DPI atlatma motoru ve temel yazılım.
- [cagritaskn/GoodbyeDPI-Turkey](https://github.com/cagritaskn/GoodbyeDPI-Turkey): Türkiye'ye özel konfigürasyonlar ve script düzenlemeleri.

Lisans
GoodbyeDPI ve WinDivert kendi ilgili lisanslarına tabidir.

⚖️ Yasal Uyarı
ÖNEMLİ

Bu uygulamanın kullanımından doğan her türlü yasal sorumluluk kullanan kişiye aittir. Uygulama yalnızca eğitim ve araştırma amaçları ile yazılmış ve düzenlenmiş olup; bu uygulamayı bu şartlar altında kullanmak ya da kullanmamak kullanıcının kendi seçimidir.

Açık kaynak kodlarının paylaşıldığı bu platformdaki düzenlenmiş bu proje, bilgi paylaşımı ve kodlama eğitimi amaçları ile yazılmış ve düzenlenmiştir.
