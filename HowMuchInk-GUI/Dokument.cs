using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PdfToImage;
using NCanalysis;
using HowMuchInk;
using HowMuchInk_GUI;

namespace NCanalysis
{
    class Dokument
    {
        #region README
        public static String name = "Dokument";
        public static String version = "1.0_15-05-31";
        public static String author = "Julius Herb - julius.herb@outlook.com";
        #endregion
        #region Parameter
        public String render_path = "C:\\Noscio\\HowMuchInk\\"; //Path, were the rendered images from the pdf files are saved
        #endregion
        #region Init
        public Dokument(String p, int[] _reichw)
        {
            reichw = _reichw;
            pfad = p;
            status = "ready";
        }
        #endregion
        #region Variablen
        public int[] reichw;
        public String pfad = "";
        public String[] pdfpfade = new String[1];
        public String status = "init";
        ulong Magenta = 0;
        ulong Cyan = 0;
        ulong Yellow = 0;
        ulong Black = 0;
        Bitmap bmp;
        Bitmap[] bmparray;
        #endregion
        #region intern
        void analyzeColor(int a, int b)
        {
            System.Drawing.Color pixelColor = bmp.GetPixel(a, b);
            float R = pixelColor.R;
            float G = pixelColor.G;
            float B = pixelColor.B;
            float R1 = R / 255;
            float G1 = G / 255;
            float B1 = B / 255;
            float K = 1 - max3(R1, G1, B1);
            float C = (1 - R1 - K) / (1 - K);
            float M = (1 - G1 - K) / (1 - K);
            float Y = (1 - B1 - K) / (1 - K);
            Magenta += (ulong)(M*1000000);
            Yellow += (ulong)(Y * 1000000);
            Cyan += (ulong)(C * 1000000);
            Black += (ulong)(K * 1000000);
        }
        float max3(float a1, float b1, float c1)
        {
            float l = Math.Max(a1, b1);
            float m = Math.Max(b1, c1);
            return Math.Max(l, m);
        }
        #endregion
        public void Analyse()
        {
            AnalyseState.setStatus("Analyse gestartet");
            try
            {
                if (pfad.EndsWith(".pdf"))
                {
                    AnalyseState.setStatus("PDF wird gerendert");
                    ConvertSingleImage(pfad);
                    ulong zähler = 0;
                    AnalyseState.setStatus("Analyse...");
                    foreach (String pf in pdfpfade)
                    {
                        bmp = new Bitmap(pf);
                        double fakt = bmp.Width*pdfpfade.Length / 100;
                        for (int a = 0; a < bmp.Width; a++)
                        {
                            zähler++;
                            AnalyseState.setCStatus((int)(zähler / fakt));
                            for (int b = 0; b < bmp.Height; b++)
                            {
                                analyzeColor(a, b);
                            }
                        }
                    }
                    AnalyseState.setCStatus(100);
                }
                else
                {
                    AnalyseState.setStatus("Bilder laden");
                    bmp = new Bitmap(pfad);
                    double fakt = bmp.Width * pdfpfade.Length / 100;
                    AnalyseState.setStatus("Analyse...");
                    for (int a = 0; a < bmp.Width; a++)
                    {
                        AnalyseState.setCStatus((int)(a/ fakt));
                        for (int b = 0; b < bmp.Height; b++)
                        {
                            analyzeColor(a, b);
                        }
                    }
                    AnalyseState.setCStatus(100);
                }
                AnalyseState.setCStatus(0);
                AnalyseState.setStatus("Ready");
            }
            catch (Exception ee)
            {
            }
        }
        public ulong[] getResult()
        {
            try
            {
                ulong faktor = (ulong)((bmp.Width * bmp.Height));
                return new ulong[] { Cyan * (ulong)reichw[4]*20 / faktor / (ulong)reichw[0], Magenta * (ulong)reichw[4]*20 / faktor / (ulong)reichw[1], Yellow * (ulong)reichw[4]*20 / faktor / (ulong)reichw[2], Black * (ulong)reichw[4]*20 / faktor / (ulong)reichw[3] };
            }
            catch(Exception except)
            {
                MessageBox.Show(except.Message, "Fehler");
                return new ulong[4];
            }
        }
        private void ConvertSingleImage(string filename)
        {
            bool Converted = false;
            //Setup the converter
            converter.OutputToMultipleFile = true;
            converter.FirstPageToConvert = -1;
            converter.LastPageToConvert = -1;
            converter.FitPage = true;
            converter.JPEGQuality = 10;
            converter.OutputFormat = "jpeg";
            System.IO.FileInfo input = new FileInfo(filename);
            string path = render_path;
            string path1 = path+""+input.Name;
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
                if (Directory.Exists(path1))
                {
                    DirectoryInfo di3 = new DirectoryInfo(path1);
                    di3.Delete(true);
                }
                DirectoryInfo di1 = Directory.CreateDirectory(path1);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            } 
            string output = string.Format("{0}{2}\\{2}{3}", render_path,input.Directory, input.Name, ".jpg");
            Converted = converter.Convert(input.FullName, output);
            var pdfstack = new DirectoryInfo(string.Format("{0}{2}\\", render_path, input.Directory, input.Name, ".jpg")).GetFiles();
            pdfpfade = new String[pdfstack.Length]; 
            for (int i = 0; i < pdfstack.Length; i++)
            {
                pdfpfade[i] = string.Format("{0}{2}\\", render_path, input.Directory, input.Name, ".jpg") + pdfstack[i].Name;
            }
            pfad = pdfpfade[0];
            Console.WriteLine("#ll" + string.Format("{0}\\{2}\\", render_path, input.Directory, input.Name, ".jpg"));
        }
        PDFConvert converter = new PDFConvert();
    }
}
