//This class is based on Igor Ralic's class but heavily modified to use with RT and BiblePronto settings model

//   Copyright 2011 Igor Ralic

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//


using System.Windows;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.System;
using Sanet.Kniffel.Models;
using System;


namespace Sanet.Models
{
    public static class ReviewBugger
    {
        private const int numOfRunsBeforeFeedback = 7;
        /// <summary>
        /// Increase number of runs if needed
        /// </summary>
        public static void CheckNumOfRuns()
        {
            var numOfRuns=RoamingSettings.NumOfRuns;
            if (numOfRuns == -1)
                return;
            RoamingSettings.NumOfRuns = numOfRuns+1;
        }

        public static void DidReview()
        {
            RoamingSettings.NumOfRuns = -1;
        }

        public static bool IsTimeForReview()
        {
            return RoamingSettings.NumOfRuns> numOfRunsBeforeFeedback  ? true : false;
        }

        public static async Task PromptUser()
        {
            MessageDialog msg = new MessageDialog("ReviewReminder".Localize(), "ReviewReminderHeader".Localize());

            // Add buttons and set their command handlers
            msg.Commands.Add(new UICommand("YesLabel".Localize(), new UICommandInvokedHandler(CommandInvokedHandler)));
            msg.Commands.Add(new UICommand("LaterLabel".Localize(), new UICommandInvokedHandler(CommandInvokedHandler)));
            msg.Commands.Add(new UICommand("NeverLabel".Localize(), new UICommandInvokedHandler(CommandInvokedHandler)));

            // Show the message dialog
            await msg.ShowAsync();
                      
        }

        /// <summary>
        /// Callback function for the invocation of the dialog commands
        /// </summary>
        /// <param name="command">The command that was invoked</param>
        static async void CommandInvokedHandler(IUICommand command)
        {
            string buttonLabel = command.Label;
            await Window.Current.Dispatcher.RunAsync(global::Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                if (command.Label == "YesLabel".Localize())
                {
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=43862AntonMakarevich.SanetDicePoker_2wtrjzrdj31kc"));
                    
                    DidReview();
                }
                if (command.Label == "LaterLabel".Localize())
                {
                    RoamingSettings.NumOfRuns = 0;
                }
                if (command.Label == "NeverLabel".Localize())
                {
                    DidReview();
                }
            });
        }

    }
}
