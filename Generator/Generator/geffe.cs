using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class geffe
    {
        private lfsr lfsr1;
        private lfsr lfsr2;
        private lfsr lfsr3;

        geffe(lfsr _1, lfsr _2, lfsr _3)
        {
            lfsr1 = _1;
            lfsr2 = _2;
            lfsr3 = _3;
        }

        public int next()
        {
            int toReturn = 0;
            switch(lfsr1.shift())
            {
                case 0:
                    {
                        toReturn = lfsr2.shift();
                        lfsr3.shift();
                    }break;
                case 1:
                    {
                        toReturn = lfsr3.shift();
                        lfsr2.shift();
                    }break;
            }
            return toReturn;
        }
    }
}
