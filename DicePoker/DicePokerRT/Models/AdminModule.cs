using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public static class AdminModule
    {
        public static bool IsAdmin(Player player)
        {
            if ((player.Name == "LW" || player.Name == "ju_pi") && player.Password == "290159")
                return true;
            return false;
        }
    }
}
