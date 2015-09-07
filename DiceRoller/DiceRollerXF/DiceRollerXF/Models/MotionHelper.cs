using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceRollerXF.Models
{
    public static class MotionHelper
    {
        public static event Action DeviceShaked;

        public static void ShakeNotify()
        {
            if (DeviceShaked != null)
                DeviceShaked();
        }
                
    }
}
