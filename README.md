# GoodbyeDPI V2 GUI • Turkey Edition

Modern WPF arayüzü, GoodbyeDPI Türkiye sürümündeki tüm CMD modlarını tek tıkla yönetir. Geçici çalıştırma ve kalıcı servis kurulumu desteklenir. Türkçe mod açıklamaları, canlı log, gizli kaydırma çubuğu.

<img width="559" height="900" alt="image" src="https://github.com/user-attachments/assets/c2a50d68-b131-4720-82f1-3fc84ef80543" />


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

Önerilen klasör yapısı:
goodbyedpi-0.2.3rc3-turkeyy/
GoodbyeDPI-GUI.exe
x86_64/
goodbyedpi.exe
WinDivert.dll
WinDivert64.sys
x86/
goodbyedpi.exe
WinDivert.dll
WinDivert32.sys

## Kullanım
1. `GoodbyeDPI-GUI.exe` dosyasını yukarıdaki yapıyla aynı klasöre koy.
2. Sağ tık → **Yönetici olarak çalıştır**.
3. **Geçici Mod** sekmesinden mod seçip BAŞLAT (program kapanınca durur).
4. **Kalıcı Mod** sekmesinden mod seçip “SERVİS OLARAK KUR” (Windows servisi; açılışta otomatik).
5. Servisi kaldırmak için “Servisi Kaldır (service_remove.cmd)” butonu.

### Modlar (özet)
- `turkey_dnsredir`: Mod 5 + TTL 5 + Yandex DNS
- `turkey_dnsredir_alternative_superonline`: TTL 3
- `turkey_dnsredir_alternative2_superonline`: Mod 5
- `turkey_dnsredir_alternative3_superonline`: TTL 3 + Yandex DNS
- `turkey_dnsredir_alternative4_superonline`: Mod 5 + Yandex DNS
- `turkey_dnsredir_alternative5_superonline`: **Mod 9 + Yandex DNS (Önerilen)**
- `turkey_dnsredir_alternative6_superonline`: Mod 9

Aynı adlarla `service_install_*` modları kalıcı servis kurulumudur.

## SmartScreen Uyarısı (Windows 10/11)
- Kod imzası olmadığı için “Yayıncı doğrulanamadı” uyarısı görebilirsiniz.
- Çalıştırmak için:
  - Dosyaya sağ tık → Özellikler → “Engellemeyi kaldır” işaretle → Uygula.
  - Açılışta uyarı gelirse “More info” → “Run anyway”.
- SmartScreen’i geçici kapatmak (önerilmez):
  - Windows 10: Ayarlar → Güncelleştirme ve Güvenlik → Windows Güvenliği → Uygulama ve tarayıcı denetimi → SmartScreen ayarını “Kapalı” yapın.
  - Windows 11: Ayarlar → Gizlilik ve güvenlik → Windows Güvenliği → Uygulama ve tarayıcı denetimi → SmartScreen’i “Kapalı” yapın.
  - İşiniz bitince yeniden açmanız tavsiye edilir.

## Antivirüs Notu (Kaspersky)
- Kaspersky bu aracı engelleyebiliyor veya sürücüyü (WinDivert) çalıştırmıyor.
- Çözüm: Kaspersky’yi tamamen kaldırın veya farklı bir AV/Windows Defender kullanın. Sadece devre dışı bırakmak çoğu zaman yeterli olmuyor.

## Derleme (opsiyonel)
- `dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true`

## Lisans
- GoodbyeDPI ve WinDivert ilgili lisanslarına tabidir.
- Bu GUI için kendi lisansını ekleyebilirsin.
