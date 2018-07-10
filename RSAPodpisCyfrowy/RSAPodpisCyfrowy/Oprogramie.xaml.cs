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
using System.Windows.Shapes;

namespace RSAPodpisCyfrowy
{
    /// <summary>
    /// Logika interakcji dla klasy Oprogramie.xaml
    /// </summary>
    public partial class Oprogramie : Window
    {
        public Oprogramie()
        {
            InitializeComponent();
            SetText();
        }

        private void SetText()
        {
            string tekst = "Generator kluczy: \n" +
                "- Klucz generowany jest automatycznie, wybierana jest losowa wartość liczby pierwszej P i Q w zakresie 1000 - 9999 \n" +
                "- N jest obliczane jako P*Q \n" +
                "- PHI jest obliczane jako (P-1)*(Q-1) \n" +
                "- E jest liczba pierwszą mniejszą od PHI, dla których największy wspólny dzielnik wynosi 1 \n" +
                "- D obliczane jest tak, aby (E * i - 1) % PHI == 0 \n" +
                "Obsługa błędow: \n" +
                "- W przypadku, gdy klucze zapisane są w złym formacie. Obsługiwany format: *.publickey oraz *.privatekey \n" +
                "- Klucz prywatny musi być odpowiedni, do odszyfrowania wiadomości \n" +
                "Szyfrowanie: \n" +
                "Tekst jest podzielony na bloki, odpowiadające pojedynczym literom(znakom) w kodzie ASCII, każdy blok oddzielony jest znakiem / \n"+
                "Tekst szyfrowany jest najpierw KLUCZEM_PRYWATNYM, a następnie deszyfrowany KLUCZEM_PUBLICZNYM";

            TxtBox.Text = tekst;
        }
    }
}
