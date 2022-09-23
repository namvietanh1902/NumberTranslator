using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class EnTranslator : ITranslator
    {
        public string Translate(string str)
        {
            string result = "";
            if (str.StartsWith("-"))
            {
                return "Minus " + Translate(str.Substring(1));
            }
            if (str.Contains("."))
            {
                return Translate(str.Split('.')[0]) + " Point " + Translate(str.Split('.')[1].TrimEnd(new char[] { '0' }));
            }

            str = str.TrimStart(new char[] { '0' });
            if (str == "") return "Zero";
            long number = Convert.ToInt64(str);
            if ((number / 1000000000) > 0)
            {
                result += Translate((number / 1000000000).ToString()) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                result += Translate((number / 1000000).ToString()) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                result += Translate((number / 1000).ToString()) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                result += Translate((number / 100).ToString()) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (result != "")
                    result += " And ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    result += unitsMap[number];
                else
                {
                    result += tensMap[number / 10];
                    if ((number % 10) > 0)
                        result += "-" + unitsMap[number % 10];
                }
            }

            return result;

        }
    }
}
