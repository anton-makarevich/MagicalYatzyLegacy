using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public static class LocalizerExtensions
    {
        public static ResourceModel RModel;
        public static string Localize(this string value)
        {
            return RModel.GetString(value);

        }
    }
}
