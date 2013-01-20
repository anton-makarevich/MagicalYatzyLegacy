using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Resources;
using System.Windows.Data;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace Sanet.DiceRoller.Models
{
    public class ApplicationResources : IValueConverter
    {
        /// <summary>
        /// The Resource Manager loads the resources out of the executing assembly (and the XAP File where there are embedded)
        /// </summary>
        private static readonly ResourceManager resourceManager = new ResourceManager("Sanet.DiceRoller.strings.strings", Assembly.GetExecutingAssembly());
        /// <summary>
        /// Use this property to specify the culture
        /// </summary>
        private static CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
        public static CultureInfo UiCulture
        {
            get { return uiCulture; }
            set { uiCulture = value;}
        }

        /// <summary>
        /// This method returns the localized string of the given resource.
        /// </summary>
        public string Get(string resource)
        {
            return resourceManager.GetString(resource, UiCulture);
        }

        #region IValueConverter Members

        /// <summary>
        /// This method is used to bind the silverlight component to the resource. 
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reader = (ApplicationResources)value;
            return reader.Get((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack is not used, because the Localization is only a One Way binding
            throw new NotImplementedException();
        }

        #endregion
    }
    public class ResourceModel
    {
        public ResourceModel()
        {
            ApplicationResources.UiCulture = CultureInfo.CurrentUICulture;//new CultureInfo("ru-RU");
        }

        public string GetString(string resource)
        {
            try
            {

                return App.ResourceProvider.Get(resource);

            }
            catch (Exception)
            {
                return resource;
            }

        }
    }
}
