using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using UglyToad.PdfPig.DocumentLayoutAnalysis.Export;

namespace testApps.Libs
{
    public class OCRProcess
    {
        public static string GetOCRFromText(string filepath)
        {
            string ocrText = "";
            using (var ocrEngine = new TesseractEngine(@"./tessdata/ltsm", "tur", EngineMode.LstmOnly))
            {
                using (var img = Pix.LoadFromFile(filepath))
                {
                    var result = ocrEngine.Process(img);
                    ocrText += result.GetText();
                }
            }
            return ocrText;
        }


        public static void SearchablePdfCreateFromJpegs(string[] inputNames, string outputName, string dil)
        {
            using (IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(outputName, @"./tessdata/ltsm", false))
            {
                // PDF Title
                using (renderer.BeginDocument(outputName))
                {
                    using (TesseractEngine engine = new TesseractEngine(@"./tessdata/ltsm", dil, EngineMode.LstmOnly))
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
        }
        public static string GetOCRFromTextMulti(string[] filepath)
        {
            string ocrText = "";
            using (var ocrEngine = new TesseractEngine(@"./tessdata/ltsm", "tur", EngineMode.LstmOnly))
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
    }
}
