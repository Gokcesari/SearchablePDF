using System;
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
        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Click(object sender, EventArgs e)
        {
            choosedocDir();
            PopplerProcess.ImagesFromPDF(docDir); ;

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

            if(!Directory.Exists(AppContext.BaseDirectory + "\\temp"))
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

        public void ConvertIt()
        {
            string folderPath = txtbx.Text;


            var files = Directory.GetFiles(folderPath);

            if (files.Length == 0)
            {
                MessageBox.Show("Klasörde dosya bulunamadı.");
                return;
            }

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();  // Get the file extension in lowercase

                switch (extension)
                {
                    case ".png":
                    case ".jpeg":
                    case ".jpg":
                        PdfFromJpeg(new string[] { file }, file.Replace(Path.GetExtension(file),""), "tur", rtbx);
                        break;

                    case ".tiff":
                    case ".tif":
                        PdfFromTiff(file, file.Replace(Path.GetExtension(file), ""), "tur", rtbx);
                        break;

                    case ".pdf":
                        PdfDFromPdf(file, file.Replace(Path.GetExtension(file), ""), "tur", rtbx);
                        break;

                    default:
                        MessageBox.Show("Desteklenmeyen dosya türü: " + extension);
                        break;
                }

            }
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            string folderPath = txtbx.Text;
            var files = Directory.GetFiles(folderPath);

            if (files == null)
            {
                MessageBox.Show("Dosya bulunamadı");

            }

            else
            {

                foreach (var file in files)
                {
                    ConvertIt();
                    rtbx.AppendText(file + " PDF oluşturuldu");
                }
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
    }
}

