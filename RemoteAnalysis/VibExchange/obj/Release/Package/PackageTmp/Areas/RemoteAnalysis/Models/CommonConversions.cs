using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    public class CommonConversions
    {
        public static string DeciamlToHexadeciaml1(int number)
        {
            string[] hexvalues = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string result = null, final = null;
            int rem = 0;
            try
            {
                while (true)
                {
                    rem = (number % 16);
                    result += hexvalues[rem].ToString();

                    if (number < 16)
                        break;
                    number = (number / 16);

                }

                for (int i = (result.Length - 1); i >= 0; i--)
                {
                    final += result[i];
                }
            }
            catch
            {

            }

            return final;
        }


        public static int HexadecimaltoDecimal(string hexadecimal)
        {
            int result = 0;

            for (int i = 0; i < hexadecimal.Length; i++)
            {
                result += Convert.ToInt32(GetNumberFromNotation(hexadecimal[i]) * Math.Pow(16, Convert.ToInt32(hexadecimal.Length) - (i + 1)));
            }
            return Convert.ToInt32(result);
        }
        private static int GetNumberFromNotation(char c)
        {
            if (c == 'A')
                return 10;
            else if (c == 'B')
                return 11;
            else if (c == 'C')
                return 12;
            else if (c == 'D')
                return 13;
            else if (c == 'E')
                return 14;
            else if (c == 'F')
                return 15;

            return Convert.ToInt32(c.ToString());
        }


        public static string DeciamlToHexadeciaml(int number)
        {
            string[] hexvalues = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string result = null, final = null;
            int rem = 0;
            try
            {
                while (true)
                {
                    rem = (number % 16);
                    result += hexvalues[rem].ToString();

                    if (number < 16)
                        break;
                    result += ',';
                    number = (number / 16);

                }

                for (int i = (result.Length - 1); i >= 0; i--)
                {
                    final += result[i];
                }
            }
            catch (Exception ex)
            {

            }
            return final;

        }
    }
}