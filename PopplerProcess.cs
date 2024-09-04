using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testApps.Libs
{
    public class PopplerProcess
    {
        public static void ImagesFromPDF(string pdfPath)
        {
            string ocrText = "";
            string outputDirectory = AppContext.BaseDirectory + "\\temp";
            string popplerPath = @"C:\Users\dijitalarsiv\Desktop\poppler\poppler-24.07.0\Library\bin";  // Poppler binari dosyalarının yolu
            string fileName= Path.GetFileNameWithoutExtension(pdfPath);
            string outputPath = Path.Combine(outputDirectory, fileName);

            // pdftoppm komutunu çağır
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(popplerPath, "pdftoppm.exe"),
                Arguments = $"-jpeg -r 300 \"{pdfPath}\" \"{outputPath}\"",  // 300 DPI'da JPEG formatına dönüştür
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                
            }
        }
    }
}
