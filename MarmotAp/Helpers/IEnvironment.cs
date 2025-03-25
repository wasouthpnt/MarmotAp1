using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MarmotAp.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint);
    }
}
