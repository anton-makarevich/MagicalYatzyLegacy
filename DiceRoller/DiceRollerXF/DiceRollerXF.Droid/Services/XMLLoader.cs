#if ANDROID
using DiceRollerXF.Droid;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sanet.Kniffel.Utils
{
    public static class XMLLoader
    {
        public static XDocument LoadDocument(string path)
        {
#if ANDROID
            return XDocument.Load(MainActivity.Instance.Assets.Open(path));
#else
            return XDocument.Load(path);
#endif
        }
    }
}
