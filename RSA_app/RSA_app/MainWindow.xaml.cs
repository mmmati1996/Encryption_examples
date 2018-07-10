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

namespace RSA_app
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private BigInteger P;
        private BigInteger Q;
        private BigInteger PHI;
        private BigInteger N;
        private BigInteger E;
        private BigInteger D;
        private LiczbyPierwsze primes;
        Random rand = new Random();

        RSA_key public_key;
        RSA_key private_key;
        RSA_key public_key_wczytany;
        RSA_key private_key_wczytany;
        private void WyznaczPiQ()
        {
            BigInteger min, max;
            bool succeeded1 = BigInteger.TryParse(ZakresOD.Text, out min);
            bool succeeded2 = BigInteger.TryParse(ZakresDO.Text, out max);
            if (succeeded1 && succeeded2)
            {
                P = 0;
                Q = 0;
                primes = new LiczbyPierwsze(min, max);
                int k;
                while (P < 1000)
                {
                    k = rand.Next(0, primes.GetList().Count - 1);
                    P = primes.GetList()[k];
                }
                while (Q < 1000)
                {
                    k = rand.Next(0, primes.GetList().Count - 1);
                    Q = primes.GetList()[k];
                }
            }
            else
                throw new Exception("Podano złe wartości z zakresu P i Q");
        }
        private void WyznaczPHI()
        {
            PHI = (P - 1) * (Q - 1);
        }
        private void WyznaczN()
        {
            N = P * Q;
        }
        private void WyznaczE()
        {
            int i = 0;
            do
            {
                E = primes.GetList()[i];
                i++;
            } while (BigInteger.GreatestCommonDivisor(E, PHI) != 1 || i>=primes.GetList().Count);
        }

        private void WyznaczD()
        {
            BigInteger i = 2;
            while (true)
            {
                if ((E * i - 1) % PHI == 0)
                {
                    D = i;
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
                public_key = new RSA_key(N, E);
                private_key = new RSA_key(N, D);
                KluczPublicznyLabel.Content = "Wygenerowany";
                KluczPrywatnyLabel.Content = "Wygenerowany";
                KluczPublicznyLabel.Foreground = Brushes.Green;
                KluczPrywatnyLabel.Foreground = Brushes.Green;
                PodglądZmiennych.IsEnabled = true;
                ZapiszKluczPublicznyButton.IsEnabled = true;
                ZapiszKluczPrywatnyButton.IsEnabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystapil nieoczekiwany błąd \n\n\n" + ex);
            }
        }

        private void PodglądZmiennych_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Wartość P = "+P+"\n"+
                "Wartość Q = " + Q + "\n" +
                "Wartość N = " + N + "\n" +
                "Wartość PHI = " + PHI + "\n" +
                "Wartość E = " + E + "\n" +
                "Wartość D = " + D
                );
        }

        private BigInteger Encrypt(RSA_key public_key, BigInteger m)
        {
            return BigInteger.ModPow(m, public_key.x, public_key.n);
        }

        private BigInteger Decrypt(RSA_key private_key, BigInteger c)
        {
            return BigInteger.ModPow(c, private_key.x, private_key.n);
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
                        writetext.WriteLine(N);
                        writetext.WriteLine(E);
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
                        writetext.WriteLine(N);
                        writetext.WriteLine(D);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie zapisać klucza prywatnego");
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
            catch(Exception ex)
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

        private void WczytajKluczPubliczny_Click(object sender, RoutedEventArgs e)
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
                        public_key_wczytany = new RSA_key(n1, e1);
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

        private void WczytajKluczPrywatny_Click(object sender, RoutedEventArgs e)
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
                        private_key_wczytany = new RSA_key(n1, d1);
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
                    TekstZaszyfrowanyTB.Text += Encrypt(public_key_wczytany, m).ToString() + "/";
                }
            }
            catch(Exception ex)
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
                    BigInteger value = Decrypt(private_key_wczytany, c);
                    TekstOdszyfrowanyTB.Text += (char)value;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystapil blad z deszyfrowaniem.");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var okno = new Oprogramie();
            okno.Show();
        }
    }
}
