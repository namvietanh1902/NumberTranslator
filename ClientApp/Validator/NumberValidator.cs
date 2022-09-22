using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientApp.Validator
{
    public class NumberValidator
    {   
        public bool checkNumber(string number)
        {
            var regex = new Regex("^(?:-(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))|(?:0|(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))))(?:.\\d+|)$");
            return regex.IsMatch(number);
        }
    }
}
