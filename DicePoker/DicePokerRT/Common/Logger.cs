using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Sanet
{
   public class RTLogger:ILogConsole
    {
       object syncRoot = new object();
       public void WriteLine(string line)
       {
           PurgeLogFiles();
           lock (syncRoot)
           {
               LogFileWrite(line + "\r\n");
           }
       }
       public void Write(string line)
       {
           LogFileWrite(line);
       }
       /// <summary>
       /// This method is for writing the Log file with the current exception message
       /// </summary>
       /// <param name="exceptionMessage"></param>
       private static async void LogFileWrite(string exceptionMessage)
       {
           try
           {
               string fileName = DateTime.Today.ToString("yyyyMMdd") + ".log";
               var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
               var logFolder = await localFolder.CreateFolderAsync("Logs", Windows.Storage.CreationCollisionOption.OpenIfExists);
               var logFile = await logFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

               if (!String.IsNullOrEmpty(exceptionMessage))
               {
                   await FileIO.AppendTextAsync(logFile, exceptionMessage);                   
               } 
           }
           catch
           {
           } 

       }

       /// <summary>
       /// This method is for prepare the error message to log using Exception object
       /// </summary>
       /// <param name="currentException"></param>
       /// <returns></returns>
       private static String CreateErrorMessage(Exception currentException)
       {
           StringBuilder messageBuilder = new StringBuilder();
           try
           {
               messageBuilder.AppendLine("-------------------------------------------------------------------------------------------------------------------------");
               messageBuilder.AppendLine("Source: " + currentException.Source.ToString().Trim());
               messageBuilder.AppendLine("Date Time: " + DateTime.Now);
               messageBuilder.AppendLine("-------------------------------------------------------------------------------------------------------------------------");
               messageBuilder.AppendLine("Method: " + currentException.Message.ToString().Trim());
               messageBuilder.AppendLine("Exception :: " + currentException.ToString());
               if (currentException.InnerException != null)
               {
                   messageBuilder.AppendLine("InnerException :: " + currentException.InnerException.ToString());
               }
               messageBuilder.AppendLine("");
               return messageBuilder.ToString();
           }
           catch
           {
               messageBuilder.AppendLine("Exception:: Unknown Exception while writing log");
               return messageBuilder.ToString();
           } 
       }


       /// <summary> 
       /// This method purge old log files in the log folder, which are older than daysToKeepLog. 
       /// <param name=""></param>
       /// <returns></returns> 
       /// </summary> 
       public static async void PurgeLogFiles()
       {
           int daysToKeepLog;
           DateTime todaysDate;
           var logFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

           try
           {
               daysToKeepLog = 5;
               todaysDate = DateTime.Now.Date;

               logFolder = await logFolder.GetFolderAsync("Logs");
               IReadOnlyList<StorageFile> files = await logFolder.GetFilesAsync();

               foreach (StorageFile file in files)
               {
                   BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
                   if (file.FileType == ".log")
                   {
                       if (DateTime.Compare(todaysDate, basicProperties.DateModified.AddDays(daysToKeepLog).DateTime.Date) >= 0)
                       {
                           await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                       }
                   }
               }
           }
           catch
           {

           }
       } 

    }
}
