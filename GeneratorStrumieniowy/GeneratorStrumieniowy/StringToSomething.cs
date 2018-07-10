using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GeneratorStrumieniowy
{
    class StringToSomething
    {
        public static int[] StoA(string tekst)
        {
            int[] t = new int[tekst.Length];
            int newsize = 0;
            Match match = Regex.Match(tekst, @"\d+");
            if (match.Success)
                t[0] = Convert.ToInt32(match.Value);
            for (int i = 1; i < tekst.Length; i++)
            {
                match = match.NextMatch();
                if (match.Success)
                {
                    t[i] = Convert.ToInt32(match.Value);
                    newsize++;
                }
            }
            int[] tab = new int[newsize + 1];
            for (int i = 0; i < newsize + 1; i++)
                tab[i] = t[i];
            return tab;
        }
        public static byte[] StoB(string tekst)
        {
            return Encoding.ASCII.GetBytes(tekst);
        }
        public static String ToBinary(Byte[] data)
        {
            return string.Join("",data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        public static string BtoS(byte[] tab)
        {
            return Encoding.ASCII.GetString(tab);
        }
        public static string BinaryToString(string binary)
        {
            if (string.IsNullOrEmpty(binary))
                throw new ArgumentNullException("binary");

            if ((binary.Length % 8) != 0)
                throw new ArgumentException("Binary string invalid (must divide by 8)", "binary");

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 8)
            {
                string section = binary.Substring(i, 8);
                int ascii = 0;
                try
                {
                    ascii = Convert.ToInt32(section, 2);
                }
                catch
                {
                    throw new ArgumentException("Binary string contains invalid section: " + section, "binary");
                }
                builder.Append((char)ascii);
            }
            return builder.ToString();
        }
        public static int XOR (char s1, char key)
        {
            int dec1 = Convert.ToInt32(s1);
            int dec2 = Convert.ToInt32(key);
            int result = dec2 ^ dec1;
            return result;
        }
    }
}
