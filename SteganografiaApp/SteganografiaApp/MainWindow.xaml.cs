using Microsoft.Win32;
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
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;

namespace SteganografiaApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SzyfrujButton.IsEnabled = false;
            DeszyfrujButton.IsEnabled = false;
        }

        private Bitmap Obraz;
        private Bitmap Obraz2;
        int pojemnoscszyfrowania = 0;
        string path;
        Bitmap output;
        private void WczytajObrazButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "BMP (*.bmp)|*.bmp|PNG (*.png)|*.png|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                path = openFileDialog1.FileName;
                Obraz = new Bitmap(openFileDialog1.FileName,true);
                ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                image1.Source = imgSource;
                SzyfrujButton.IsEnabled = true;
            }

            if (Obraz != null) pojemnoscszyfrowania = Obraz.Height * Obraz.Width;

        }

        private void SzyfrujButton_Click(object sender, RoutedEventArgs e)
        {
            var sizeoftext = (Changery.StringToBitArray(DoZaszyfrowaniaText.Text).Length);
            if (sizeoftext > pojemnoscszyfrowania)
            {
                MessageBox.Show("Niestety, tekst jest zbyt długi dla wczytanego obrazu. Wczytaj większy obraz lub skróć tekst tekst.");
            }
            else
            {
                if (CheckBox.IsChecked == false)
                {
                    Obraz = Szyfry.Szyfruj(Obraz, DoZaszyfrowaniaText.Text);
                    ImageSource imgSource = new BitmapImage(new Uri(path));
                    image2.Source = imgSource;
                    //DeszyfrujButton.IsEnabled = true;
                    MessageBox.Show("Pomyślnie zaszyfrowano.");
                }
                else
                {
                    Obraz = Szyfry.Ukryj2(Obraz, DoZaszyfrowaniaText.Text);
                    ImageSource imgSource = new BitmapImage(new Uri(path));
                    image2.Source = imgSource;
                    //DeszyfrujButton.IsEnabled = true;
                    MessageBox.Show("Pomyślnie zaszyfrowano.");
                }

            }

        }

        private void ZapiszZaszyfrowanyObrazButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "BMP (*.bmp)|*.bmp|PNG (*.png)|*.png|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Obraz.Save(saveFileDialog.FileName, ImageFormat.Png);
                MessageBox.Show("Pomyślnie zapisano obraz jako: \n" + saveFileDialog.FileName);
            }
        }

        private void DeszyfrujButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                string tekstOdszyfrowany = Szyfry.Odkryj2(Obraz2);
                DoZaszyfrowaniaText.Text = tekstOdszyfrowany;
            }
            else
            {
                string tekstOdszyfrowany = Szyfry.DekodujObraz(Obraz2);
                DoZaszyfrowaniaText.Text = tekstOdszyfrowany;
            }
        }

        private void WczytajObrazZaszyfrowanyButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "BMP (*.bmp)|*.bmp|PNG (*.png)|*.png|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                path = openFileDialog1.FileName;
                Obraz2 = new Bitmap(openFileDialog1.FileName);
                if (Obraz == Obraz2)
                    MessageBox.Show("Succeesss!");
                //niewazne
                ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                image2.Source = imgSource;
                //do tad
                DeszyfrujButton.IsEnabled = true;
            }
        }

        private void WczytajTekstButton_Click(object sender, RoutedEventArgs e)
        {
            string file1 = string.Empty;
            Encoding enc = Encoding.GetEncoding("UTF-8");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Wybierz plik *.txt do zaszyfrowania";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == true)
            {
                file1 = openFileDialog1.FileName;
            }
            try
            {
                using (StreamReader sr = new StreamReader(file1, enc))
                {
                    String line = sr.ReadToEnd();
                    DoZaszyfrowaniaText.Clear();
                    DoZaszyfrowaniaText.Text = line;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

        private void ZapiszTekstButton_Click(object sender, RoutedEventArgs e)
        {
            string file1 = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                file1 = saveFileDialog.FileName;
            }
            try
            {
                using (StreamWriter writetext = new StreamWriter(file1))
                {
                    writetext.Write(DoZaszyfrowaniaText.Text.ToString());
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error! The file could not be read.");
            }
        }

 
    }
}
