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
using System.Numerics;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace RSAPodpisCyfrowy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BigInteger Pa, Pb;
        private BigInteger Qa, Qb;
        private BigInteger PHIa, PHIb;
        private BigInteger Na, Nb;
        private BigInteger Ea, Eb;
        private BigInteger Da, Db;
        private LiczbyPierwsze primes;

        Random rand = new Random();

        RSA_key public_keyA;
        RSA_key private_keyA;
        RSA_key public_key_wczytanyA;
        RSA_key private_key_wczytanyA;

        RSA_key public_keyB;
        RSA_key private_keyB;
        RSA_key public_key_wczytanyB;
        RSA_key private_key_wczytanyB;


        private void PodglądZmiennych_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Para kluczy A: \n" +
                "Wartość P = " + Pa + "\n" +
                "Wartość Q = " + Qa + "\n" +
                "Wartość N = " + Na + "\n" +
                "Wartość PHI = " + PHIa + "\n" +
                "Wartość E = " + Ea + "\n" +
                "Wartość D = " + Da + "\n" +
                "Para kluczy B: \n" +
                "Wartość P = " + Pb + "\n" +
                "Wartość Q = " + Qb + "\n" +
                "Wartość N = " + Nb + "\n" +
                "Wartość PHI = " + PHIb + "\n" +
                "Wartość E = " + Eb + "\n" +
                "Wartość D = " + Db

                );
        }

        private void ZapiszKluczPublicznyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz klucz publiczny",
                    Filter = "Public Key (*.publickey)|*.publickey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.WriteLine(Na);
                        writetext.WriteLine(Ea);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać klucza publicznego");
            }
        }

        private void ZapiszKluczPrywatnyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz klucz prywatny",
                    Filter = "Private Key (*.privatekey)|*.privatekey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.WriteLine(Na);
                        writetext.WriteLine(Da);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać klucza prywatnego");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            primes = new LiczbyPierwsze(2, 10000);
        }

        private void ZapiszKluczPublicznyButtonB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz klucz publiczny",
                    Filter = "Public Key (*.publickey)|*.publickey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.WriteLine(Nb);
                        writetext.WriteLine(Eb);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać klucza publicznego");
            }
        }

        private void ZapiszKluczPrywatnyButtonB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz klucz prywatny",
                    Filter = "Private Key (*.privatekey)|*.privatekey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.WriteLine(Nb);
                        writetext.WriteLine(Db);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać klucza prywatnego");
            }
        }

        private void WczytajKluczPubliczny_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "Wybierz plik z kluczem prywatnym",
                    Filter = "Private Key (*.privatekey)|*.privatekey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                if (openFileDialog1.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        BigInteger n1 = BigInteger.Parse(sr.ReadLine());
                        BigInteger d1 = BigInteger.Parse(sr.ReadLine());
                        private_key_wczytanyA = new RSA_key(n1, d1);
                    }
                }
                LabelKluczPubliczny.Foreground = Brushes.Green;
                LabelKluczPubliczny.Content = "Wczytany";
                SzyfrujTekstButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się wczytać klucza");
            }
        }

        private void WczytajTekstJawny_Click(object sender, RoutedEventArgs e)
        {
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "Wybierz plik",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                if (openFileDialog1.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName, enc))
                    {
                        String line = sr.ReadToEnd();
                        TekstJawnyTB.Clear();
                        TekstJawnyTB.Text = line;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się wczytać tekstu");
            }
        }

        private void WczytajTekstZaszyfrowany_Click(object sender, RoutedEventArgs e)
        {
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "Wybierz plik",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                if (openFileDialog1.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName, enc))
                    {
                        String line = sr.ReadToEnd();
                        TekstZaszyfrowanyTB.Clear();
                        TekstZaszyfrowanyTB.Text = line;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się wczytać tekstu");
            }
        }

        private void ZapiszTekstZaszyfrowany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz plik",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.Write(TekstZaszyfrowanyTB.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać tekstu zaszyfrowanego");
            }
        }

        private void WczytajKluczPrywatny_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "Wybierz plik z kluczem publicznym",
                    Filter = "Public Key (*.publickey)|*.publickey",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                if (openFileDialog1.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        BigInteger n1 = BigInteger.Parse(sr.ReadLine());
                        BigInteger e1 = BigInteger.Parse(sr.ReadLine());
                        public_key_wczytanyA = new RSA_key(n1, e1);
                    }
                }
                LabelKluczPrywatny.Foreground = Brushes.Green;
                LabelKluczPrywatny.Content = "Wczytany";
                DeszyfrujTekstButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się wczytać klucza");
            }
        }

        private void ZapiszTekstOdszyfrowany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz plik",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName))
                    {
                        writetext.Write(TekstOdszyfrowanyTB.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać tekstu odszyfrowanego");
            }
        }

        private void SzyfrujTekstButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TekstJawnyTB.Text == string.Empty)
                    throw new Exception();
                TekstZaszyfrowanyTB.Clear();
                foreach (char letter in TekstJawnyTB.Text)
                {
                    int code = letter;
                    BigInteger m = code;
                    TekstZaszyfrowanyTB.Text += Encrypt(private_key_wczytanyA, m).ToString() + "/";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystapil błąd podczas szyfrowania");
            }
        }

        private void DeszyfrujTekstButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TekstZaszyfrowanyTB.Text == string.Empty)
                    throw new Exception();
                TekstOdszyfrowanyTB.Clear();
                string tekst = TekstZaszyfrowanyTB.Text;
                string ciag;
                Regex r = new Regex(@"\d+");
                for (Match m = r.Match(tekst); m.Success; m = m.NextMatch())
                {
                    ciag = m.Value.ToString();
                    BigInteger c = BigInteger.Parse(ciag);
                    BigInteger value = Decrypt(public_key_wczytanyA, c);
                    TekstOdszyfrowanyTB.Text += (char)value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystapil blad z deszyfrowaniem.");
            }
        }

        private void WyznaczPiQ()
        {
            //BigInteger min, max;
            //bool succeeded1 = BigInteger.TryParse(ZakresOD.Text, out min);
            //bool succeeded2 = BigInteger.TryParse(ZakresDO.Text, out max);
                Pa = 0;
                Qa = 0;
                Pb = 0;
                Qb = 0;
                //primes = new LiczbyPierwsze(2, 10000);
                int k;
                while (Pa < 1000)
                {
                    k = rand.Next(0, primes.GetList().Count - 1);
                    Pa = primes.GetList()[k];
                }
                while (Qa < 1000)
                {
                    k = rand.Next(0, primes.GetList().Count - 1);
                    Qa = primes.GetList()[k];
                }
            while (Pb < 1000)
            {
                k = rand.Next(0, primes.GetList().Count - 1);
                Pb = primes.GetList()[k];
            }
            while (Qb < 1000)
            {
                k = rand.Next(0, primes.GetList().Count - 1);
                Qb = primes.GetList()[k];
            }
        }
        private void WyznaczPHI()
        {
            PHIa = (Pa - 1) * (Qa - 1);
            PHIb = (Pb - 1) * (Qb - 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var okno = new Oprogramie();
            okno.Show();
        }

        private void WyznaczN()
        {
            Na = Pa * Qa;
            Nb = Pb * Qb;
        }
        private void WyznaczE()
        {
            int i = 0;
            do
            {
                Ea = primes.GetList()[i];
                i++;
            } while (BigInteger.GreatestCommonDivisor(Ea, PHIa) != 1 || i >= primes.GetList().Count);
            i = 0;
            do
            {
                Eb = primes.GetList()[i];
                i++;
            } while (BigInteger.GreatestCommonDivisor(Eb, PHIb) != 1 || i >= primes.GetList().Count);
        }

        private void WyznaczD()
        {
            BigInteger i = 2;
            while (true)
            {
                if ((Ea * i - 1) % PHIa == 0)
                {
                    Da = i;
                    break;
                }
                i++;
            }
            i = 2;
            while (true)
            {
                if ((Eb * i - 1) % PHIb == 0)
                {
                    Db = i;
                    break;
                }
                i++;
            }
        }
        private void GenerujKluczeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WyznaczPiQ();
                WyznaczN();
                WyznaczPHI();
                WyznaczE();
                WyznaczD();
                public_keyA = new RSA_key(Na, Ea);
                private_keyA = new RSA_key(Na, Da);
                public_keyB = new RSA_key(Nb, Eb);
                private_keyB = new RSA_key(Nb, Db);
                KluczPublicznyLabel.Content = "Wygenerowany";
                KluczPrywatnyLabel.Content = "Wygenerowany";
                KluczPublicznyLabel.Foreground = Brushes.Green;
                KluczPrywatnyLabel.Foreground = Brushes.Green;
                PodglądZmiennych.IsEnabled = true;
                ZapiszKluczPublicznyButton.IsEnabled = true;
                ZapiszKluczPrywatnyButton.IsEnabled = true;

                KluczPublicznyLabelB.Content = "Wygenerowany";
                KluczPrywatnyLabelB.Content = "Wygenerowany";
                KluczPublicznyLabelB.Foreground = Brushes.Green;
                KluczPrywatnyLabelB.Foreground = Brushes.Green;
                ZapiszKluczPublicznyButtonB.IsEnabled = true;
                ZapiszKluczPrywatnyButtonB.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystapil nieoczekiwany błąd \n\n\n" + ex);
            }
        }
        private BigInteger Encrypt(RSA_key public_key, BigInteger m)
        {
            return BigInteger.ModPow(m, public_key.x, public_key.n);
        }

        private BigInteger Decrypt(RSA_key private_key, BigInteger c)
        {
            return BigInteger.ModPow(c, private_key.x, private_key.n);
        }

    }
}
