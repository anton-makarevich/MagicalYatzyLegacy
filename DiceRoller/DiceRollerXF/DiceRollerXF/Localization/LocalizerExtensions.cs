
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sanet.Kniffel.Localization
{
    public static class LocalizerExtensions
    {
        static IResourceModel _RModel;
        
        public static void Initialize(IResourceModel model)
        {
            _RModel = model;
        }

        public static string CurrentLanguage
        {
            get
            {
                return _RModel.CurrentLanguage;
            }
            set
            {
                _RModel.CurrentLanguage = value;
                //AppSettings.CurrentLanguage = value;
            }
        }
        
        public static string Localize(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return _RModel.GetString(value);
            return "";

        }

        public static string Localize(this string value, string language)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value))
                return _RModel.GetString(value,language);
            return "";

        }
    }
}
