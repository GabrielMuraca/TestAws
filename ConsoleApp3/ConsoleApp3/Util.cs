using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Util : IConsoleApp3
    {
        /// <summary>
        /// 
        /// </summary>
        public bool RevisionPalindromo(string esPalindromo)
        {
            for (int i = 0; i < esPalindromo.Length / 2; i++)
            {
                if (esPalindromo[i] != esPalindromo[esPalindromo.Length - 1 - i])
                    return false;
            }

            return true;
        }
    }
}
