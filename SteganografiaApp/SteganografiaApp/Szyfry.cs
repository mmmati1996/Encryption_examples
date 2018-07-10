using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganografiaApp
{
    static class Szyfry
    {
        public static Bitmap Szyfruj(Bitmap Obraz, string tekst)
        {
            //Zmiany bitów "Niezauważalne"
            //1 bit w kolorze zielonym
            //2 bity w czerwonym
            //5 bitów w niebieskiem
            //Szyfruję w kolejności R G B

            //Kodowanie zielone - parzyste->0 nieparzyste->1
            //Kodowanie czerwone %4 0->00 1->01 2->10 3->11
            //Kodowanie niebieskie %32 0->0 0000 .... 31-> 1 1111

            BitArray TekstBitowy = Changery.StringToBitArray(tekst);
            int PozycjaWbitach = 0;

            int Height = Obraz.Height;
            int Width = Obraz.Width;
            Color kolor;
            int R, G, B;
            R = G = B = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    kolor = Obraz.GetPixel(j, i);
                    R = kolor.R;
                    B = kolor.B;
                    G = kolor.G;

                    var Zakoduj = 0;

                    //Kodowanie czerwonego

                    //1 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj++;
                    PozycjaWbitach++;

                    //2bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj += 2;
                    PozycjaWbitach++;
                    R = R - R % 4 + Zakoduj;

                    //Kodowanie zielonego 1 bitowe
                    Zakoduj = 0;
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj = 1;
                    PozycjaWbitach++;

                    G = G - G % 2 + Zakoduj;

                    //Kodowanie niebieskiego 5 bitowe
                    Zakoduj = 0;
                    //1 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj = 1;
                    PozycjaWbitach++;

                    //2 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj += 2;
                    PozycjaWbitach++;

                    //3 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj += 4;
                    PozycjaWbitach++;

                    //4 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj += 8;
                    PozycjaWbitach++;

                    //5 bit
                    if (PozycjaWbitach < TekstBitowy.Length)
                        if (TekstBitowy[PozycjaWbitach]) Zakoduj += 16;
                    PozycjaWbitach++;


                    B = B - B % 32 + Zakoduj;

                    Obraz.SetPixel(j, i, Color.FromArgb(R, G, B));
                    //if (PozycjaWbitach == TekstBitowy.Length)
                    //    break;
                }
            }
            return Obraz;
        }
        public static string DekodujObraz(Bitmap Obraz)
        {
            //Zmiany bitów "Niezauważalne"
            //1 bit w kolorze zielonym
            //2 bity w czerwonym
            //5 bitów w niebieskiem
            //Szyfruję w kolejności R G B

            //Kodowanie zielone - parzyste->0 nieparzyste->1
            //Kodowanie czerwone %4 0->00 1->01 2->10 3->11
            //Kodowanie niebieskie %32 0->0 0000 .... 31-> 1 1111


            int PozycjaWbitach = 0;

            int Height = Obraz.Height;
            int Width = Obraz.Width;
            BitArray TekstBitowy = new BitArray((Height * Width * 8));
            Color kolor;
            int R, G, B;
            R = G = B = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    kolor = Obraz.GetPixel(j, i);
                    R = kolor.R;
                    B = kolor.B;
                    G = kolor.G;

                    var Zakoduj = 0;

                    Zakoduj = R % 4;
                    switch (Zakoduj)
                    {
                        case 0:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            break;
                        case 1:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            break;
                        case 2:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            break;
                        case 3:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            break;
                    }
                    PozycjaWbitach += 2;

                    //Kodowanie zielonego 1 bitowe

                    TekstBitowy[PozycjaWbitach] = (G % 2 == 1);

                    PozycjaWbitach++;
                    //Kodowanie niebieskiego 5 bitowe
                    Zakoduj = B % 32;
                    switch (Zakoduj)
                    {
                        case 0:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 1:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 2:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 3:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;

                        case 4:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 5:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 6:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 7:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;

                        case 8:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 9:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 10:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 11:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;

                        case 12:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 13:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 14:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;
                        case 15:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = false;
                            break;

                        case 16:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 17:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 18:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 19:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;

                        case 20:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 21:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 22:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 23:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = false;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;

                        case 24:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 25:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 26:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 27:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = false;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;

                        case 28:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 29:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = false;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 30:
                            TekstBitowy[PozycjaWbitach] = false;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                        case 31:
                            TekstBitowy[PozycjaWbitach] = true;
                            TekstBitowy[PozycjaWbitach + 1] = true;
                            TekstBitowy[PozycjaWbitach + 2] = true;
                            TekstBitowy[PozycjaWbitach + 3] = true;
                            TekstBitowy[PozycjaWbitach + 4] = true;
                            break;
                    }
                    PozycjaWbitach += 5;

                }
            }
            string tekstOdszyfrowany = Changery.BitsToString(TekstBitowy);
            tekstOdszyfrowany = tekstOdszyfrowany.Replace('\0', ' ');
            return tekstOdszyfrowany;
        }
        public static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static Bitmap Ukryj2(Bitmap obraz, String tekst)
        {
            Bitmap img = obraz;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (i < 1 && j < tekst.Length)
                    {
                        Console.WriteLine("R = [" + i + "][" + j + "] = " + pixel.R);
                        Console.WriteLine("G = [" + i + "][" + j + "] = " + pixel.G);
                        Console.WriteLine("G = [" + i + "][" + j + "] = " + pixel.B);

                        char letter = Convert.ToChar(tekst.Substring(j, 1));
                        int value = Convert.ToInt32(letter);
                        Console.WriteLine("letter : " + letter + " value : " + value);

                        img.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));
                    }

                    if (i == img.Width - 1 && j == img.Height - 1)
                    {
                        img.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, tekst.Length));
                    }
    
                }
            }
            return img;
        }
        public static string Odkryj2(Bitmap obraz)
        {
            Bitmap img = obraz;
            string message = "";

            Color lastpixel = img.GetPixel(img.Width - 1, img.Height - 1);
            int msgLength = lastpixel.B;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (i < 1 && j < msgLength)
                    {
                        int value = pixel.B;
                        char c = Convert.ToChar(value);
                        string letter = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(c) });

                        message = message + letter;
                    }
                }
            }
            return message;
        }
    }
}
