using System;
using System.IO.Ports;

namespace cnc
{
    class Taladro
    {
        byte mask;

        public Taladro()
        {
            mask = 63;
        }

        public void Stop()
        {
            Main.data &= mask;
            Main.Refresh();
        }
        public void StartClockWise()
        {
            Main.data &= mask;
            Main.data |= 128;
            Main.Refresh();
        }
        public void StartCounterClockWise()
        {
            Main.data &= mask;
            Main.data |= 64;
            Main.Refresh();
        }
    }

    
}
