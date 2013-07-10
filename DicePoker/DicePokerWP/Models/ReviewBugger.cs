//
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


using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Threading;
using Sanet.Kniffel.Models;
using Sanet.Models;

namespace System
{
    public static class ReviewBugger
    {
        private const int numOfRunsBeforeFeedback = 8;
        private static readonly Button yesButton = new Button() { Content = "YesLabel".Localize(), Width = 220, FontSize = 17 };
        private static readonly Button laterButton = new Button() { Content = "LaterLabel".Localize(), Width = 220, FontSize=16 };
        private static readonly Button neverButton = new Button() { Content = "NeverLabel".Localize(), Width = 120 };
        private static readonly MessagePrompt messagePrompt = new MessagePrompt();

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

        static DispatcherTimer promptTimer = new DispatcherTimer();

        public static void PromptUser()
        {
            promptTimer.Interval= TimeSpan.FromMilliseconds(500);
            promptTimer.Tick+=promptTimer_Tick;
            promptTimer.Start();
            
        }

static void promptTimer_Tick(object sender, EventArgs e)
{
 	promptTimer.Tick-=promptTimer_Tick;
    promptTimer.Stop();
    try
    {
            yesButton.Click += new RoutedEventHandler(yesButton_Click);
            laterButton.Click += new RoutedEventHandler(laterButton_Click);
            neverButton.Click += new RoutedEventHandler(neverButton_Click);

            messagePrompt.Message = "ReviewReminder".Localize();

            messagePrompt.ActionPopUpButtons.RemoveAt(0);
            messagePrompt.ActionPopUpButtons.Add(yesButton);
            messagePrompt.ActionPopUpButtons.Add(laterButton);
            //messagePrompt.ActionPopUpButtons.Add(neverButton);
            messagePrompt.Show();
    }
    catch{}
}

        static void yesButton_Click(object sender, RoutedEventArgs e)
        {
            CommonNavigationActions.RateApp();
            messagePrompt.Hide();
            
        }

        static void laterButton_Click(object sender, RoutedEventArgs e)
        {
            RoamingSettings.NumOfRuns = 0;
            messagePrompt.Hide();
        }

        static void neverButton_Click(object sender, RoutedEventArgs e)
        {
            DidReview();
            messagePrompt.Hide();
        }
    }
}
