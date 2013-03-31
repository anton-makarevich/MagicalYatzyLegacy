using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public static class InstanceKeeper
    {
        public static Player GetNewPlayer()
        {
            return new Player();
        }
    }
}
