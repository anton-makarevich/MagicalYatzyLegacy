﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Sanet.Kniffel.Models
{
    public static class LocalSettings
    {
        static XDocument xmlProgress;

        static StorageFolder storage = Windows.Storage.ApplicationData.Current.LocalFolder;

        private static  Dictionary<string, string> values { get; set; }
        
        public static  string GetValue(string key)
        {
            lock (lockObj)
            {
                if (values.ContainsKey(key))
                    return values[key];
                return null;
            }
        }
        public static  void SetValue(string key, object value)
        {
            lock (lockObj)
            {
                if (values.ContainsKey(key))
                    values[key] = value.ToString();
                else
                    values.Add(key, value.ToString());

                SaveProgress();
            }
            
        }

        public async static Task  InitLocalSettings()
        {
            await getStorageFile();
            
        }

        public static  bool ContainsKey(string key)
        {
            return values.ContainsKey(key);
        }


        static object lockObj = new object();

        async static  Task  getStorageFile()
        {
            StorageFile storageFile=null;
            values = new Dictionary<string, string>();
            try
            {
                storageFile = await storage.GetFileAsync("settings.xml");
            }
            catch { }
            if (storageFile != null)
            {
                var fas = await storageFile.OpenAsync(FileAccessMode.Read);
                using (Stream str = fas.AsStreamForRead())
                {
                    try
                    {
                        xmlProgress = XDocument.Load(str);
                    }
                    catch { }
                }
                    
                if (xmlProgress!=null)
                    foreach (XElement el in xmlProgress.Element("Settings").Elements())
                    {
                        SetValue(el.Name.LocalName, el.Value);
                    }
            }
            
            
        }

        public static void SaveProgress()
        {
            if (xmlProgress == null)
            {
                xmlProgress = new XDocument();
                xmlProgress.Add(new XElement("Settings"));
            }
            
            foreach (string key in values.Keys)
            {
                var pr = xmlProgress.Descendants(key).FirstOrDefault();
                var value=values[key];
                if (pr == null)
                {
                    pr = new XElement(key, value);
                    xmlProgress.Element("Settings").Add(pr);
                }
                else
                {
                    pr.Value = value;
                }
            }

            saveFile();
        }
        
        static async void saveFile()
        {
            try
            {
                var file = await storage.CreateFileAsync("settings.xml", CreationCollisionOption.ReplaceExisting);
                var writeStream = await file.OpenStreamForWriteAsync() as Stream;
                xmlProgress.Save(writeStream);
                writeStream.Flush();
            }
            catch { }
            
        }

 
        
    }
   
}
