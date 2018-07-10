using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganografiaApp
{
    static class Changery
    {
        public static string BitsToString(BitArray bits)
        {
            var TekstByte = ToByteArray(bits);
            string something = Encoding.UTF8.GetString(TekstByte);
            return something;
        }

        public static BitArray StringToBitArray(string something)
        {
            byte[] TekstByte = Encoding.UTF8.GetBytes(something);
            BitArray bits = new BitArray(TekstByte);
            return bits;
        }

        public static byte[] ToByteArray(BitArray bits)
        {
            int size;
            size = bits.Length;
            size = size / 8;
            byte[] ret = new byte[size];
            bits.CopyTo(ret, 0);
            int counter = 0;
            int retcounter = 0;
            foreach (byte t in ret)
            {
                if (t != 0) counter++;
            }
            byte[] finalret = new byte[counter];
            for (int i = 0; i < ret.Length; i++)
            {
                if (ret[i] != 0)
                {
                    finalret[retcounter++] = ret[i];
                }
            }
            return finalret;
        }
    }
}
