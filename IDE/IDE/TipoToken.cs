using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    class TipoToken
    {
        public enum Tipo {
            ID = 1,
            KEYWORD = 12,
            ARTH_OP = 11, // & 2
            REL_OP = 10, // & 5 & 9
            STRING = 13,
            INT = 4,
            REAL = 8, // & 3
            COMMENT = 14
        }
    }
}
