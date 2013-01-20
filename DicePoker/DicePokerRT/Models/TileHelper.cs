
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace Sanet.Kniffel.Models
{
    /// <summary>
    /// Class to work with app tiles
    /// </summary>
    public static class TileHelper
    {
        /// <summary>
        /// Method to update the secondary tile content with given text
        /// </summary>
        /// <param name="tileID"></param>
        /// <param name="tileText"></param>
        public static void UpdateTileContent(string tileID, string tileText)
        {
            // Get a XML DOM version of a specific template by using the getTemplateContent
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText04);

            XmlNodeList tileTextElements = tileXml.GetElementsByTagName("text");
            tileTextElements.Item(0).AppendChild(tileXml.CreateTextNode(tileText));

            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText04);
            XmlNodeList squareTileTextElements = squareTileXml.GetElementsByTagName("text");
            squareTileTextElements.Item(0).AppendChild(squareTileXml.CreateTextNode(tileText));

            // Include the square template in the notification
            IXmlNode subNode = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(subNode);

            TileNotification tileNotification = new TileNotification(tileXml);
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID);
            updater.Update(tileNotification);
        }

        /// <summary>
        /// Method to update the secondary tile (or "main") content with given (template with block)
        /// </summary>
        public static void UpdateTileContent(string tileID, string tileText1, string tileText2,
            string tileText3, string tileText4, string tileText5)
        {
            
                // Get a XML DOM version of a specific template by using the getTemplateContent
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageAndText02);

                XmlNodeList tileTextElements = tileXml.GetElementsByTagName("text");
                tileTextElements.Item(0).AppendChild(tileXml.CreateTextNode(tileText1));
                tileTextElements.Item(1).AppendChild(tileXml.CreateTextNode(tileText2));
                tileTextElements.Item(2).AppendChild(tileXml.CreateTextNode(tileText3));
                tileTextElements.Item(3).AppendChild(tileXml.CreateTextNode(tileText4));
                tileTextElements.Item(4).AppendChild(tileXml.CreateTextNode(tileText5));

                XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
                ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/WideLogo.scale-100.png");
                ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "Dice Poker");

                XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText03);
                XmlNodeList squareTileTextElements = squareTileXml.GetElementsByTagName("text");
                squareTileTextElements.Item(0).AppendChild(squareTileXml.CreateTextNode(tileText2));
                squareTileTextElements.Item(1).AppendChild(squareTileXml.CreateTextNode(tileText3));
                squareTileTextElements.Item(2).AppendChild(squareTileXml.CreateTextNode(tileText4));
                squareTileTextElements.Item(3).AppendChild(squareTileXml.CreateTextNode(tileText5));

                XmlNodeList squareTileImageAttributes = squareTileXml.GetElementsByTagName("image");
                ((XmlElement)squareTileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/Logo.scale-100.png");
                ((XmlElement)squareTileImageAttributes[0]).SetAttribute("alt", "Dice Poker");


                // Include the square template in the notification
                IXmlNode subNode = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
                tileXml.GetElementsByTagName("visual").Item(0).AppendChild(subNode);


                TileNotification tileNotification = new TileNotification(tileXml);
                TileUpdater updater;
                if (tileID.ToLower() == "main")
                    updater = TileUpdateManager.CreateTileUpdaterForApplication();
                else
                    updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID);
                updater.Update(tileNotification);
            
        }

        /// <summary>
        /// Method to update the secondary tile content with given (template with 4 rows)
        /// </summary>
        public static void UpdateTileContent(string tileID, string tileText1, string tileText2,
            string tileText3, string tileText4)
        {

            // Get a XML DOM version of a specific template by using the getTemplateContent
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageAndText02);

            XmlNodeList tileImageElements = tileXml.GetElementsByTagName("image");
            var imageSourceNode=tileImageElements.Item(0).Attributes.Where(f => f.NodeName == "src").FirstOrDefault();
            imageSourceNode.NodeValue = "ms-appx:///Assets/LogoSquare.png";
            XmlNodeList tileTextElements = tileXml.GetElementsByTagName("text");
            tileTextElements.Item(0).AppendChild(tileXml.CreateTextNode(tileText1));
            tileTextElements.Item(1).AppendChild(tileXml.CreateTextNode(tileText2));
            tileTextElements.Item(2).AppendChild(tileXml.CreateTextNode(tileText3));
            tileTextElements.Item(3).AppendChild(tileXml.CreateTextNode(tileText4));
            
            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText03);
            XmlNodeList tileWideImageElements = tileXml.GetElementsByTagName("image");
            var imageSourceNodeWide = tileWideImageElements.Item(0).Attributes.Where(f => f.NodeName == "src").FirstOrDefault();
            imageSourceNodeWide.NodeValue = "ms-appx:///Assets/LogoWide.png";
                        
            XmlNodeList squareTileTextElements = squareTileXml.GetElementsByTagName("text");
            squareTileTextElements.Item(0).AppendChild(squareTileXml.CreateTextNode(tileText1));

            squareTileTextElements.Item(1).AppendChild(squareTileXml.CreateTextNode(tileText2));
            squareTileTextElements.Item(2).AppendChild(squareTileXml.CreateTextNode(tileText3));
            squareTileTextElements.Item(3).AppendChild(squareTileXml.CreateTextNode(tileText4));

            // Include the square template in the notification
            IXmlNode subNode = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(subNode);

            TileNotification tileNotification = new TileNotification(tileXml);
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID);
            updater.Update(tileNotification);

        }

        ///// <summary>
        ///// Trying to update tile if it exists
        ///// </summary>
        ///// <param name="selectedPlan"></param>
        ///// <returns></returns>
        //public static void UpdatePlanTile(BiblePlan selectedPlan )
        //{ 
        //    string tileId ="SecondaryPlanTile" + selectedPlan.PlanDBName;
        //    if (selectedPlan.IsPinned)
        //    {
                
        //        UpdateTileContent(selectedPlan.TileId, selectedPlan.PlanName, "Last read: " + selectedPlan.LastReadLabel, selectedPlan.LastPassageLabel, selectedPlan.ProgressLabel);
        //    }
        //}

        public static void UpdateMainTile(string title, string text)
        {
            text = title + " " + text;

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImage04);

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");

            XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/LogoWide.png");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "Bible Pronto");


            int index = 1;

            foreach (IXmlNode tileTextAttr in tileTextAttributes)
            {
                tileTextAttr.InnerText = text;
                index++;
            }

            TileNotification tileNotification = new TileNotification(tileXml);
            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddHours(1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

        }

        public static bool IsSecondaryTileExists(string tileid)
        {
            return Windows.UI.StartScreen.SecondaryTile.Exists(tileid);
        }
        ///// <summary>
        ///// Returns if secondary tile for this verse exists
        ///// </summary>
        ///// <param name="selectedVerse"></param>
        ///// <returns></returns>
        //public static bool IsVerseTileExist(Verse selectedVerse)
        //{
        //    return IsSecondaryTileExists(selectedVerse.TileId);
        //}
    }
}
