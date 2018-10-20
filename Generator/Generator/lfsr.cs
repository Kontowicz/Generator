using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class lfsr
    {
        private int[] register;
        private int[] positions;
        
        private void moveRight(int t)
        {
            for(int i = register.Length - 1; i > 0; i--)
            {
                register[i] = register[i-1];
            }
            register[0] = t;
        }
        public int shift()
        {
            List<int> tmp = new List<int>();
            int output = register[register.Length - 1];
            for (int i = 0; i < positions.Length; ++i)
            {
                tmp.Add(register[positions[i]]);
            }

            int[] valuesCalc = tmp.ToArray();
            int result = 0;
            foreach (int num in valuesCalc)
                result += num;
            result = result % 2;

            moveRight(result);

            return output;
        }
        
        public lfsr(string polynomial, string begin)
        {
            register = new int[begin.Length];
            string[] test = polynomial.Split('+');
            List<int> tmp = new List<int>();
            for (int i = 0; i < test.Length; ++i)
            {
                if(test[i][0]=='x')
                {
                    if(test[i] == "x")
                        tmp.Add(0);
                    else
                        tmp.Add(int.Parse(test[i].Remove(0, 1))-1);
                }
            }

            for(int i = 0; i < begin.Length; i++)
            {
                register[i] = int.Parse(begin[i].ToString());
            }

            positions = tmp.ToArray();
        }
    }
}
