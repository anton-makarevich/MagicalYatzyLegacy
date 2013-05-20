using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;


namespace Sanet.Models
{
    //will store precached sounds here to improve performance
    public static class SoundsProvider
    {
        static Dictionary<string, MediaStreamSource> audioStreams;

        static MediaElement _globalPlayer = new MediaElement();
        //    static StorageFolder dataFolder;

        /// <summary>
        /// Loads files from the disk
        /// </summary>
            public static void Init()
            {
                //dataFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Audio");
                audioStreams = new Dictionary<string, MediaStreamSource>();
                //var files = await dataFolder.GetFilesAsync();
                //foreach (var file in files)
                //{
                //    if (file.Name.ToLower().Contains(".mp3"))
                //    {
                //        var audiostream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //        audioStreams.Add(file.DisplayName.ToLower(), audiostream);
                //    }
                //}
            }
        /// <summary>
        /// play sound usng provided mediaelement
        /// </summary>
        public static void PlaySound(MediaElement player, string filename)
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
                        //dataFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Audio");
                        //var file = await dataFolder.GetFileAsync(filename + ".mp3");
                        //astream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        //audioStreams[filename] = astream;
                    }
                    player.SetSource(astream);
                }
            }
            catch (Exception ex)
            {
                LogManager.Log("PVM.PlaySound", ex);
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
