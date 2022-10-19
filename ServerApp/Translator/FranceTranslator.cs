using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class FranceTranslator : ITranslator
    {
        string[] _units = new[] { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
        string[] _tens = new[] { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "quatre-vingt" };
        string[] _uppers = { "", "mille", "million", "milliard", "billion", "billiard", "trillion",
        "trilliard", "quadrillion", "quadrilliard", "quintillion", "quintilliard", "sextillion", "sextilliard", "septillion",
        "septilliard", "octillion", "octilliard", "nonillion", "nonilliard", "décillion", "décilliard", "undecillion", "undecilliard",
        "duodecillion", "duodecilliard", "tredecillion", "tredecilliard"};



        public string TranslateGroup(int threeDigits)
        {
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = threeDigits / 100;
            int tensUnits = threeDigits % 100;

            // Hundreds rules
            if (hundreds != 0)
            {
                if (hundreds == 1)
                    groupText += "cent";
                else if (hundreds != 1 && tensUnits == 0)
                    groupText += _units[hundreds] + " cents";
                else
                    groupText += _units[hundreds] + " cent";

                if (tensUnits != 0)
                    groupText += " ";
            }
            int tens = tensUnits / 10;
            int units = tensUnits % 10;

            // Tens rules
            if (tens >= 2)
            {
                switch (tens)
                {
                    case 7:
                        groupText += _tens[tens - 1];
                        if (units != 0)
                        {
                            if (units == 1)
                                groupText += " et " + _units[units + 10];
                            else
                                groupText += "-" + _units[units + 10];
                        }
                        else
                            groupText += "-" + _units[units + 10];
                        break;
                    case 8:
                        groupText += _tens[tens - 1];
                        if (units != 0)
                        {
                            groupText += "-" + _units[units];
                        }
                        else
                            groupText += "s";
                        break;
                    case 9:
                        groupText += _tens[tens - 2];
                        groupText += "-" + _units[units + 10];
                        break;

                    default:
                        groupText += _tens[tens];
                        if (units != 0)
                        {
                            if (units == 1)
                                groupText += " et " + _units[units];
                            else
                                groupText += "-" + _units[units];
                        }
                        break;
                }

            }

            else if (tensUnits != 0)
                groupText += _units[tensUnits];

            return groupText;


        }

        public int lengthgroup(string val)
        {
            int idx = 0;
            int unit = 0; ;
            for (int i = val.Length - 1; i >= 0; i--)
            {
                idx++;
                if (idx == 3 || i < 1)
                {
                    unit++;
                    idx = 0;
                }


            }
            return unit;
        }
        public string[] FormatValue(string val)
        {
            
            int idx = 0;
            string group = "";
            int unit = 0; ;
            string[] res = new string[lengthgroup(val)];
            for (int i = val.Length - 1; i >= 0; i--)
            {
                idx++;
                group = val[i] + group;
                if (idx == 3 || i < 1)
                {
                    res[unit++] = group;
                    idx = 0;
                    group = "";
                }


            }
            return res;
        }

        public string Translate(string value)
        {
            string val = value.TrimStart(new char[] { '0' });

            if (val == "") return "zéro";

            if (value.StartsWith("-"))
            {
                if (val.Substring(1).TrimStart(new char[] { '0' }) == "") return "zéro";
                return "Moins " + Translate(value.Substring(1));
            }



            string[] digitGroups = FormatValue(val);
            int n = digitGroups.Length;
            string[] groupTexts = new string[n];
            for (int i = 0; i < n; i++)
            {
                int number = Convert.ToInt32(digitGroups[i]);
                groupTexts[i] = TranslateGroup(number);
            }
            if(n==2 && groupTexts[1] == "un")
            {
                groupTexts[1] = "";
            }





            string combined = groupTexts[0];
            bool appendAnd = (Convert.ToInt32(digitGroups[0]) > 0) && (Convert.ToInt32(digitGroups[0]) < 100);



            int dem = 3;
            string prefix = "";

            for (int i = 1; i < n; i++)
            {
                // Only add non-zero items
                if (Convert.ToInt32(digitGroups[i]) != 0)
                {
                    // Build the string to add as a prefix
                    dem += digitGroups[i].Length;
                    if (dem < 7 || (dem-1) % 3 == 0)
                    { 
                        prefix = groupTexts[i] + " " + _uppers[i];
                    }
                    else
                    {
                        prefix = groupTexts[i] + " " + _uppers[i] +"s";
                    }

                    if (combined.Length != 0)
                    {
                        prefix += appendAnd ? " " : " ";
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
