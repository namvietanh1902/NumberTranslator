using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public class VNTranslator : ITranslator
    {
        string[] units = {"","một","hai","ba","bốn","năm","sáu","bảy","tám","chín"};
        string[] tens = {"mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
        string[] uppers = { };

        public string Translate(string str)
        {
            string result ="";
            if (str.StartsWith("-"))
            {
                result.Concat("Âm");
            }
            
            str = str.TrimStart(new char[] { '0' });
            if (str == "") return "Không";
            long number = Convert.ToInt64(str);
            if (number < 0) number *= -1;


            return result;
        }
    }
}
