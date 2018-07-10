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
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace SzyfrPermutacyjny
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Key;
        public MainWindow()
        {
            InitializeComponent();
            for(int i = 0; i < 16; i++)
            {
                KeyLength.Items.Add(i+1);
            }
            KeyLength.SelectedIndex = 0;
        }
        public void GenerateKey(int n)
        {
            Random rand = new Random();
            List<int> lista = new List<int>();
            int i = 0;
            while (i<n)
            {
                int randnum = rand.Next(0, n);
                if(!lista.Contains(randnum))
                {
                    lista.Add(randnum);
                    i++;
                }
            }
            Key = string.Empty;
            foreach(var a in lista)
            {
                Key = Key + a + " ";
            }
            Klucz.Text = Key;
        }
        public string[] Encrypt(string tekst,int[] klucz)
        {
            string[] wynik = new string[tekst.Length];
            for(int i = 0; i < klucz.Length; i++)
            {
                wynik[i] = tekst[klucz[i]].ToString();
            }
            return wynik;
        }
        public string[] ReverseEncrypt(string tekst, int[] klucz)
        {
            string[] wynik = new string[tekst.Length];
            for (int i = 0; i < klucz.Length; i++)
            {
                wynik[klucz[i]] = tekst[i].ToString();
            }
            return wynik;
        }
        private void Szyfruj_Click(object sender, RoutedEventArgs e)
        {
            Time1.Clear();
            Stopwatch zegar = Stopwatch.StartNew();
            GenerujKlucz.IsEnabled = false ;
            KeyLength.IsEnabled = false;
            TekstZaszyfrowany.Clear();
            string wyjsciowy;
            string TekstjawnyLocal = TekstJawny.Text.Replace(System.Environment.NewLine, "");
            int[] t = StringtoArray.StoA(Klucz.Text, Convert.ToInt32(KeyLength.Text));
            int NumOfColumns = TekstjawnyLocal.Length / Convert.ToInt32(KeyLength.Text);
            string blockOfText;
            string restOfText = TekstjawnyLocal.ToString();
            for(int i = 0; i < NumOfColumns; i++)
            {
                blockOfText = restOfText.Substring(0, Convert.ToInt32(KeyLength.Text));
                restOfText = restOfText.Substring(Convert.ToInt32(KeyLength.Text));
                wyjsciowy = string.Concat(Encrypt(blockOfText, t));
                TekstZaszyfrowany.Text = TekstZaszyfrowany.Text + wyjsciowy;
                wyjsciowy = string.Empty;
            }
            int dl = Convert.ToInt32(KeyLength.Text) - restOfText.Length;
            for (int i = 0; i <= dl; i++)
            {
                restOfText += " ";
            }
            wyjsciowy = string.Concat(Encrypt(restOfText, t));
            TekstZaszyfrowany.Text = TekstZaszyfrowany.Text + wyjsciowy;
            Deszyfruj.IsEnabled = true;
            zegar.Stop();
            Time1.Text = zegar.ElapsedMilliseconds.ToString() + " ms";
        }

        private void Odszyfruj_click(object sender, RoutedEventArgs e)
        {
            Time2.Clear();
            Stopwatch zegar = Stopwatch.StartNew();
            Szyfruj.IsEnabled = true;
            Deszyfruj.IsEnabled = false;
            TekstOdszyfrowany.Clear();
            string wyjsciowy;
            string TekstZaszyfrowanyLocal = TekstZaszyfrowany.Text.Replace(System.Environment.NewLine, "");
            int[] t = StringtoArray.StoA(Klucz.Text, Convert.ToInt32(KeyLength.Text));
            int NumOfColumns = TekstZaszyfrowanyLocal.Length / Convert.ToInt32(KeyLength.Text);
            string blockOfText;
            string restOfText = TekstZaszyfrowanyLocal.ToString();
            for (int i = 0; i < NumOfColumns; i++)
            {
                blockOfText = restOfText.Substring(0, Convert.ToInt32(KeyLength.Text));
                restOfText = restOfText.Substring(Convert.ToInt32(KeyLength.Text));
                wyjsciowy = string.Concat(ReverseEncrypt(blockOfText, t));
                TekstOdszyfrowany.Text = TekstOdszyfrowany.Text + wyjsciowy;
                wyjsciowy = string.Empty;
            }
            TekstOdszyfrowany.Text = TekstOdszyfrowany.Text + restOfText;
            zegar.Stop();
            Time2.Text = zegar.ElapsedMilliseconds.ToString() + " ms";
        }

        private void Wyczysc_Click(object sender, RoutedEventArgs e)
        {
            KeyLength.IsEnabled = true;
            Szyfruj.IsEnabled = false;
            Deszyfruj.IsEnabled = false;
            TekstJawny.IsEnabled = true;
            GenerujKlucz.IsEnabled = true;
            TekstJawny.Clear();
            TekstZaszyfrowany.Clear();
            TekstOdszyfrowany.Clear();
            Key = string.Empty;
            Klucz.Clear();
            Time1.Clear();
            Time2.Clear();
        }

        private void GenerujKlucz_Click(object sender, RoutedEventArgs e)
        {
            GenerateKey(Convert.ToInt32(KeyLength.Text));
            Szyfruj.IsEnabled = true;
            TekstOdszyfrowany.IsReadOnly = true;
            TekstZaszyfrowany.IsReadOnly = true;
        }

        private void WczytajPlik_Click(object sender, RoutedEventArgs e)
        {
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik *.txt do zaszyfrowania";
            openFileDialog1.InitialDirectory = @"c:\";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == true)
            {
                file1.Content = openFileDialog1.FileName;
            }
            try
            { 
                using (StreamReader sr = new StreamReader(file1.Content.ToString(),enc))
                {
                    String line = sr.ReadToEnd();
                    TekstJawny.Clear();
                    TekstJawny.Text = line;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void ZapiszKlucz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter writetext = new StreamWriter("Klucz"))
                {
                    writetext.Write(Klucz.Text.ToString());
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void ZapiszZaszyfrowany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter writetext = new StreamWriter("TekstZaszyfrowany"))
                {
                    writetext.Write(TekstZaszyfrowany.Text.ToString());
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void WczytajKlucz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik z kluczem";
            openFileDialog1.InitialDirectory = @"c:\";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            string file = string.Empty;
            if (openFileDialog1.ShowDialog() == true)
            {
                file = openFileDialog1.FileName;
            }
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = sr.ReadToEnd();
                    int[] t = StringtoArray.StoA(line, StringtoArray.length(line));
                    foreach (var x in t)
                        Console.WriteLine(x);
                    if (StringtoArray.CheckKey(t))
                    {
                        Klucz.Clear();
                        Klucz.Text = line;
                        KeyLength.SelectedIndex = StringtoArray.length(Klucz.Text.ToString()) - 1;
                    }
                    else
                    {
                        MessageBox.Show("Podany błędny format klucza. Poprawnym kluczem jest permutacja liczb bez powtorzen oddzielona znakami spacji");
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void WczytajZaszyfrowany_Click(object sender, RoutedEventArgs e)
        {
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik zaszyfrowany";
            openFileDialog1.InitialDirectory = @"c:\";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            string file = string.Empty;
            if (openFileDialog1.ShowDialog() == true)
            {
                file = openFileDialog1.FileName;
            }
            try
            {
                using (StreamReader sr = new StreamReader(file,enc))
                {
                    String line = sr.ReadToEnd();
                    TekstJawny.Clear();
                    TekstZaszyfrowany.Clear();
                    TekstZaszyfrowany.Text = line;
                    Szyfruj.IsEnabled = false;
                    Deszyfruj.IsEnabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }
        private void Informations_Click(object sender, RoutedEventArgs e)
        {
            Info nowe = new Info();
            nowe.Show();
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help nowe = new Help();
            nowe.Show();
        }
        private void Oszyfrie_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
