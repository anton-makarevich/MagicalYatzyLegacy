using System;
using Sanet.Kniffel.XF.Services;
using Xamarin.Forms;
using Sanet.Kniffel.XF.Droid.Services;
using System.IO;

#if ROLLER
using DiceRollerXF.Droid;
#else
using MagicalYatzyXF.Droid;
#endif

using System.Threading.Tasks;

[assembly: Dependency(typeof(ImageService))]
namespace Sanet.Kniffel.XF.Droid.Services
{
    class ImageService : IImageService
    {
        public NGraphics.IPlatform GetPlatform()
        {
            return new NGraphics.AndroidPlatform();
        }

        public async Task<Stream> GetFileStream(string name)
        {

            using (var stream = MainActivity.Instance.Assets.Open(name))
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                return ms;
            }
        }
    }
}