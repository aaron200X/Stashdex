using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Helpfunctions {
    public static class HelpFunctions {
       public static Regex getNumber1Regex = new Regex(@"\d{1,10}");
    }
}
