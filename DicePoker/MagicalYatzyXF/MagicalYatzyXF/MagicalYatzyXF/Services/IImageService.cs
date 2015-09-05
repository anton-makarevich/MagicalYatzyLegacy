using NGraphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.XF.Services
{
    public interface IImageService
    {
        IPlatform GetPlatform();
        Task<Stream> GetFileStream(string name);
    }
}
