using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace GeneratorStrumieniowy
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            UtworzKlucz12();
        }
        public void UtworzKlucz12()
        {
            bool isalltrue = true;
            StreamWriter sw = new StreamWriter("Klucz");
            LFSR rej1 = new LFSR(0x4221, LFSR1Polynomial.Text);
            LFSR rej2 = new LFSR(0x4CE1, LFSR2Polynomial.Text);
            int period = 0;
            int maxsize = 0;
            if (CheckClass.CheckIfInt(DlugoscKlucza.Text))
                maxsize = Convert.ToInt32(DlugoscKlucza.Text);
            else
            {
                MessageBox.Show("Zła wartość");
                isalltrue = false;
            }
            if (isalltrue == true)
            {
                while (period < maxsize)
                {
                    uint b1 = rej1.work();
                    uint b2 = rej2.work();
                    if (b2 == 1)
                    {
                        sw.Write(b1);
                        period++;
                    }
                }
                sw.Close();
                MessageBox.Show("Utworzono plik klucz");
                //utworz plik dla wartosci początkowych
                zapisz();
            }
        }
        public void zapisz()
        {
            StreamWriter sw = new StreamWriter("Wartosci poczatkowe dla klucza");
            sw.Write("LFSR1: Start_state = " + LFSR1StartState.Text + " Wielomian = " + LFSR1Polynomial.Text);
            sw.WriteLine();
            sw.Write("LFSR2: Start_state = " + LFSR2StartState.Text + " Wielomian = " + LFSR2Polynomial.Text);
            sw.Close();
        }


        //SZYFRATOR STRUMIENIOWY
        private string KluczString = "00011111";
        private string TekstZaszyfrowanyBIN = String.Empty;
        private bool IsKeyLoaded = false;
        private void ZaszyfrujButton_Click(object sender, RoutedEventArgs e)
        {
            Zaszyfruj();
        }
        private void Zaszyfruj()
        {
            if (CheckClass.CheckLength(TekstJawny.Text, KluczString))
            {
                if (IsKeyLoaded)
                {
                    TekstZaszyfrowany.Text = String.Empty;
                    TekstZaszyfrowanyBIN = String.Empty;
                    string BinJawny = StringToSomething.ToBinary(StringToSomething.StoB(TekstJawny.Text));
                    for (int i = 0; i < BinJawny.Length; i++)
                    {
                        TekstZaszyfrowanyBIN += StringToSomething.XOR(BinJawny[i], KluczString[i]);
                    }
                    TekstZaszyfrowany.Text = StringToSomething.BinaryToString(TekstZaszyfrowanyBIN);
                }
                else
                    MessageBox.Show("Nie wczytano klucza");
            }
            else
                MessageBox.Show("Klucz jest krotszy niz tekst");
        }

        private void WczytajKlucz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik z kluczem";
            //openFileDialog1.InitialDirectory = @"c:\";
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
                    KluczString = sr.ReadToEnd();
                    file1.Content = "Plik z kluczem wczytany";
                    IsKeyLoaded = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void OdszyfrujButton_Click(object sender, RoutedEventArgs e)
        {
            Odszyfruj();
        }

        private void Odszyfruj()
        {
            string tekst = String.Empty;
            TekstRozszyfrowany.Text = String.Empty;
            string BinZaszyfrowany = StringToSomething.ToBinary(StringToSomething.StoB(TekstZaszyfrowany.Text));
            for (int i = 0; i < TekstZaszyfrowanyBIN.Length; i++)
            {
                tekst += StringToSomething.XOR(TekstZaszyfrowanyBIN[i], KluczString[i]);
            }
            TekstRozszyfrowany.Text = StringToSomething.BinaryToString(tekst);
        }
        private void ZapiszPlik_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter("Tekst zaszyfrowany");
            sw.Write(TekstZaszyfrowanyBIN);
            sw.Close();
            MessageBox.Show("Plik zapisany");
        }

        private void WczytajPlik_Click(object sender, RoutedEventArgs e)
        {
            if (IsKeyLoaded)
            {
                Encoding enc = Encoding.GetEncoding("Windows-1250");
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Wybierz plik z tekstem";
                //openFileDialog1.InitialDirectory = @"c:\";
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
                        TekstZaszyfrowanyBIN = sr.ReadToEnd();
                        Odszyfruj();
                        MessageBox.Show("Plik z tekstem wczytany i roszyfrowany.");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("Error! The file could not be read.");
                }
            }
            else
                MessageBox.Show("Nie wczytano klucza");
        }

        private void UtworzKlucz_Click(object sender, RoutedEventArgs e)
        {
            UtworzKlucz12();
            string file = "klucz";
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    KluczString = sr.ReadToEnd();
                    file1.Content = "Plik z kluczem wczytany";
                    IsKeyLoaded = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }


        //TESTY FIPS
        string test;
        bool isfileloaded = false;
        int magic1 = 0;
        int L0_1, L0_2, L0_3, L0_4, L0_5, L0_6,
            L1_1, L1_2, L1_3, L1_4, L1_5, L1_6;
        private void LoadFileTest_Click(object sender, RoutedEventArgs e)
        {
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik z kluczem";
            //openFileDialog1.InitialDirectory = @"c:\";
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
                using (StreamReader sr = new StreamReader(file, enc))
                {
                    test = sr.ReadToEnd();
                    isfileloaded = true;
                    MessageBox.Show("Wczytano plik poprawnie");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }
        private Boolean MonoBitTest(string tekst, int size = 20000)
        {
            int n = 0;
            for(int i = 0; i < size; i++)
            {
                if (tekst[i] == '1')
                    n++;
            }
            magic1 = n;
            if (n >= 925 && n <= 10275)
            {
                return true;
            }
		    else
            {
                return false;
            }
        }
        private Boolean RunsTest(string tekst2, int size = 20000)
        {
            string tekst = tekst2;
            int magiczny_licznik_jedynek = 0;
            int magiczny_licznik_zer = 0;
            int j_magicalCounter1 = 0, j_magicalCounter2 = 0, j_magicalCounter3 = 0,
                j_magicalCounter4 = 0, j_magicalCounter5 = 0, j_magicalCounter6 = 0;
            int z_magicalCounter1 = 0, z_magicalCounter2 = 0, z_magicalCounter3 = 0,
                z_magicalCounter4 = 0, z_magicalCounter5 = 0, z_magicalCounter6 = 0;
            for (int i = 0; i < size; i++)
            {

                if (Equals(tekst[i], '1') == true)
                {
                    if (magiczny_licznik_zer == 1) j_magicalCounter1++;
                    else if (magiczny_licznik_zer == 2) j_magicalCounter2++;
                    else if (magiczny_licznik_zer == 3) j_magicalCounter3++;
                    else if (magiczny_licznik_zer == 4) j_magicalCounter4++;
                    else if (magiczny_licznik_zer == 5) j_magicalCounter5++;
                    else if (magiczny_licznik_zer >= 6) j_magicalCounter6++;
                    magiczny_licznik_zer = 0;
                    magiczny_licznik_jedynek++;
                }

                if (Equals(tekst[i], '0') == true)
                {
                    if (magiczny_licznik_jedynek == 1) z_magicalCounter1++;
                    else if (magiczny_licznik_jedynek == 2) z_magicalCounter2++;
                    else if (magiczny_licznik_jedynek == 3) z_magicalCounter3++;
                    else if (magiczny_licznik_jedynek == 4) z_magicalCounter4++;
                    else if (magiczny_licznik_jedynek == 5) z_magicalCounter5++;
                    else if (magiczny_licznik_jedynek >= 6) z_magicalCounter6++;
                    magiczny_licznik_jedynek = 0;
                    magiczny_licznik_zer++;
                }
            }
            L0_1 = z_magicalCounter1; L0_2 = z_magicalCounter2; L0_3 = z_magicalCounter3; L0_4 = z_magicalCounter4; L0_5 = z_magicalCounter5; L0_6 = z_magicalCounter6;
            L1_1 = j_magicalCounter1; L1_2 = j_magicalCounter2; L1_3 = j_magicalCounter3; L1_4 = j_magicalCounter4; L1_5 = j_magicalCounter5; L1_6 = j_magicalCounter6;
            /*Great Magic here*/
            if (((j_magicalCounter1 >= 2315 && j_magicalCounter6 <= 2685) && (j_magicalCounter2 >= 1114 && j_magicalCounter2 <= 1386)
                && (j_magicalCounter3 >= 527 && j_magicalCounter3 <= 723) && (j_magicalCounter4 >= 240 && j_magicalCounter4 <= 384)
                && (j_magicalCounter5 >= 103 && j_magicalCounter5 <= 209) && (j_magicalCounter6 >= 103 && j_magicalCounter6 <= 209) &&
                ((z_magicalCounter1 >= 2315 && z_magicalCounter1 <= 2685) && (z_magicalCounter2 >= 1114 && z_magicalCounter2 <= 1386)
                && (z_magicalCounter3 >= 527 && z_magicalCounter3 <= 723) && (z_magicalCounter4 >= 240 && z_magicalCounter4 <= 384)
                && (z_magicalCounter5 >= 103 && z_magicalCounter5 <= 209) && (z_magicalCounter6 >= 103 && z_magicalCounter6 <= 209))) == true) return true;

            else return false;
        }
        int najdluzszy_ciag_zer = 0, najdluzszy_ciag_jedynek = 0;
        private Boolean LongRunTest(string tekst, int size = 20000)
        {
            int magiczny_licznik_dlugosci_zer = 0;
            int magiczny_licznik_dlugosci_jedynek = 0;
            for (int i = 0; i < tekst.Length; i++)
            {

                if (Equals(tekst[i], '1') == true)
                {
                    if (magiczny_licznik_dlugosci_zer > najdluzszy_ciag_zer) najdluzszy_ciag_zer = magiczny_licznik_dlugosci_zer;
                    magiczny_licznik_dlugosci_zer = 0;
                    magiczny_licznik_dlugosci_jedynek++;
                }
                if (magiczny_licznik_dlugosci_jedynek >= 26) return false;

                if (Equals(tekst[i], '0') == true)
                {
                    if (magiczny_licznik_dlugosci_jedynek > najdluzszy_ciag_jedynek) najdluzszy_ciag_jedynek = magiczny_licznik_dlugosci_jedynek;
                    magiczny_licznik_dlugosci_jedynek = 0;
                    magiczny_licznik_dlugosci_zer++;
                }
                if (magiczny_licznik_dlugosci_zer >= 26) return false;
            }
            return true;
        }
        double poker;
        private Boolean PokerTest(string tekst, int size = 20000)
        {
            String pomoc;
            int[] wystapienia = new int[16];
            double p;

            for (int i = 0; i < size; i+=4)
            {
                int j = i;
                int pomoc2 = 0;

                pomoc = Convert.ToString(tekst[j]) + Convert.ToString(tekst[j++]) +
                        Convert.ToString(tekst[j++]) + Convert.ToString(tekst[j++]);
                pomoc2 = Convert.ToInt32(pomoc, 2);
                wystapienia[pomoc2]++;
            }

            int suma = 0;
            for (int i = 0; i < 16; i++)
            {
                suma += wystapienia[i] * wystapienia[i];
            }

            p = (16.0 / 5000.0) * suma /1000;
            poker = p;
            if (p < 46.17 && p > 2.16)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            najdluzszy_ciag_zer = 0;
            najdluzszy_ciag_jedynek = 0;
            if (isfileloaded == true)
            {
                MessageBox.Show("Test pojedynczego bitu: >>" + MonoBitTest(test) + "<< Z liczbą: " + magic1 +"\n" +
                    "Test serii: >>" + RunsTest(test) + "<< Z wynikami: " + "\n" +
                    "0: " + L0_1 + " " + L0_2 + " " + L0_3 + " " + L0_4 + " " + L0_5 + " " + L0_6 + " " + "\n" +
                    "1: " + L1_1 + " " + L1_2 + " " + L1_3 + " " + L1_4 + " " + L1_5 + " " + L1_6 + " " + "\n" +
                    "Test dlugiej serii: >>" + LongRunTest(test) + "<< Z wynikami: \n" +
                    "Najdluzsza seria zer: " + najdluzszy_ciag_zer + "\n" +
                    "Najdluzsza seria jedynek: " + najdluzszy_ciag_jedynek + "\n" +
                    "Test pokerowy: >>" + PokerTest(test) + "<< Z p=" + poker
                    );

            }
            else
                MessageBox.Show("Nie wczytano pliku.");
        }
    }
}
