using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace testApps.Libs
{
    public class PopplerProcess
    {
        public static void ImagesFromPDF(string pdfPath)
        {
            // Çıktı klasörü
            string outputDirectory = Path.Combine(AppContext.BaseDirectory, "temp");
            string popplerPath = @"C:\Users\Gökçe\Desktop\poppler-24.08.0\Library\bin";  // Poppler binari dosyalarının tam yolu
            string fileName = Path.GetFileNameWithoutExtension(pdfPath);
            string outputPath = Path.Combine(outputDirectory, fileName);

            // Çıktı klasörü mevcut değilse oluştur
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Poppler'ın doğru yerde olduğundan emin olun
            string pdftoppmPath = Path.Combine(popplerPath, "pdftoppm.exe");
            if (!File.Exists(pdftoppmPath))
            {
                MessageBox.Show("Poppler bulunamadı. Lütfen Poppler kurulum yolunu kontrol edin.");
                return;
            }

            // Process ayarları
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pdftoppmPath,
                Arguments = $"-jpeg -r 300 \"{pdfPath}\" \"{outputPath}\"",  // 300 DPI'da JPEG formatına dönüştür
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show($"PDF başarıyla dönüştürüldü! Çıktılar: {outputDirectory}");
                    }
                    else
                    {
                        MessageBox.Show($"Hata oluştu:\n{error}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
