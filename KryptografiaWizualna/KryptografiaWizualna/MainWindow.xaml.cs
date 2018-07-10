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
using System.Drawing.Imaging;

namespace KryptografiaWizualna
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
        Random rand = new Random();
        Bitmap ObrazWczytany;
        String ObrazWczytanyPath;
        Bitmap ObrazUdzial1;
        Bitmap ObrazUdzial2;

        private void WczytajObrazCzarnoBialy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Wybierz obraz.",
                Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                ObrazWczytanyPath = openFileDialog1.FileName;
                ObrazWczytany = new Bitmap(ObrazWczytanyPath, true);
                ImageSource imgSource = new BitmapImage(new Uri(ObrazWczytanyPath));
                Image1.Source = imgSource;
            }
        }

        private string GetExtensionOfFile(string path)
        {
            return path.Substring(path.Length - 3);
        }
        private int GenerateRandom()
        {
            return rand.Next(0, 2);
        }

        private void UtworzUdzialy(Bitmap Obraz)
        {
            System.Drawing.Color kolor;
            ObrazUdzial1 = new Bitmap(Obraz.Width*2, Obraz.Height*2);
            ObrazUdzial2 = new Bitmap(Obraz.Width*2, Obraz.Height*2);
            /*
             Dzielimy czarny pixel na:
             Obraz1    Obraz2            Obraz1  Obraz2
             |0 1|     |1 0|       LUB   |1 0|   |0 1|
             |1 0|     |0 1|             |0 1|   |1 0|
             Dzielimy biały pixel na:
             Obraz1    Obraz2            Obraz1  Obraz2
             |0 1|     |0 1|       LUB   |1 0|   |1 0|
             |1 0|     |1 0|             |0 1|   |0 1|
             */
            int x = 0, y = 0; //wysokość i długość udziałów
                for (int i = 0; i < Obraz.Width; i++)
                {
                    for (int j = 0; j < Obraz.Height; j++)
                    {
                        kolor = Obraz.GetPixel(i, j);
                        if (kolor != System.Drawing.Color.FromArgb(255, 255, 255) && kolor != System.Drawing.Color.FromArgb(0, 0, 0))
                        {
                            Console.WriteLine(kolor.ToString());
                            throw new Exception("Wczytany obraz nie jest czarno-biały!!!!");
                        }
                        else
                        {
                            if (kolor == System.Drawing.Color.FromArgb(0, 0, 0))
                            {
                                if(GenerateRandom()==0)
                                {
                                    
                                    //opcja 1
                                    ObrazUdzial1.SetPixel(x, y, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x+1, y, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x, y+1, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x+1, y+1, System.Drawing.Color.White);

                                    ObrazUdzial2.SetPixel(x, y, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x + 1, y, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x, y + 1, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x + 1, y + 1, System.Drawing.Color.Black);
                                }
                                else
                                {
                                    //opcja 2
                                    ObrazUdzial1.SetPixel(x, y, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x + 1, y, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x, y + 1, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x + 1, y + 1, System.Drawing.Color.Black);

                                    ObrazUdzial2.SetPixel(x, y, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x + 1, y, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x, y + 1, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x + 1, y + 1, System.Drawing.Color.White);
                                }
                            }
                            else
                            {
                                if (GenerateRandom() == 0)
                                {
                                    //opcja 1
                                    ObrazUdzial1.SetPixel(x, y, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x + 1, y, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x, y + 1, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x + 1, y + 1, System.Drawing.Color.White);

                                    ObrazUdzial2.SetPixel(x, y, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x + 1, y, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x, y + 1, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x + 1, y + 1, System.Drawing.Color.White);
                                }
                                else
                                {
                                    //opcja 2
                                    ObrazUdzial1.SetPixel(x, y, System.Drawing.Color.Black);
                                    ObrazUdzial1.SetPixel(x + 1, y, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x, y + 1, System.Drawing.Color.White);
                                    ObrazUdzial1.SetPixel(x + 1, y + 1, System.Drawing.Color.Black);

                                    ObrazUdzial2.SetPixel(x, y, System.Drawing.Color.Black);
                                    ObrazUdzial2.SetPixel(x + 1, y, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x, y + 1, System.Drawing.Color.White);
                                    ObrazUdzial2.SetPixel(x + 1, y + 1, System.Drawing.Color.Black);
                                }
                            }
                        }
                        y += 2;
                    }
                    y = 0;
                    x += 2;
                }
                string path1 = string.Empty;
                string path2 = string.Empty;
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Zapisz udział 1",
                    Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    path1 = saveFileDialog.FileName;
                    ObrazUdzial1.Save(saveFileDialog.FileName);
                }
                else
                {
                    throw new Exception("Nie zapisano obrazu: Udział 1");
                }

                SaveFileDialog saveFileDialog2 = new SaveFileDialog()
                {
                    Title = "Zapisz udział 2",
                    Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (saveFileDialog2.ShowDialog() == true)
                {
                    path2 = saveFileDialog2.FileName;
                    ObrazUdzial2.Save(saveFileDialog2.FileName);
                }
                else
                {
                    throw new Exception("Nie zapisano obrazu: Udział 2");
                }
                Udzial1Image.Source = new BitmapImage(new Uri(path1));
                Udzial2Image.Source = new BitmapImage(new Uri(path2));

        }

        Bitmap ObrazWynikowy;
        private void PolaczObrazy(Bitmap obraz1, Bitmap obraz2)
        {
                if (obraz1.Size != obraz2.Size)
                    throw new Exception("Wczytane obrazy nie są takie same !");
                ObrazWynikowy = obraz1;
                for (int i = 0; i < ObrazWynikowy.Width; i++)
                {
                    for (int j = 0; j < ObrazWynikowy.Height; j++)
                    {
                        if (obraz2.GetPixel(i, j) == System.Drawing.Color.FromArgb(0, 0, 0))
                            ObrazWynikowy.SetPixel(i, j, System.Drawing.Color.Black);
                    }
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObrazWczytany == null)
                    throw new Exception("Nie wczytano żadnego obrazu");
                UtworzUdzialy(ObrazWczytany);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystapil blad: " + ex);
            }
        }

        private void Udzial1Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Wybierz udzial 1",
                Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                ObrazUdzial1 = new Bitmap(openFileDialog1.FileName, true);
                ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                WczytanyUdzial1.Source = imgSource;
            }
        }

        private void Udzial2Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Wybierz udzial 2",
                Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                ObrazUdzial2 = new Bitmap(openFileDialog1.FileName, true);
                ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                WczytanyUdzial2.Source = imgSource;
            }
        }

        private void PolaczObrazyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObrazUdzial1 == null || WczytanyUdzial1.Source == null)
                    throw new Exception("Nie wczytano obrazu: Udział 1");
                if (ObrazUdzial2 == null || WczytanyUdzial2.Source == null)
                    throw new Exception("Nie wczytano obrazu: Udział 2");
                PolaczObrazy(ObrazUdzial1, ObrazUdzial2);
                SaveFileDialog saveFileDialog2 = new SaveFileDialog()
                {
                    Title = "Zapisz odszyfrowany obraz",
                    Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|ALL (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                string path = string.Empty;
                if (saveFileDialog2.ShowDialog() == true)
                {
                    path = saveFileDialog2.FileName;
                    ObrazWynikowy.Save(saveFileDialog2.FileName);
                }
                else
                {
                    throw new Exception("Nie zapisano połączonego obrazu");
                }
                OdszyfrowanyObraz.Source = new BitmapImage(new Uri(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex);
            }
        }

    }
}
