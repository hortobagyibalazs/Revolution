using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Misc.Cheats
{
    public class AddCheatcodeEvent
    {
        public CheatCode Cheatcode { get; set; }

        public AddCheatcodeEvent(CheatCode code)
        {
            Cheatcode = code;
        }
    }
}
