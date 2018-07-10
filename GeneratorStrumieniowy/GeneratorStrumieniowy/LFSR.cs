using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorStrumieniowy
{
    class LFSR
    {
        private uint start_state; //np. ABCD zapisany szesnastkowo
        private uint lfsr;
        private uint bit;
        private int[] t;
        private int length = 16;

        private int pomoc;
        public LFSR(uint start, string wielomian) {
            this.start_state = start;
            this.lfsr = start_state;
            this.t = StringToSomething.StoA(wielomian);
            this.pomoc = t.Length;
        }

        public uint work()
        {

            //wielomian x^11+x^2+1 (dla przykladu)
            if (pomoc == 3)
            {
                bit = ((lfsr >> length - t[0]) ^ (lfsr >> length - t[1])) & ((uint)t[2] + 1);
                lfsr = (lfsr >> 1) | (bit << length - 1);
            }
            //wielomian
            else if (pomoc == 4)
            {
                bit = ((lfsr >> length - t[0]) ^ (lfsr >> length - t[1]) ^ (lfsr >> length - t[2])) & ((uint)t[3] + 1);
                lfsr = (lfsr >> 1) | (bit << length - 1);
            }
            //wielomian x^16+x^14+x^13+x^11+1 (dla przykladu)
            else if (pomoc == 5)
            {
                bit = ((lfsr >> length - t[0]) ^ (lfsr >> length - t[1]) ^ (lfsr >> length - t[2]) ^ (lfsr >> length - t[3])) & ((uint)t[4] + 1);
                lfsr = (lfsr >> 1) | (bit << length - 1);
            }
            //wielomian x^66+x^8+x^3+x^1+1 (dla przykladu)
            else if (pomoc == 6)
            {
                bit = ((lfsr >> length - t[0]) ^ (lfsr >> length - t[1]) ^ (lfsr >> length - t[2]) ^ (lfsr >> length - t[3]) ^ (lfsr >> length - t[4])) & ((uint)t[5] + 1);
                lfsr = (lfsr >> 1) | (bit << length - 1);
            }
            //wielomian x^89+x^66+x^8+x^3+x^1+1 (dla przykladu)
            else if (pomoc == 7)
            {
                bit = ((lfsr >> length - t[0]) ^ (lfsr >> length - t[1]) ^ (lfsr >> length - t[2]) ^ (lfsr >> length - t[3]) ^ (lfsr >> length - t[4]) ^ (lfsr >> length - t[5])) & ((uint)t[6] + 1);
                lfsr = (lfsr >> 1) | (bit << length - 1);
            }
            return bit;
        }

        

    }
}
