using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using Sanet.Kniffel.Utils;

namespace Sanet.Kniffel.Localization
{
    
    public class ResourceModel : IResourceModel
    {
        Dictionary<string,XDocument> _strings;
        string[] _supportedLocales;
        int _loadedLocales = 0;

        public ResourceModel(string[] supportedLocales)
        {
            _strings = new Dictionary<string, XDocument>();
            _supportedLocales = supportedLocales;
            foreach (var locale in _supportedLocales) 
            {
                _strings.Add(locale, XMLLoader.LoadDocument(string.Format("Strings/{0}/Resources.resw", locale)));
            }
        }
                
        /// <summary>
        /// Returns string localization for current language
        /// </summary>
        public string GetString(string resource)
        {
            if (_strings.Count == 0)
                return resource;
            try
            {
                var resDoc = _strings[CurrentLanguage];
                var node = resDoc.Descendants("data").FirstOrDefault(f=>f.Attribute("name").Value==resource);
                return node.Element("value").Value;
            }
            catch (Exception)
            {
                return resource;
            }

        }

        /// <summary>
        /// Return string localization for special language
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public string GetString(string resource, string languageCode)
        {
            if (_strings.Count == 0 || !_strings.ContainsKey(languageCode))
                return resource;
            try
            {
                var resDoc = _strings[languageCode];
                var node = resDoc.Descendants("data").FirstOrDefault(f=>f.Attribute("name").Value==resource);
                return node.Element("value").Value;
            }
            catch (Exception)
            {
                return resource;
            }
        }

        public string CurrentLanguage
        {
            get
            {
				var lang = UiCulture.TwoLetterISOLanguageName;
				if (!_supportedLocales.Contains (lang)) 
				{
					lang = _supportedLocales [0];
					UiCulture =  new CultureInfo(lang);
				}
				return lang;
            }
            set
            {
                UiCulture =  new CultureInfo(value);
            }
        }

        CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
        CultureInfo UiCulture
        {
            get
            {
                return uiCulture;
            }
            set
            {
                string lang = value.Name.Split('-')[0];
                CultureInfo culture = new CultureInfo(lang);

                uiCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }
    }
}
