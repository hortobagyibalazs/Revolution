using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.HUD.Events
{
    public class ShowToastEvent
    {
        public string Message { get; set; }

        public ShowToastEvent(string msg)
        {
            Message = msg;
        }
    }
}
