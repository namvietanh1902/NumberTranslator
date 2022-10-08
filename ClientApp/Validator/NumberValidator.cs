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
            var regex = new Regex("^-?\\d*(\\\\d+)?$");
            return regex.IsMatch(number);
        }
    }
}
