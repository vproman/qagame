using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using QAGame.Common;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
using QAGame.About;
#endif

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace QAGame
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
#if WINDOWS_APP
            // Sigh, this is a hack because you can't seem to put these in a ResourceDictionary
            // for a Windows app?
            this.Resources["SharedLargerSize"] = (double)24;
            this.Resources["SharedNormalSize"] = (double)20;
            this.Resources["PhoneFontFamilySemiBold"] = this.Resources["ContentControlThemeFontFamily"];
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(HubPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        // COMMENTFIRSTRUN - this is a bit of a hack to deal with the fact that
        // the HubPage can be gotten to two different ways, and the NavigationHelper
        // makes my usual BackStack tricks not work...
        public static bool FirstRunAdvanceHubPage { get; set; }

        // A similar hack for dismissing the first-run tutorial
        public static bool FirstRunDismiss { get; set; }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

#if WINDOWS_APP
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Tips", "Tips", (handler) => ShowTipsSettingFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "About", "About", (handler) => ShowAboutSettingFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Settings", "Settings", (handler) => ShowSettingsSettingFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Other Apps", "Other Apps", (handler) => ShowOtherAppsSettingFlyout()));
        }

        public void ShowTipsSettingFlyout()
        {
            SettingsFlyout tipsFlyout = new SettingsFlyout();
            Tips tips = new Tips();
            tips.TutorialTapped += (sender, args) =>
            {
                Frame frame = Window.Current.Content as Frame;
                if (frame == null) return;
                if (frame.CanGoBack && frame.BackStack[frame.BackStackDepth - 1].SourcePageType == typeof(HubPage))
                {
                    frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
                }
                frame.Navigate(typeof(HubPage), 0);
            };
            tipsFlyout.Content = tips;
            tipsFlyout.Title = "Tips";
            tipsFlyout.Show();
        }

        public void ShowAboutSettingFlyout()
        {
            SettingsFlyout aboutFlyout = new SettingsFlyout();
            AboutMain aboutMain = new AboutMain();
            // This is the nicer way to set SharedNormalSize in this template.
            // You can just use {StaticResource SharedNormalSize}, but that means
            // every time you get any error you'll see a bunch of fake ones as well.
            aboutMain.SharedNormalSize = (double)this.Resources["SharedNormalSize"];
            aboutFlyout.Content = aboutMain;
            aboutFlyout.Title = "About";
            aboutFlyout.Show();
        }

        public void ShowSettingsSettingFlyout()
        {
            SettingsFlyout settingsFlyout = new SettingsFlyout();
            settingsFlyout.Content = new Settings();
            settingsFlyout.Title = "Settings";
            settingsFlyout.Show();
        }

        public void ShowOtherAppsSettingFlyout()
        {
            SettingsFlyout otherAppsFlyout = new SettingsFlyout();
            OtherAppsPage otherAppsPage = new OtherAppsPage();
            otherAppsPage.SharedLargerSize = (double)this.Resources["SharedLargerSize"];
            otherAppsPage.SharedNormalSize = (double)this.Resources["SharedNormalSize"];
            otherAppsFlyout.Content = otherAppsPage;
            otherAppsFlyout.Title = "Other Apps";
            otherAppsFlyout.Show();
        }
#endif
    }
}
