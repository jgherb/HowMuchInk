using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using NCanalysis;
using HowMuchInk;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Globalization;

namespace HowMuchInk_GUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            items = new ObservableCollection<FileAnalyse>();
            ResultView.ItemsSource = items;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            dt.Tick += dt_Tick;
            dt.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dt.Start();
        }
        int[] reichw;
        private void dt_Tick(object sender, EventArgs e)
        {
            String s = DateTime.Now.ToShortTimeString()+" - "+AnalyseState.getStatus();
            StatusBox.Content = s;
            if (s.Contains("PDF"))
            {
                ProgressB.IsIndeterminate = true;
            }
            else
            {
                ProgressB.IsIndeterminate = false;
            }
            ProgressB.Value = AnalyseState.getCStatus();
        }
        DispatcherTimer dt = new DispatcherTimer();
        ObservableCollection<FileAnalyse> items;
        BackgroundWorker worker = new BackgroundWorker();
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dokument doc1 = new Dokument(AnalyseState.Pfad, reichw);
            doc1.Analyse();
            AnalyseState.setResult(doc1.getResult());
        }
        String getResult(float f)
        {
            f = f / 1000;
            decimal dd = (decimal)f;
            dd = Math.Round(dd, 4, System.MidpointRounding.AwayFromZero);
            int i = (int)(dd / 10);
            int b = (int)(dd - i*10);
            String s = i.ToString()+","+b+"%";
            return s;
        }
        String getResultX(float f)
        {
            //f = f / 1000;
            decimal dd = (decimal)f;
            dd = Math.Round(dd, 4, System.MidpointRounding.AwayFromZero);
            int i = (int)(dd / 10); 
            int b = (int)(dd - i * 10);
            String s = i.ToString() + "," + b + "%";
            return s;
        }
        void Ausgabe()
        {
            items.Add(new FileAnalyse() { File = AnalyseState.Pfad, Kopien = reichw[4].ToString(), Cyan = getResult(AnalyseState.Cyan) + " /" + reichw[0], Magenta = getResult(AnalyseState.Magenta) + " /" + reichw[1], Yellow = getResult(AnalyseState.Yellow) + " /" + reichw[2], Black = getResult(AnalyseState.Black) + " /" + reichw[3] });
            ResultView.ItemsSource = items;
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CyanBox.Content = AnalyseState.Cyan+"ppm"+ " /" + reichw[0];
            MagentaBox.Content = AnalyseState.Magenta + "ppm" + " /" + reichw[1];
            YellowBox.Content = AnalyseState.Yellow + "ppm" + " /" + reichw[2];
            BlackBox.Content = AnalyseState.Black + "ppm" + " /" + reichw[3];
            Ausgabe();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                reichw = new int[] { Int32.Parse(CBox.Text), Int32.Parse(MBox.Text), Int32.Parse(YBox.Text), Int32.Parse(KBox.Text), Int32.Parse(CopyBox.Text) };
            }
            catch
            {
                MessageBox.Show("Fehlerhafte Eingaben bei der Druckerreichweite oder den Anzahl der Kopien", "Eingabefehler");
                return;
            }
            try {
                AnalyseState.Pfad = PathInput.Text;
            }
            catch
            {
                MessageBox.Show("Fehlerhafte Eingaben bei dem Dateipfad", "Eingabefehler");
                return;
            }
            worker.RunWorkerAsync();
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Readable files|*.pdf;*.BMP;*.JPG;*.GIF|PDF documents (.pdf)|*.pdf|Pictures|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                PathInput.Text = filename;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            Credits cs = new Credits();
            cs.Show();
        }
        ulong getValueX(String s)
        {
            return (ulong)(float.Parse(s.Substring(0, s.IndexOf("%")).Replace(',', '.'), CultureInfo.InvariantCulture) * 10);
        }
        private void SummeButton_Click(object sender, RoutedEventArgs e)
        {
            ulong Mag = 0;
            ulong Cya = 0;
            ulong Yel = 0;
            ulong Bla = 0;
            int zähl = 0;
            foreach (FileAnalyse hea in items)
            {
                MessageBox.Show(hea.Magenta.Substring(0, hea.Magenta.IndexOf("%")).Replace(',','.'));
                Mag += getValueX(hea.Magenta);
                Cya += getValueX(hea.Cyan);
                Yel += getValueX(hea.Yellow);
                Bla += getValueX(hea.Black);
            }
            MagentaBox.Content = getResultX(Mag);
            CyanBox.Content = getResultX(Cya);
            YellowBox.Content = getResultX(Yel);
            BlackBox.Content = getResultX(Bla);
        }
        private void FormatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Options.PaperSize papersize;
            switch (FormatCombo.SelectedIndex)
            {
                case 0:
                    papersize = Options.PaperSize.dinA4;
                    break;
                case 1:
                    papersize = Options.PaperSize.dinA5;
                    break;
                case 2:
                    papersize = Options.PaperSize.cm10xcm15;
                    break;
                default:
                    return;
            }
            String[] strsize = Options.getDimensions(papersize);
            try
            {
                String[] strsize2 = new String[2] { FormatBox1.Text, FormatBox2.Text };
                if (papersize==Options.getSize(strsize2)) {
                    return;
                }
                FormatBox1.Text = strsize[0];
                FormatBox2.Text = strsize[1];
            }
            catch
            {

            }
        }
        private void FormatBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String[] strsize = new String[2] { FormatBox1.Text, FormatBox2.Text };
                Options.PaperSize papersize = Options.getSize(strsize);
                switch (papersize)
                {
                    case Options.PaperSize.dinA4:
                        FormatCombo.SelectedIndex = 0;
                        break;
                    case Options.PaperSize.dinA5:
                        FormatCombo.SelectedIndex = 1;
                        break;
                    case Options.PaperSize.cm10xcm15:
                        FormatCombo.SelectedIndex = 2;
                        break;
                    default:
                        FormatCombo.SelectedIndex = 3;
                        break;
                }
            }
            catch
            {

            }
        }
        private void FormatBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String[] strsize = new String[2] { FormatBox1.Text, FormatBox2.Text };
                Options.PaperSize papersize = Options.getSize(strsize);
                switch (papersize)
                {
                    case Options.PaperSize.dinA4:
                        FormatCombo.SelectedIndex = 0;
                        break;
                    case Options.PaperSize.dinA5:
                        FormatCombo.SelectedIndex = 1;
                        break;
                    case Options.PaperSize.cm10xcm15:
                        FormatCombo.SelectedIndex = 2;
                        break;
                    default:
                        FormatCombo.SelectedIndex = 3;
                        break;
                }
            }
            catch
            {

            }
        }
    }
    public class FileAnalyse
    {
        public String File { get; set; }

        public String Kopien { get; set; }

        public String Cyan { get; set; }

        public String Magenta { get; set; }

        public String Yellow { get; set; }

        public String Black { get; set; }
    }
}
