using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class EnTranslator : ITranslator
    {
        string[] _units = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        string[] _tens = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        string[] _uppers = { "",
        "thousand",     "million",         "billion",       "trillion",       "quadrillion",
        "quintillion",  "sextillion",      "septillion",    "octillion",      "nonillion",
        "decillion",    "undecillion",     "duodecillion",  "tredecillion",   "quattuordecillion",
        "sexdecillion", "septendecillion", "octodecillion", "novemdecillion", "vigintillion" };
        

        
        public string TranslateGroup(int threeDigits)
        {
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = threeDigits / 100;
            int tensUnits = threeDigits % 100;

            // Hundreds rules
            if (hundreds != 0)
            {
                groupText += _units[hundreds] + " Hundred";

                if (tensUnits != 0)
                {
                    groupText += " and ";
                }
            }
            int tens = tensUnits / 10;
            int units = tensUnits % 10;

            // Tens rules
            if (tens >= 2)
            {
                groupText += _tens[tens];
                if (units != 0)
                {
                    groupText += " " + _units[units];
                }
            }
            else if (tensUnits != 0)
                groupText += _units[tensUnits];

            return groupText;
            
            
        }
        public string[] FormatValue(string val)
        {
            int idx = 0;
            string group = "";
            int unit = 0; ;
            string[] res = new string[1000];
            for (int i =val.Length - 1; i >= 0; i--)
            {
                idx++;
                group = val[i]+group;
                if (idx == 3 || i<2)
                {
                    res[unit++]= group;
                    idx = 0;
                    group = "";
                }
                

            }
            return res;
        }
        
        public string Translate(string value)
        {

            if (value.StartsWith("-"))
            {
                return "Minus " + Translate(value.Substring(1));
            }
            
            string val = value.TrimStart(new char[] { '0' });
            double x = Convert.ToDouble(val);
            if (val == "") return "Zero";
            
            
            string[] digitGroups = FormatValue(val);
            int n= digitGroups.Length;
            string[] groupTexts = new string[n];
            for (int i = 0; i < n; i++)
            {
                int number = Convert.ToInt32(digitGroups[i]);
                groupTexts[i] = TranslateGroup(number);
            }
            
            



            string combined = groupTexts[0];
            bool appendAnd = (Convert.ToInt32(digitGroups[0]) > 0) && (Convert.ToInt32(digitGroups[0]) < 100);





            for (int i = 1; i < n; i++)
            {
                // Only add non-zero items
                if (Convert.ToInt32(digitGroups[i]) != 0)
                {
                    // Build the string to add as a prefix
                    string prefix = groupTexts[i] + " " + _uppers[i];

                    if (combined.Length != 0)
                    {
                        prefix += appendAnd ? " and " : " ";
                    }

                    // Opportunity to add 'and' is ended
                    appendAnd = false;
                    combined = prefix + combined;
                }
            }
            return combined;
        }
    }

}


