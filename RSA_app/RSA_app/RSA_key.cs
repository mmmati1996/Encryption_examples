using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA_app
{
    class RSA_key
    {
        public BigInteger n;
        public BigInteger x; // e lub d

        public RSA_key(BigInteger n, BigInteger x)
        {
            this.n = n;
            this.x = x;
        }
        
    }
}
