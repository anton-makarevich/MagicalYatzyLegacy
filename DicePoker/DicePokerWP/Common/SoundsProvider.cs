using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Sanet.Models
{
    //will store precached sounds here to improve performance
    public static class SoundsProvider
    {
        static Dictionary<string, Stream> audioStreams;

        static MediaElement _globalPlayer = new MediaElement();
        //    static StorageFolder dataFolder;

        /// <summary>
        /// Loads files from the disk
        /// </summary>
            public static void Init()
            {
                ////dataFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Audio");
                //audioStreams = new Dictionary<string, Stream>();
                //string[] files = new string[]
                //{
                //    "click",
                //    "dice1",
                //    "dice2",
                //    "dice3",
                //    "fanfare",
                //    "magic",
                //    "win",
                //    "wrong"

                //};
                //foreach (string file in files)
                //{
                //    var resrouceStream = Application.GetResourceStream(new Uri("Audio/" + file + ".wav", UriKind.Relative));
                //    audioStreams.Add(file,resrouceStream.Stream);
                //}

        }
            
        /// <summary>
        /// play sound usng provided mediaelement
        /// </summary>
        public static void PlaySound(MediaElement player, string filename)
        {
            try
            {
                if (!RoamingSettings.IsSoundEnabled)
                    return;
                filename = filename.ToLower();
                 var soundfile = "Audio/" + filename + ".wav"; //Note no slash before the Assets folder, and it's a WAV file!
                Stream astream = TitleContainer.OpenStream(soundfile);
                SoundEffect effect = SoundEffect.FromStream(astream);
                FrameworkDispatcher.Update();
                effect.Play();
                
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

        /// <summary>
        /// Method to copy file from app resources to isolated storage
        /// </summary>
        private static void CopyFile(string path, string fileName)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(fileName))
                    {
                        return;
                        //myIsolatedStorage.DeleteFile(fileName);
                    }
                    Stream resourceStream = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
                    using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, myIsolatedStorage))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fileStream))
                        {
                            using (BinaryReader reader = new BinaryReader(resourceStream))
                            {

                                long length = resourceStream.Length;
                                byte[] buffer = new byte[32];
                                int readCount = 0;
                                // read file in chunks in order to reduce memory consumption and increase performance
                                while (readCount < length)
                                {
                                    int actual = reader.Read(buffer, 0, buffer.Length);
                                    readCount += actual;
                                    writer.Write(buffer, 0, actual);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
