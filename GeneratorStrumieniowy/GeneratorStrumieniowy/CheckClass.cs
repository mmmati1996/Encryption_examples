using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneratorStrumieniowy
{
    class CheckClass
    {
        public static Boolean CheckIfInt(string tekst)
        {
            return Regex.IsMatch(tekst, @"^\d+$");
        }
        public static Boolean CheckLength(string tekst1, string tekst2)
        {
            if (tekst1.Length > tekst2.Length)
                return false;
            else
                return true;
        }
    }
}
