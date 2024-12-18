using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using testApps.Libs;
using UglyToad.PdfPig.DocumentLayoutAnalysis.Export;
using static System.Resources.ResXFileRef;

namespace SearchablePDF
{
    public partial class Form1 : Form
    {

        string docDir = ".";
        public Form1()
        {
            InitializeComponent();
        }
        private List<string> selectedFilePaths = new List<string>(); // Seçilen dosya yollarını saklar

        public void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (selectedFilePaths == null || !selectedFilePaths.Any())
            {
                MessageBox.Show("Lütfen önce dosya veya klasör seçin.");
                return;
            }

            foreach (var filePath in selectedFilePaths)
            {
                if (!File.Exists(filePath))
                {
                    rtbx.AppendText($"Dosya bulunamadı: {filePath}\n");
                    continue;
                }

                string extension = Path.GetExtension(filePath).ToLower();
                string outputFilePath;

                // Eğer zaten .pdf değilse uzantıyı değiştir
                if (extension != ".pdf")
                {
                    outputFilePath = Path.ChangeExtension(filePath, ".pdf");
                }
                else
                {
                    outputFilePath = filePath; // Zaten PDF ise, olduğu gibi kullan
                }

                try
                {
                    switch (extension)
                    {
                        case ".png":
                            PdfFromPng(filePath, outputFilePath, "tur", rtbx);
                            break;
                        case ".jpeg":
                            PdfFromJpeg(new string[] { filePath }, outputFilePath, "tur", rtbx);
                            break;
                        case ".jpg":
                            PdfFromJpg(filePath, outputFilePath, "tur", rtbx);
                            break;

                        case ".tiff":
                        case ".tif":
                            PdfFromTiff(filePath, outputFilePath, "tur", rtbx);
                            break;

                        case ".pdf":
                            PdfDFromPdf(filePath, outputFilePath, "tur", rtbx);
                            break;

                        default:
                            rtbx.AppendText($"Desteklenmeyen dosya türü: {extension}\n");
                            break;
                    }

                    rtbx.AppendText($"{Path.GetFileName(filePath)} başarıyla {outputFilePath} olarak kaydedildi.\n");
                }
                catch (Exception ex)
                {
                    rtbx.AppendText($"Hata: {filePath} işlenirken bir hata oluştu: {ex.Message}\n");
                }
            }

            MessageBox.Show("Tüm dosyalar başarıyla dönüştürüldü!");
        }
        public static void PdfFromPng(string inputName, string outputName, string dil, RichTextBox rtbx)
        {
            try
            {
                // Tesseract PDF Renderer oluştur
                using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata", false))

                using (renderer.BeginDocument(outputName)) // PDF dokümanına başla

                using (TesseractEngine engine = new TesseractEngine(@"./tessdata/", dil, EngineMode.LstmOnly))
                {
                    // PNG dosyasını yükle
                    using (var img = Pix.LoadFromFile(inputName))
                    {
                        // OCR işlemi gerçekleştir
                        using (var page = engine.Process(img))
                        {
                            // PDF'ye sayfa olarak ekle
                            renderer.AddPage(page);
                        }
                    }

                    // Başarı durumu çıktısı
                    rtbx.AppendText($"{inputName} başarıyla {outputName} olarak dönüştürüldü.\n");
                }
            }
            catch (Exception ex)
            {
                // Hata durumu çıktısı
                rtbx.AppendText($"Hata: {inputName} işlenirken bir hata oluştu: {ex.Message}\n");
            }
        }


        public static void PdfFromJpg(string inputNames, string outputName, string dil, RichTextBox rtbx)
        {

            using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata", false))

            using (renderer.BeginDocument(outputName))

            using (TesseractEngine engine = new TesseractEngine(@"./tessdata/", dil, EngineMode.LstmOnly))
            {
                using (var img = Pix.LoadFromFile(inputNames))
                {
                    using (var page = engine.Process(img))
                    {
                        renderer.AddPage(page);
                    }
                }
                rtbx.AppendText(inputNames + " PDF oluşturuldu");
            }
        }

        public static void PdfDFromPdf(string inputName, string outputName, string dil, RichTextBox rtbx)
        {

            string extension = "";

            PopplerProcess.ImagesFromPDF(inputName);

            if (!Directory.Exists(AppContext.BaseDirectory + "\\temp"))
            {
                Directory.CreateDirectory(AppContext.BaseDirectory + "\\temp");
            }

            var jpegListe = Directory.GetFiles(AppContext.BaseDirectory + "\\temp", "*.jpg");

            using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata", false))
            {
                using (renderer.BeginDocument(outputName))
                {
                    using (TesseractEngine engine = new TesseractEngine(@"./tessdata", dil, EngineMode.LstmOnly))
                    {
                        foreach (var jpeg in jpegListe)
                        {
                            using (Pix img = Pix.LoadFromFile(jpeg))
                            {
                                using (var page = engine.Process(img, jpeg))
                                {
                                    renderer.AddPage(page);
                                }
                            }
                        }

                    }

                }
                rtbx.AppendText(inputName + " PDF oluşturuldu");
            }
        }

        private void ConvertIt(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Geçersiz bir dosya yolu girildi.");
                return;
            }

            string extension = Path.GetExtension(filePath).ToLower(); // Dosyanın uzantısı
            string outputFilePath = Path.ChangeExtension(filePath, ".pdf"); // Aynı klasöre PDF olarak kaydedilecek

            try
            {
                switch (extension)
                {
                    case ".png":
                    case ".jpeg":
                    case ".jpg":
                        PdfFromJpeg(new string[] { filePath }, outputFilePath, "tur", rtbx);
                        break;

                    case ".tiff":
                    case ".tif":
                        PdfFromTiff(filePath, outputFilePath, "tur", rtbx);
                        break;

                    case ".pdf":
                        PdfDFromPdf(filePath, outputFilePath, "tur", rtbx);
                        break;

                    default:
                        MessageBox.Show($"Desteklenmeyen dosya türü: {extension}");
                        return;
                }

                rtbx.AppendText($"{Path.GetFileName(filePath)} başarıyla {outputFilePath} olarak dönüştürüldü.\n");
            }
            catch (Exception ex)
            {
                rtbx.AppendText($"Hata: {filePath} işlenirken bir hata oluştu: {ex.Message}\n");
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Dosya seçmek istiyorsanız evete klasör seçmek istiyorsanız hayıra basınız.",
                "Seçim Yapın",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Dosya seçimi
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Supported Files|*.jpg;*.jpeg;*.png;*.tiff;*.tif;*.pdf|All Files|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedFilePaths.Clear();
                        txtbx.Clear();

                        foreach (var file in openFileDialog.FileNames)
                        {
                            selectedFilePaths.Add(file);
                            txtbx.AppendText(file + Environment.NewLine); // Dosya yollarını txtbx'e yaz
                        }

                        rtbx.AppendText($"{selectedFilePaths.Count} dosya seçildi.\n");
                    }
                    else
                    {
                        MessageBox.Show("Hiçbir dosya seçilmedi.");
                    }
                }
            }
            else if (result == DialogResult.No)
            {
                // Klasör seçimi
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        selectedFilePaths.Clear();
                        txtbx.Clear();

                        string[] filesInFolder = Directory.GetFiles(fbd.SelectedPath);
                        foreach (var file in filesInFolder)
                        {
                            selectedFilePaths.Add(file);
                            txtbx.AppendText(file + Environment.NewLine); // Klasördeki dosyaları txtbx'e yaz
                        }

                        rtbx.AppendText($"{filesInFolder.Length} dosya bulundu.\n");
                    }
                    else
                    {
                        MessageBox.Show("Hiçbir klasör seçilmedi.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seçim yapılmadı.");
            }
        }
        public void choosedocDir()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                docDir = fbd.SelectedPath;

                string[] Files = Directory.GetFiles(docDir, ".");


                string[] allFiles = Files.ToArray();

                if (allFiles.Length == 0)
                {
                    rtbx.AppendText("\nNo Files in " + docDir);
                    DialogResult dr = MessageBox.Show("Would you like to select another directory?", "No Files Found!", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            choosedocDir();
                            break;
                        case DialogResult.No:
                            return;
                    }
                }
                else
                {
                    // Update 
                    rtbx.AppendText("\n" + allFiles.Length + " File(s) Found");
                    txtbx.Text = fbd.SelectedPath;
                    for (int i = 0; i < allFiles.Length; i++)
                    {
                        lstbx.Items.Add(allFiles[i].Substring(txtbx.Text.Length + 1));
                        rtbx.AppendText("\n" + allFiles[i].Substring(txtbx.Text.Length + 1) + " found!");
                    }

                }
            }

        }
        public void SelectFilesOrDirectory(bool selectFolder)
        {
            if (selectFolder)
            {
                // Klasör seçimi
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = fbd.SelectedPath;
                        txtbx.Text = selectedPath;

                        string[] filesInFolder = Directory.GetFiles(selectedPath);
                        rtbx.AppendText($"\n{filesInFolder.Length} dosya bulundu.\n");

                        foreach (var file in filesInFolder)
                        {
                            rtbx.AppendText($"- {Path.GetFileName(file)}\n");
                            txtbx.AppendText(file + Environment.NewLine);
                        }
                    }
                }
            }
            else
            {
                // Dosya seçimi
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Supported Files|*.jpg;*.jpeg;*.png;*.tiff;*.tif;*.pdf|All Files|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var selectedFiles = openFileDialog.FileNames;
                        txtbx.Clear();
                        rtbx.AppendText($"{selectedFiles.Length} dosya seçildi:\n");

                        foreach (var file in selectedFiles)
                        {
                            txtbx.AppendText(file + Environment.NewLine);
                            rtbx.AppendText($"- {Path.GetFileName(file)}\n");
                        }
                    }
                }
            }
        }



        public static void PdfFromTiff(string inputName, string outputName, string dil, RichTextBox rtbx)
        {
            int sayfaSayisi = 0;
            int toplamSayfa = 0;
            string extension = "";




            using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata", false))
            {

                // PDF Title
                using (renderer.BeginDocument(outputName))
                {
                    using (TesseractEngine engine = new TesseractEngine(@"./tessdata", dil, EngineMode.LstmOnly))
                    {
                        using (PixArray pages = PixArray.LoadMultiPageTiffFromFile(inputName))
                        {
                            foreach (Pix p in pages)
                            {
                                Application.DoEvents();
                                using (var page = engine.Process(p, outputName))
                                {
                                    renderer.AddPage(page);
                                    sayfaSayisi++;

                                }
                            }
                        }


                    }

                }
                rtbx.AppendText(inputName + " PDF oluşturuldu");
            }

        }
        public static string GetOCRFromTextMulti(string[] filepath)
        {
            string ocrText = "";
            using (var ocrEngine = new TesseractEngine(@"./tessdata/Lstm ", "tur", EngineMode.LstmOnly))
            {
                foreach (var file in filepath)
                {
                    using (var img = Pix.LoadFromFile(file))
                    {
                        using (var result = ocrEngine.Process(img))
                        {
                            ocrText += result.GetText();
                        }
                    }
                }

            }
            return ocrText;
        }


        public static void PdfFromJpeg(string[] inputNames, string outputName, string dil, RichTextBox rtbx)
        {
            string extension = "";

            for (int i = 0; i < inputNames.Length; i++)
            {
                extension = Path.GetExtension(inputNames[i]);
            }
            using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata", false))
            {
                // PDF Title
                using (renderer.BeginDocument(outputName))
                {
                    using (TesseractEngine engine = new TesseractEngine(@"./tessdata", dil, EngineMode.LstmOnly))
                    {
                        foreach (string inputName in inputNames)
                        {
                            using (Pix img = Pix.LoadFromFile(inputName))
                            {
                                using (var page = engine.Process(img, inputName))
                                {
                                    renderer.AddPage(page);
                                }
                            }
                        }
                    }
                }
            }
            rtbx.AppendText(inputNames + " PDF oluşturuldu");
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            rtbx.AppendText("Launch: Successful ");
        }

        private void txtbx_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kullanıcı bir öğe seçtiğinde işlem tamamlandı mesajı göster
            if (lstbx.SelectedItem != null)
            {
                MessageBox.Show("İşlem Tamamlandı!");
            }
        }



        private void rtbx_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
