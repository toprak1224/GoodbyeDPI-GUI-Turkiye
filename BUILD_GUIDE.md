# GoodbyeDPI V2 GUI - Derleme Rehberi

## 🛠️ Visual Studio ile Derleme

### Gereksinimler
- **Visual Studio 2022** (Community, Professional veya Enterprise)
- **.NET 6.0 SDK** veya üstü
- **Windows 10/11**

---

## 📋 Adım Adım Derleme

### 1. Visual Studio Kurulumu

1. [Visual Studio 2022](https://visualstudio.microsoft.com/tr/downloads/) indirin
2. Kurulum sırasında şu workload'u seçin:
   - ✅ **.NET masaüstü geliştirme** (veya ".NET desktop development")

### 2. Projeyi Açma

1. Visual Studio'yu açın
2. **Dosya** → **Aç** → **Proje/Çözüm** seçin
3. `GoodbyeDPI-GUI` klasöründeki `GoodbyeDPI-GUI.sln` dosyasını açın

### 3. Derleme

#### Yöntem A: Visual Studio İçinden
1. Üst menüden **Yapı** → **Çözümü Derle** seçin (veya `Ctrl+Shift+B`)
2. Derleme tamamlandığında çıktı:
   - Debug: `bin\Debug\net6.0-windows\`
   - Release: `bin\Release\net6.0-windows\`

#### Yöntem B: Komut Satırından
```powershell
# GoodbyeDPI-GUI klasörüne gidin
cd GoodbyeDPI-GUI

# Debug derleme
dotnet build

# Release derleme (Dağıtım için önerilen)
dotnet build -c Release

# Tek dosya olarak yayınlama (Önerilen)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

---

## 📦 Dağıtım İçin Yayınlama

### Tek Dosya Olarak Yayınlama (Önerilen)

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

Bu komut `bin\Release\net6.0-windows\win-x64\publish\` klasöründe tek bir `.exe` dosyası oluşturur.

### Visual Studio'dan Yayınlama

1. Solution Explorer'da projeye sağ tıklayın
2. **Yayımla** seçin
3. **Klasör** seçin
4. Ayarları yapılandırın:
   - Hedef çalışma zamanı: `win-x64`
   - Dağıtım modu: `Bağımsız` (Self-contained)
   - Dosya yayımlama seçenekleri: ✅ Tek dosya oluştur

---

## 🎯 Kullanım

### Derlenen Dosyayı Yerleştirme

Derlenen `GoodbyeDPI-GUI.exe` dosyasını şu konuma kopyalayın:

```
GoodbyeDPI-Turkey/
├── x86_64/
│   ├── goodbyedpi.exe    ← Asıl program
│   ├── WinDivert.dll
│   ├── WinDivert64.sys
│   └── GoodbyeDPI-GUI.exe  ← GUI'yi buraya kopyalayın
├── ...
```

Veya:

```
GoodbyeDPI-Turkey/
├── x86_64/
│   ├── goodbyedpi.exe
│   ├── WinDivert.dll
│   └── WinDivert64.sys
├── GoodbyeDPI-GUI.exe      ← GUI'yi ana klasöre de koyabilirsiniz
└── ...
```

### Çalıştırma

1. `GoodbyeDPI-GUI.exe` dosyasına **sağ tıklayın**
2. **Yönetici olarak çalıştır** seçin
3. Mod seçin ve **BAŞLAT** butonuna tıklayın

---

## ⚠️ Önemli Notlar

1. **Yönetici Yetkisi Gerekli**: GoodbyeDPI network paketlerini değiştirdiği için yönetici olarak çalıştırılmalıdır.

2. **WinDivert Dosyaları**: `WinDivert.dll` ve `WinDivert64.sys` dosyalarının `goodbyedpi.exe` ile aynı klasörde olduğundan emin olun.

3. **Antivirüs**: Bazı antivirüs yazılımları yanlış alarm verebilir. Klasörü istisnalara ekleyin.

---

## 🔧 Sorun Giderme

### "goodbyedpi.exe bulunamadı" Hatası
- GUI'yi `goodbyedpi.exe` dosyasının bulunduğu klasöre veya bir üst klasöre taşıyın

### Derleme Hatası: .NET SDK bulunamadı
```powershell
# .NET 6.0 SDK'yı indirin ve kurun
winget install Microsoft.DotNet.SDK.6
```

### WinDivert Hatası
- `WinDivert.dll` ve `WinDivert64.sys` dosyalarının mevcut olduğundan emin olun
- Antivirüs yazılımınızı kontrol edin

---

## 📝 Proje Yapısı

```
GoodbyeDPI-GUI/
├── GoodbyeDPI-GUI.sln      # Visual Studio solution
├── GoodbyeDPI-GUI.csproj   # Proje dosyası
├── App.xaml                # Uygulama kaynakları ve stiller
├── App.xaml.cs             # Uygulama başlangıç kodu
├── MainWindow.xaml         # Ana pencere tasarımı
├── MainWindow.xaml.cs      # Ana pencere mantığı
├── app.manifest            # Yönetici yetki talebi
└── BUILD_GUIDE.md          # Bu dosya
```

---

**Made with ♥ by Toprak**
GoodbyeDPI V2 GUI • Turkey Edition

