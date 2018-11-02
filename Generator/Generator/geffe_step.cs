using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class geffe_step
    {
        private lfsr lfsr1;
        private lfsr lfsr2;
        private lfsr lfsr3;

        public geffe_step(lfsr _1, lfsr _2, lfsr _3)
        {
            lfsr1 = _1;
            lfsr2 = _2;
            lfsr3 = _3;
        }

        public string get1()
        {
            return lfsr1.getRegister();
        }

        public string get2()
        {
            return lfsr2.getRegister();
        }
        public string get3()
        {
            return lfsr3.getRegister();
        }

        public Tuple<int, int> next()
        {
            int toReturn = 0;
            int c = 0;
            switch (lfsr1.shift())
            {
                case 0:
                    {
                        toReturn = lfsr2.shift();
                        c = 0;
                        lfsr3.shift();
                    }
                    break;
                case 1:
                    {
                        toReturn = lfsr3.shift();
                        c = 1;
                        lfsr2.shift();
                    }
                    break;
            }
            return new Tuple<int, int>(toReturn, c);
        }
    }
}
