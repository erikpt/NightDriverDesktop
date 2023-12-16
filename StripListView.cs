using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NightDriver
{
    internal class StripListView : ListView
    {
        public StripListView() 
        {
            // Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Enable notify messages so we get a shot at WM_ERASEBKGND
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            // Eat the WM_ERASEBKGND message for cleaner painting with less flicker, like Task Manager :-)

            if (m.Msg != 0x14)
                base.OnNotifyMessage(m);
        }
    }
}