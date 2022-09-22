using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Translator
{
    public interface ITranslator
    {
        void Translate(string str);
    }
}
