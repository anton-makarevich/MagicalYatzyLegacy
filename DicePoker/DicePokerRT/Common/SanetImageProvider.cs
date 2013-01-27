using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public class SanetImageProvider
    {
        public static Uri GetAssetsImage(string imagename)
        {
            return new Uri("ms-appx:///Assets/"+imagename, UriKind.Absolute);
        }
    }
}
