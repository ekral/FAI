using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prednaska3
{
    public class Model : IModel
    {
        public bool UlozAOverVysledek(double body)
        {
            // ulozi vysledek do databaze
            // nacte dochazku, to zda student je platny student

            // vrati true, pokud uspeje

            return body > 50 ? true : false;
        }
    }
}
