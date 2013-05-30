using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace Sanet.Models
{
    //will store precached sounds here to improve performance
    public static class SoundsProvider
    {
        static Dictionary<string, IRandomAccessStream> audioStreams;

        static MediaElement _globalPlayer = new MediaElement();
            static StorageFolder dataFolder;
        /// <summary>
        /// Loads files from the disk
        /// </summary>
            public async static void Init()
            {
                dataFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Audio");
                audioStreams = new Dictionary<string, IRandomAccessStream>();
                var files = await dataFolder.GetFilesAsync();
                foreach (var file in files)
                {
                    if (file.Name.ToLower().Contains(".mp3"))
                    {
                        var audiostream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        audioStreams.Add(file.DisplayName.ToLower(), audiostream);
                    }
                }
            }
        /// <summary>
        /// play sound usng provided mediaelement
        /// </summary>
        public async static void PlaySound(MediaElement player, string filename)
        {
            try
            {
                if (audioStreams == null || player == null)
                    return;
                if (!RoamingSettings.IsSoundEnabled)
                    return;
                filename = filename.ToLower();
                if (audioStreams.ContainsKey(filename))
                {
                    player.Stop();
                    var astream = audioStreams[filename];
                    if (astream == null)
                    {
                        dataFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Audio");
                        var file = await dataFolder.GetFileAsync(filename + ".mp3");
                        astream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        audioStreams[filename] = astream;
                    }
                    player.SetSource(astream, "audio/mpeg");
                }
            }
            catch (Exception ex)
            {
                LogManager.Log("PVM.PlaySound", ex, MarkedUpExceptionType.Information);
            }
        }
        /// <summary>
        /// play sound usng default mediaelement
        /// </summary>
        public static void PlaySound(String filename)
        {
            PlaySound(_globalPlayer, filename);
        }
    }
}
