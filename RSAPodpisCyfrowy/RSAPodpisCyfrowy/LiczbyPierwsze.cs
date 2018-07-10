using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSAPodpisCyfrowy
{
    class LiczbyPierwsze
    {
        private List<BigInteger> TablicaLiczbPierwszych;

        public LiczbyPierwsze(BigInteger min,BigInteger max) {
            TablicaLiczbPierwszych = new List<BigInteger>();
            WyznaczLiczby(min,max);
        }

        public void WyznaczLiczby(BigInteger min,BigInteger max)
        {
            TablicaLiczbPierwszych.Clear();
            if (max < 10000)
                throw new Exception("Liczba musi być większa od 10000");
            
            for (BigInteger j = min; j < max; j++)
            {
                bool isFirst = false;
                BigInteger SqrtMax = (BigInteger)Math.Ceiling(Math.Pow(Math.E, BigInteger.Log(j) / 2));
                for (BigInteger i = 2; i <= SqrtMax; i++)
                {
                    if (j % i == 0)
                    {
                        isFirst = false;
                        break;
                    }
                    else
                    {
                       isFirst = true;
                    }
                }
                if (isFirst)
                {
                    TablicaLiczbPierwszych.Add(j);
                }

            }
        }

        public List<BigInteger> GetList()
        {
            return TablicaLiczbPierwszych;
        }

    }
}
