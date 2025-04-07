using System;
using System.Collections.Generic;
using System.Text;

namespace MarmotAp.Helpers
{
    public class FindDuplicateChar
    {
        static public int repeatcount(string str, char ch)
        {

            var count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (ch == str[i])
                {
                    count++;
                }

            }

            return count;
        }
    }
   
}
