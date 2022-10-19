using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class VNTranslator : ITranslator
    {
        string[] _units = new[] { "Không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười Một", "Mười Hai", "Mười Ba", "Mười Bốn", "Mười Lăm", "Mười Sáu", "Mười Bảy", "Mười Tám", "Mười Chín" };
        string[] _tens = new[] { "Không", "Mười", "Hai Mươi", "Ba Mươi", "Bốn Mươi", "Năm Mươi", "Sáu Mươi", "Bảy Mươi", "Tám Mươi", "Chín Mươi" };
        string[] _uppers = new[] { "", "Nghìn", "Triệu", "Tỷ", "Nghìn Tỷ", "Triệu Tỷ", "Tỷ Tỷ" , "Nghìn Tỷ Tỷ", "Triệu Tỷ Tỷ", "Tỷ Tỷ Tỷ" };



        public string TranslateGroup(int threeDigits)
        {
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = threeDigits / 100;
            int tensUnits = threeDigits % 100;

            // Hundreds rules
            if (hundreds != 0)
            {
                groupText += _units[hundreds] + " Trăm";

                if (tensUnits != 0)
                {
                    groupText += " ";
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
                    switch (units)
                    {
                        case 1:
                            {
                                groupText += " " + "Mốt";
                                break;
                            }
                        case 5:
                            {
                                groupText += " " + "Lăm";
                                break;
                            }
                        default:
                            {
                                groupText += " " + _units[units];
                                break;

                            }

                    }
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
            if (val == "") return "Không";
            if (value.StartsWith("-"))
            {
                if (val.Substring(1).TrimStart(new char[] { '0' }) == "") return "Không";
                return "Âm " + Translate(value.Substring(1));
            }



            string[] digitGroups = FormatValue(val);
            int n = digitGroups.Length;
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
                        prefix += appendAnd ? " Lẻ " : " ";
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

