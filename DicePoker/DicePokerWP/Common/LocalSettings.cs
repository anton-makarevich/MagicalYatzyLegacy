using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Sanet.Kniffel.Models
{
    public static class LocalSettings
    {
        static XDocument xmlProgress;

        static IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

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

        public static void  InitLocalSettings()
        {
            getStorageFile();
            
        }

        public static  bool ContainsKey(string key)
        {
            return values.ContainsKey(key);
        }


        static object lockObj = new object();

        static void getStorageFile()
        {
            values = new Dictionary<string, string>();
            if (storage.FileExists("settings.xml"))
            {
                using (IsolatedStorageFileStream strmProgress = storage.OpenFile("settings.xml", FileMode.Open, FileAccess.Read))
                {
                    xmlProgress = XDocument.Load(strmProgress, LoadOptions.None);
                }
                
                if (xmlProgress != null)
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
        
        static void saveFile()
        {
            try
            {
                if (storage.FileExists("settings.xml"))
                {
                    storage.DeleteFile("settings.xml");
                }
                using (IsolatedStorageFileStream strmProgress = new IsolatedStorageFileStream("settings.xml", FileMode.Create, storage))
                {
                    xmlProgress.Save(strmProgress);
                    strmProgress.Close();
                }
            }
            catch { }
            
            
            
        }

 
        
    }
   
}
