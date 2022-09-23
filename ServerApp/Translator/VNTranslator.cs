using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class VNTranslator : ITranslator
    {
        string[] units = {};
        string[] uppers = {"","Mươi","Trăm","Nghìn","Triệu","Tỷ" };

        public string Translate(string str)
        {
            string result ="";
            if (str.StartsWith("-"))
            {
                return "Âm "+Translate(str.Substring(1));
            }
            if (str.Contains("."))
            {
                return Translate(str.Split('.')[0]) +" Phẩy "+Translate(str.Split('.')[1].TrimEnd(new char[] {'0'}));
            }
            
            str = str.TrimStart(new char[] { '0' });
            if (str == "") return "Không";
            long number = Convert.ToInt64(str);
            if ((number / 1000000000) > 0)
            {
                result += Translate((number / 1000000000).ToString()) + " Tỷ ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                result += Translate((number / 1000000).ToString()) + " Triệu ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                result += Translate((number / 1000).ToString()) + " Nghìn ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                result += Translate((number / 100).ToString()) + " Trăm ";
                number %= 100;
            }

            if (number > 0)
            {
                if (result != "")
                    result += " Và ";

                var unitsMap = new[] { "Không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười Một", "Mười Hai", "Mười Ba", "Mười Bốn", "Mười Lăm", "Mười Sáu", "Mười Bảy", "Mười Tám", "Mười Chín" };
                var tensMap = new[] { "Không", "Mười", "Hai Mươi", "Ba Mươi", "Bốn mươi", "Năm Mươi", "Sáu Mươi", "Bảy Mươi", "Tám Mươi", "Chín Mươi" };

                if (number < 20)
                    result += unitsMap[number];
                else
                {
                    result += tensMap[number / 10];
                    if ((number % 10) > 0)
                        result += " Lẻ " + unitsMap[number % 10];
                } 
            }

            return result;
            
        }
    }
}
