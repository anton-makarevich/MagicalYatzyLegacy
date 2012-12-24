using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Sanet
{
  public  class Utilities
    {
      
      public static string TrimText(string text,int nos)
      {
          return text.Length>nos? ( text.Substring(0,nos)+"...") :text;
      }
      public static string TrimText(string text)
      {
          return TrimText(text, 110);
      }
      public static string TrimTextSnp(string text)
      {
          return TrimText(text, 70);
      }

      public static async void ShowMessage(string msgString,string title)
      {
          try
          {

              MessageDialog msg = new MessageDialog(msgString, title);
              msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler((s) => { })));
              IUICommand x = await msg.ShowAsync();
          }
          catch (UnauthorizedAccessException ex)
          {
              //ExceptionLogger.LogException(ex);
          }

      }
      public static void ShowMessage(string msgString)
      {
          ShowMessage(msgString, "Sanet Systems");
      }

      public static async Task<int> ShowMessageAsync(string msgString, string title)
      {
          MessageDialog msg = new MessageDialog(msgString, title);
          msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler((s) => { })));
          IUICommand x = await msg.ShowAsync();

          return 1;
      }

      

     public  async static Task<bool> DoesFileExistAsync(string fileName)
      {
          try
          {
              StorageFile  file=    await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
              BasicProperties properties = await file.GetBasicPropertiesAsync();
              if (properties.Size > 0)
              {
                  return true;
              }
              else
                  return false;

          }
          catch
          {
              return false;
          }
      }

     public static void ShowToastNotification(string text)
     {
         // The template is set to be a ToastImageAndText01. This tells the toast notification manager what to expect next.
         ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
         XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

         XmlNodeList toastImageElements = toastXml.GetElementsByTagName("image");
        ((XmlElement)toastImageElements[0]).SetAttribute("src", "ms-appx:///Assets/LogoSquare.png");
        ((XmlElement)toastImageElements[0]).SetAttribute("alt", "Bible Pronto");
         
         XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
         toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

         ToastNotification toast = new ToastNotification(toastXml);
         ToastNotificationManager.CreateToastNotifier().Show(toast);

     }

     
     ///// <summary>
     // /// Method to check if file exists
     // /// </summary>
     // /// <returns></returns>
     public async static Task<bool> FileExistAsync(string path, string fileName, bool checkdate = false)
     {
         try
         {
             var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
             StorageFile assetFile = null;
             BasicProperties assetProperties;

             if (file == null)
                 return false;//faster then exception
             BasicProperties basicProperties = await file.GetBasicPropertiesAsync();

             if (checkdate)
             {
                 assetFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(path);
                 assetProperties = await assetFile.GetBasicPropertiesAsync();
                 if (basicProperties.Size > 0 && ( basicProperties.DateModified > assetProperties.DateModified))
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             }
             else
             {
                 if (basicProperties.Size > 0 )
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             }
             
         }
         catch
         {
             return false;
         }
     }

    }
}
