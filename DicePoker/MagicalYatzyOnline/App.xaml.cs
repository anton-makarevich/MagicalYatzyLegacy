using DicePokerRT;
using Sanet;
using Sanet.Kniffel.ViewModels;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MagicalYatzyOnline
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //init markedup analitics
            MarkedUp.AnalyticClient.Initialize("72937eb8-be46-4b18-8816-2a433254af62");
            //init localizer
            LocalizerExtensions.RModel = new ResourceModel();
            //load sounds
            SoundsProvider.Init();
            //check numofruns
            ReviewBugger.CheckNumOfRuns();

            //init viewmodels
            var ngvm = ViewModelProvider.GetViewModel<NewGameViewModel>();
            var pgvm = ViewModelProvider.GetViewModel<PlayGameViewModel>();

            //init logger
            LogManager.LoggingLevel = LogLevel.Warning;
            LogManager.MessageLogged += LogManager_MessageLogged;

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            //starting navigation tracking in MarkedUp
            MarkedUp.AnalyticClient.RegisterNavigationFrame(rootFrame);

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            SmartDispatcher.Initialize(rootFrame.Dispatcher);
            // Ensure the current window is active
            Window.Current.Activate();
            //register for settings charm event
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
        }
        object objSync = new object();
        static ILogConsole Logger = new RTLogger();
        void LogManager_MessageLogged(string from, string line, int level)
        {
            lock (objSync)
            {
                String message = string.Format("{0}: {1}, {2}", DateTime.Now, from, line);
                Logger.WriteLine(message);
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        #region settings

        public static TaskPanePopup Settings;
        private TaskPanePopup _policy;
        private void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            //policy
            SettingsCommand cmd = new SettingsCommand("PrivacyPolicy", LocalizerExtensions.RModel.GetString("PrivacyLabel/Text"), (command) =>
            {
                if (_policy == null)
                {

                    _policy = new TaskPanePopup(new PrivacyPolicyControl());
                }
                _policy.Show();
            });
            args.Request.ApplicationCommands.Add(cmd);
            //options
            SettingsCommand cmd2 = new SettingsCommand("Options", LocalizerExtensions.RModel.GetString("SettingsCaption/Text"), (command) =>
            {
                if (Settings == null)
                {

                    Settings = new TaskPanePopup(new SettingsPage());
                }
                Settings.Show();
            });

            args.Request.ApplicationCommands.Add(cmd2);
            //about
            //SettingsCommand cmd3 = new SettingsCommand("About", resProvider.GetString("AboutCaption/Text"), (command) =>
            //{
            //    ((Frame)Window.Current.Content).Navigate(typeof(AboutPage));
            //});

            //args.Request.ApplicationCommands.Add(cmd3);

        }
        #endregion
    }
}
