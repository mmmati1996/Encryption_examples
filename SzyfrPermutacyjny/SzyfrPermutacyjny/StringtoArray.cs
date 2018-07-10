using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SzyfrPermutacyjny
{
    class StringtoArray
    {
        public static int[] StoA (string tekst, int size)
        {
            int[] t = new int[size];
            Match match = Regex.Match(tekst, @"\d+");
            if(match.Success)
                t[0] = Convert.ToInt32(match.Value);
            for(int i = 1; i < size; i++)
            {
                match = match.NextMatch();
                if (match.Success)
                    t[i] = Convert.ToInt32(match.Value);
            }
            return t;
        }
        public static int length (string tekst)
        {
            int k = 0;
            Match match = Regex.Match(tekst, @"\d+");
            if (match.Success)
                k++;
            for (int i = 1; i < 17; i++)
            {
                match = match.NextMatch();
                if (match.Success)
                    k++;
            }
            return k;
        }

        public static bool CheckKey(int[] tab)
        {
            bool tr = false;
            for(int i = 0; i < tab.Length; i++)
            {
                if (!tab.Contains(i))
                {
                    tr = false;
                    break;
                }
                else
                    tr = true;
            }
            if (tab.Length > 16)
                tr = false;
            return tr;
        }
    }
}
