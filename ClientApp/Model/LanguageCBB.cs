using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Model
{
    public class LanguageCBB
    {
        public string value { get; set; }
        public string description { get; set; }
        public override string ToString()
        {
            return description;
        }
    }
}
