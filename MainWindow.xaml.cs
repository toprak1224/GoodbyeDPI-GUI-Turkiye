using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GoodbyeDPI_GUI
{
    public partial class MainWindow : Window
    {
        private Process? _goodbyeDpiProcess;
        private bool _isRunning = false;
        private string _exePath = "";
        private readonly SolidColorBrush _greenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 136));
        private readonly SolidColorBrush _redBrush = new SolidColorBrush(Color.FromRgb(255, 51, 102));
        private readonly SolidColorBrush _yellowBrush = new SolidColorBrush(Color.FromRgb(255, 133, 0));
        private readonly SolidColorBrush _grayBrush = new SolidColorBrush(Color.FromRgb(138, 138, 158));
        private readonly SolidColorBrush _blueBrush = new SolidColorBrush(Color.FromRgb(0, 150, 255));

        // ═══════════════════════════════════════════════════════════════════════════════
        // GEÇİCİ MODLAR (turkey_dnsredir*.cmd) - Program kapatılınca kapanır
        // ═══════════════════════════════════════════════════════════════════════════════
        
        private readonly string[] _tempModeNames = new string[]
        {
            "turkey_dnsredir",
            "turkey_dnsredir_alternative_superonline",
            "turkey_dnsredir_alternative2_superonline",
            "turkey_dnsredir_alternative3_superonline",
            "turkey_dnsredir_alternative4_superonline",
            "turkey_dnsredir_alternative5_superonline",
            "turkey_dnsredir_alternative6_superonline"
        };

        private readonly string[] _tempModeArguments = new string[]
        {
            "-5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "--set-ttl 3",
            "-5",
            "--set-ttl 3 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-9 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-9"
        };

        private readonly string[] _tempModeDescriptions = new string[]
        {
            "Mod 5 + TTL 5 + Yandex DNS — Çoğu ISS için çalışır",
            "Sadece TTL 3 — Windows'tan DNS ayarı gerekir",
            "Sadece Mod 5 — Windows'tan DNS ayarı gerekir",
            "TTL 3 + Yandex DNS",
            "Mod 5 + Yandex DNS (TTL yok)",
            "Mod 9 + Yandex DNS (ÖNERİLEN)",
            "Sadece Mod 9 — Windows'tan DNS ayarı gerekir"
        };

        // ═══════════════════════════════════════════════════════════════════════════════
        // KALICI MODLAR (service_install*.cmd) - Windows servisi olarak kurulur
        // ═══════════════════════════════════════════════════════════════════════════════
        
        private readonly string[] _serviceModeNames = new string[]
        {
            "service_install_dnsredir_turkey",
            "service_install_dnsredir_turkey_alternative_superonline",
            "service_install_dnsredir_turkey_alternative2_superonline",
            "service_install_dnsredir_turkey_alternative3_superonline",
            "service_install_dnsredir_turkey_alternative4_superonline",
            "service_install_dnsredir_turkey_alternative5_superonline",
            "service_install_dnsredir_turkey_alternative6_superonline"
        };

        private readonly string[] _serviceModeArguments = new string[]
        {
            "-5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "--set-ttl 3",
            "-5",
            "--set-ttl 3 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-9 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253",
            "-9"
        };

        private readonly string[] _serviceModeDescriptions = new string[]
        {
            "Mod 5 + TTL 5 + Yandex DNS — Çoğu ISS için çalışır",
            "Sadece TTL 3 — Windows'tan DNS ayarı gerekir (Alt.1)",
            "Sadece Mod 5 — Windows'tan DNS ayarı gerekir (Alt.2)",
            "TTL 3 + Yandex DNS (Alt.3)",
            "Mod 5 + Yandex DNS - TTL yok (Alt.4)",
            "Mod 9 + Yandex DNS (Alt.5 - ÖNERİLEN)",
            "Sadece Mod 9 — Windows'tan DNS ayarı gerekir (Alt.6)"
        };

        public MainWindow()
        {
            InitializeComponent();
            FindGoodbyeDpiExe();
            
            UpdateStatusIndicator(_grayBrush);
            AppendLog("╔══════════════════════════════════════════════╗");
            AppendLog("║     GoodbyeDPI V2 GUI - Turkey Edition       ║");
            AppendLog("║        Made with ♥ by Toprak                 ║");
            AppendLog("║     github.com/toprak1224                    ║");
            AppendLog("╚══════════════════════════════════════════════╝");
            AppendLog("");
            
            if (!string.IsNullOrEmpty(_exePath))
            {
                AppendLog("✓ GoodbyeDPI bulundu!");
                AppendLog($"  Konum: {_exePath}");
            }
            else
            {
                AppendLog("⚠ UYARI: goodbyedpi.exe bulunamadı!");
                AppendLog("  GUI'yi x86_64 klasörüne taşıyın.");
            }
            
            AppendLog("");
            AppendLog("📌 GEÇİCİ: Program kapatılınca DPI de kapanır");
            AppendLog("📌 KALICI: Servis olarak kurulur, hep çalışır");
            AppendLog("");
            AppendLog("Hazır. Bir mod seçip başlatın.");
        }

        private void FindGoodbyeDpiExe()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            
            string[] possiblePaths = new string[]
            {
                Path.Combine(baseDir, "goodbyedpi.exe"),
                Path.Combine(baseDir, "x86_64", "goodbyedpi.exe"),
                Path.Combine(baseDir, "..", "x86_64", "goodbyedpi.exe"),
                Path.Combine(baseDir, "..", "..", "x86_64", "goodbyedpi.exe"),
                Path.Combine(Directory.GetParent(baseDir)?.FullName ?? "", "x86_64", "goodbyedpi.exe"),
                Path.Combine(Directory.GetParent(baseDir)?.Parent?.FullName ?? "", "x86_64", "goodbyedpi.exe")
            };

            foreach (var path in possiblePaths)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        _exePath = Path.GetFullPath(path);
                        break;
                    }
                }
                catch { }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                var result = MessageBox.Show(
                    "GoodbyeDPI (geçici mod) hala çalışıyor.\n\nKapatmak istediğinize emin misiniz?\n\n(Kalıcı servis kurduysanız o çalışmaya devam eder)",
                    "Uyarı",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    StopGoodbyeDpi();
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void TempModeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TempModeDescription != null && TempModeComboBox.SelectedIndex >= 0 && TempModeComboBox.SelectedIndex < _tempModeDescriptions.Length)
            {
                TempModeDescription.Text = _tempModeDescriptions[TempModeComboBox.SelectedIndex];
            }
        }

        private void ServiceModeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServiceModeDescription != null && ServiceModeComboBox.SelectedIndex >= 0 && ServiceModeComboBox.SelectedIndex < _serviceModeDescriptions.Length)
            {
                ServiceModeDescription.Text = _serviceModeDescriptions[ServiceModeComboBox.SelectedIndex];
            }
        }

        private void GitHubBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/toprak1224",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // GEÇİCİ MOD - BAŞLAT (turkey_dnsredir*.cmd gibi)
        // ═══════════════════════════════════════════════════════════════════════════════
        private async void TempStartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning) return;

            if (string.IsNullOrEmpty(_exePath) || !File.Exists(_exePath))
            {
                AppendLog("❌ HATA: goodbyedpi.exe bulunamadı!");
                MessageBox.Show(
                    "goodbyedpi.exe bulunamadı!\n\nLütfen bu GUI'yi GoodbyeDPI klasöründe çalıştırın.",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedMode = TempModeComboBox.SelectedIndex;
            if (selectedMode < 0 || selectedMode >= _tempModeArguments.Length)
            {
                AppendLog("❌ HATA: Geçersiz mod seçimi!");
                return;
            }

            string arguments = _tempModeArguments[selectedMode];
            string modeName = _tempModeNames[selectedMode];

            TempStartButton.IsEnabled = false;
            TempStopButton.IsEnabled = true;
            TempModeComboBox.IsEnabled = false;

            UpdateStatusIndicator(_yellowBrush);
            StatusText.Text = "Başlatılıyor...";
            StatusDescription.Text = "GEÇİCİ mod yükleniyor";

            AppendLog($"");
            AppendLog($"▶ GEÇİCİ MOD: {modeName}.cmd");
            AppendLog($"  Argümanlar: {arguments}");

            try
            {
                await Task.Run(() => StartGoodbyeDpi(arguments));
            }
            catch (Exception ex)
            {
                AppendLog($"❌ HATA: {ex.Message}");
                StopGoodbyeDpi();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // GEÇİCİ MOD - DURDUR
        // ═══════════════════════════════════════════════════════════════════════════════
        private void TempStopButton_Click(object sender, RoutedEventArgs e)
        {
            StopGoodbyeDpi();
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // KALICI MOD - SERVİS KUR (service_install*.cmd gibi)
        // ═══════════════════════════════════════════════════════════════════════════════
        private async void ServiceInstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_exePath) || !File.Exists(_exePath))
            {
                AppendLog("❌ HATA: goodbyedpi.exe bulunamadı!");
                MessageBox.Show(
                    "goodbyedpi.exe bulunamadı!\n\nLütfen bu GUI'yi GoodbyeDPI klasöründe çalıştırın.",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedMode = ServiceModeComboBox.SelectedIndex;
            if (selectedMode < 0 || selectedMode >= _serviceModeArguments.Length)
            {
                AppendLog("❌ HATA: Geçersiz mod seçimi!");
                return;
            }

            string arguments = _serviceModeArguments[selectedMode];
            string modeName = _serviceModeNames[selectedMode];

            var result = MessageBox.Show(
                $"'{modeName}' servisi kurulacak.\n\n" +
                "Bu işlem:\n" +
                "• GoodbyeDPI'yı Windows servisi olarak kurar\n" +
                "• Bilgisayar açılınca otomatik başlar\n" +
                "• Sen kaldırana kadar çalışır\n\n" +
                "Devam edilsin mi?",
                "Servis Kurulumu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            ServiceInstallButton.IsEnabled = false;
            ServiceModeComboBox.IsEnabled = false;

            UpdateStatusIndicator(_blueBrush);
            StatusText.Text = "Kuruluyor...";
            StatusDescription.Text = "Servis kuruluyor";

            AppendLog($"");
            AppendLog($"═══════════════════════════════════════════════");
            AppendLog($"📦 KALICI SERVİS KURULUMU: {modeName}.cmd");
            AppendLog($"  Argümanlar: {arguments}");

            try
            {
                await Task.Run(() => InstallService(arguments));

                Dispatcher.Invoke(() =>
                {
                    UpdateStatusIndicator(_greenBrush);
                    StatusText.Text = "Servis Aktif";
                    StatusDescription.Text = "GoodbyeDPI servisi çalışıyor";
                    AppendLog($"✓ Servis başarıyla kuruldu ve başlatıldı!");
                    AppendLog($"═══════════════════════════════════════════════");

                    MessageBox.Show(
                        "GoodbyeDPI servisi başarıyla kuruldu!\n\n" +
                        "• Servis şu anda çalışıyor\n" +
                        "• Bilgisayar yeniden başlatılsa bile çalışacak\n" +
                        "• Kaldırmak için 'Servisi Kaldır' butonunu kullan",
                        "Başarılı",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    AppendLog($"❌ HATA: {ex.Message}");
                    UpdateStatusIndicator(_redBrush);
                    StatusText.Text = "Hata";
                    StatusDescription.Text = "Servis kurulamadı";
                });
            }

            ServiceInstallButton.IsEnabled = true;
            ServiceModeComboBox.IsEnabled = true;
        }

        private void InstallService(string arguments)
        {
            // Önce mevcut servisi kaldır
            RunScCommand("stop", "GoodbyeDPI");
            RunScCommand("delete", "GoodbyeDPI");

            // Yeni servisi oluştur
            string binPath = $"\\\"{_exePath}\\\" {arguments}";
            
            var psi = new ProcessStartInfo
            {
                FileName = "sc",
                Arguments = $"create \"GoodbyeDPI\" binPath= \"{binPath}\" start= \"auto\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Dispatcher.Invoke(() => AppendLog($"  sc create GoodbyeDPI..."));

            using (var process = Process.Start(psi))
            {
                process?.WaitForExit(10000);
                string output = process?.StandardOutput.ReadToEnd() ?? "";
                string error = process?.StandardError.ReadToEnd() ?? "";
                
                Dispatcher.Invoke(() =>
                {
                    if (!string.IsNullOrEmpty(output)) AppendLog($"  {output.Trim()}");
                    if (!string.IsNullOrEmpty(error)) AppendLog($"  ⚠ {error.Trim()}");
                });
            }

            // Açıklama ekle
            psi.Arguments = "description \"GoodbyeDPI\" \"Turkiye icin DNS zorlamasini kaldirir - GUI ile kuruldu\"";
            using (var process = Process.Start(psi))
            {
                process?.WaitForExit(5000);
            }

            // Servisi başlat
            Dispatcher.Invoke(() => AppendLog($"  sc start GoodbyeDPI..."));
            psi.Arguments = "start \"GoodbyeDPI\"";
            using (var process = Process.Start(psi))
            {
                process?.WaitForExit(10000);
                string output = process?.StandardOutput.ReadToEnd() ?? "";
                Dispatcher.Invoke(() =>
                {
                    if (!string.IsNullOrEmpty(output)) AppendLog($"  {output.Trim()}");
                });
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // SERVİS KALDIRMA - service_remove.cmd
        // ═══════════════════════════════════════════════════════════════════════════════
        private async void ServiceRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "GoodbyeDPI servisini kaldırmak istediğinize emin misiniz?\n\n" +
                "Bu işlem:\n" +
                "• GoodbyeDPI servisini durduracak ve kaldıracak\n" +
                "• WinDivert sürücülerini kaldıracak",
                "Servisi Kaldır (service_remove)",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            ServiceRemoveBtn.IsEnabled = false;

            AppendLog("");
            AppendLog("═══════════════════════════════════════════════");
            AppendLog("🗑️ service_remove.cmd çalıştırılıyor...");

            try
            {
                await Task.Run(() =>
                {
                    RunScCommand("stop", "GoodbyeDPI");
                    RunScCommand("delete", "GoodbyeDPI");
                    RunScCommand("stop", "WinDivert");
                    RunScCommand("delete", "WinDivert");
                    RunScCommand("stop", "WinDivert14");
                    RunScCommand("delete", "WinDivert14");
                });

                UpdateStatusIndicator(_grayBrush);
                StatusText.Text = "Hazır";
                StatusDescription.Text = "Servis kaldırıldı";

                AppendLog("✓ Servis kaldırma işlemi tamamlandı!");
                AppendLog("═══════════════════════════════════════════════");

                MessageBox.Show(
                    "GoodbyeDPI servisi başarıyla kaldırıldı!",
                    "Başarılı",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                AppendLog($"⚠ Hata: {ex.Message}");
            }

            ServiceRemoveBtn.IsEnabled = true;
        }

        private void RunScCommand(string action, string serviceName)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"{action} \"{serviceName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                process?.WaitForExit(5000);

                Dispatcher.Invoke(() =>
                {
                    AppendLog($"  sc {action} \"{serviceName}\" - OK");
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    AppendLog($"  sc {action} \"{serviceName}\" - {ex.Message}");
                });
            }
        }

        private void ClearLogBtn_Click(object sender, RoutedEventArgs e)
        {
            LogOutput.Text = "";
            AppendLog("Günlük temizlendi.");
        }

        private void StartGoodbyeDpi(string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(_exePath) ?? ""
                };

                _goodbyeDpiProcess = new Process { StartInfo = startInfo };
                _goodbyeDpiProcess.OutputDataReceived += Process_OutputDataReceived;
                _goodbyeDpiProcess.ErrorDataReceived += Process_ErrorDataReceived;
                _goodbyeDpiProcess.EnableRaisingEvents = true;
                _goodbyeDpiProcess.Exited += Process_Exited;

                _goodbyeDpiProcess.Start();
                _goodbyeDpiProcess.BeginOutputReadLine();
                _goodbyeDpiProcess.BeginErrorReadLine();

                _isRunning = true;

                Dispatcher.Invoke(() =>
                {
                    UpdateStatusIndicator(_greenBrush);
                    StatusText.Text = "Çalışıyor (Geçici)";
                    StatusDescription.Text = $"PID: {_goodbyeDpiProcess.Id} - Program kapatılınca durur";
                    AppendLog($"✓ GoodbyeDPI başlatıldı! (PID: {_goodbyeDpiProcess.Id})");
                    AppendLog($"  ⚠ Bu geçici mod - GUI kapatılınca kapanır");
                    AppendLog("");
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    AppendLog($"❌ Başlatma hatası: {ex.Message}");
                    UpdateStatusIndicator(_redBrush);
                    StatusText.Text = "Hata";
                    StatusDescription.Text = "Başlatılamadı";
                    TempStartButton.IsEnabled = true;
                    TempStopButton.IsEnabled = false;
                    TempModeComboBox.IsEnabled = true;
                });
            }
        }

        private void StopGoodbyeDpi()
        {
            if (_goodbyeDpiProcess != null && !_goodbyeDpiProcess.HasExited)
            {
                try
                {
                    AppendLog("");
                    AppendLog("■ GoodbyeDPI (geçici) durduruluyor...");
                    _goodbyeDpiProcess.Kill(true);
                    _goodbyeDpiProcess.WaitForExit(3000);
                    AppendLog("✓ Durduruldu.");
                }
                catch (Exception ex)
                {
                    AppendLog($"⚠ {ex.Message}");
                }
            }

            try
            {
                foreach (var process in Process.GetProcessesByName("goodbyedpi"))
                {
                    process.Kill();
                }
            }
            catch { }

            _goodbyeDpiProcess = null;
            _isRunning = false;

            Dispatcher.Invoke(() =>
            {
                UpdateStatusIndicator(_grayBrush);
                StatusText.Text = "Durduruldu";
                StatusDescription.Text = "GoodbyeDPI devre dışı";
                TempStartButton.IsEnabled = true;
                TempStopButton.IsEnabled = false;
                TempModeComboBox.IsEnabled = true;
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Dispatcher.Invoke(() => AppendLog(e.Data));
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Dispatcher.Invoke(() => AppendLog($"⚠ {e.Data}"));
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            _isRunning = false;
            Dispatcher.Invoke(() =>
            {
                int exitCode = _goodbyeDpiProcess?.ExitCode ?? -1;
                AppendLog($"─ İşlem sonlandı (Çıkış: {exitCode})");
                
                UpdateStatusIndicator(exitCode != 0 ? _redBrush : _grayBrush);
                StatusText.Text = "Durduruldu";
                StatusDescription.Text = exitCode != 0 ? $"Çıkış kodu: {exitCode}" : "GoodbyeDPI kapatıldı";

                TempStartButton.IsEnabled = true;
                TempStopButton.IsEnabled = false;
                TempModeComboBox.IsEnabled = true;
            });
        }

        private void UpdateStatusIndicator(SolidColorBrush color)
        {
            var statusGlow = FindName("StatusGlow") as System.Windows.Shapes.Ellipse;
            if (statusGlow != null)
            {
                statusGlow.Fill = color;
            }
        }

        private void AppendLog(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            LogOutput.Text += $"[{timestamp}] {message}\n";
            LogScrollViewer.ScrollToEnd();
        }
    }
}
