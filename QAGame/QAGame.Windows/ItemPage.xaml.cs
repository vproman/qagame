using QAGame.Data;
using QAGame.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace QAGame
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        
        /// <summary>
        /// Gets the NavigationHelper used to aid in navigation and process lifetime management.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the DefaultViewModel. This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            if (e.NavigationParameter is int)
            {
                // COMMENTFIRSTRUN - First-run - just show some item.                
                var item = await SampleDataSource.GetDemoItemAsync();
                this.DefaultViewModel["Item"] = item;
                SetFirstRunActive(true);
                _firstRunIndex = (int)e.NavigationParameter;
            }
            else
            {
                var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
                this.DefaultViewModel["Item"] = item;
            }
            UpdateFirstRunCanvas();
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["FirstRunActive"] = _firstRunActive;
            e.PageState["FirstRunIndex"] = _firstRunIndex;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region First-run tutorial
        private bool _firstRunActive = false;
        private int _firstRunIndex = 0;
        private DateTime? _lastFirstRunTap = null;
        // COMMENTFIRSTRUN - the step at which we start showing the first-run tutorial on this
        // page.
        private const int MIN_FIRSTRUNINDEX = 4;
        // COMMENTFIRSTRUN - the step at which we show the main screen again
        private const int MAX_FIRSTRUNINDEX = 6;

        private void FirstRunCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (_firstRunActive)
            {
                // don't allow taps too fast to prevent accidents
                if (_lastFirstRunTap.HasValue &&
                    DateTime.Now.Subtract(_lastFirstRunTap.Value).CompareTo(TimeSpan.FromMilliseconds(500)) < 0)
                {
                    return;
                }
                _lastFirstRunTap = DateTime.Now;
                _firstRunIndex++;
                UpdateFirstRunCanvas();
            }
        }

        private void SetFirstRunActive(bool firstRunActive)
        {
            _firstRunActive = firstRunActive;
            // COMMENTFIRSTRUN - add more controls here that shouldn't be able to be interacted
            // with while the tutorial is active
            contentRegion.IsHitTestVisible = !firstRunActive;
            TitleTextPanel.IsHitTestVisible = !firstRunActive;

            FirstRunCanvas.Visibility = _firstRunActive ? Visibility.Visible : Visibility.Collapsed;
            double mainOpacity = _firstRunActive ? 0.6 : 1.0;
            contentRegion.Opacity = mainOpacity;
        }

        private void UpdateFirstRunCanvas()
        {
            // check if done already
            if (_firstRunIndex >= MAX_FIRSTRUNINDEX)
            {
                // kick out to main page again
                SetFirstRunActive(false);
                int realFirstRunIndex = _firstRunIndex;
                _firstRunIndex = 0;
                if (Frame.CanGoBack && Frame.BackStack[Frame.BackStackDepth - 1].SourcePageType == typeof(HubPage))
                {
                    App.FirstRunAdvanceHubPage = true;
                    Frame.GoBack();
                }
                else
                {
                    App.FirstRunAdvanceHubPage = false;
                    Frame.Navigate(typeof(HubPage), realFirstRunIndex);
                }
                return;
            }

            if (_firstRunActive)
            {
                if (_firstRunIndex == 4)
                {
                    FirstRunFirstShowStoryboard.Begin();
                    FirstRunFirst.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunFirstHideStoryboard.Begin();
                }
                if (_firstRunIndex == 5)
                {
                    FirstRunLastShowStoryboard.Begin();
                    FirstRunLast.Visibility = Visibility.Visible;
                    FirstRunLastHighlightStoryboard.Begin();
                }
                else
                {
                    FirstRunLastHideStoryboard.Begin();
                    FirstRunLastHighlightStoryboard.Stop();
                }
            }
            else
            {
                FirstRunFirstHideStoryboard.Begin();
                FirstRunLastHideStoryboard.Begin();
            }
        }

        private void HideWhenDone(object sender, object e)
        {
            Utils.HideWhenDone(sender, this.FindName);
        }

        private void DismissTutorial(object sender, TappedRoutedEventArgs e)
        {
            DismissTutorialCore();
        }
        private void DismissTutorialCore()
        {
            SetFirstRunActive(false);
            // COMMENTFIRSTRUN - done with first-run tutorial, delete any
            // demo objects you created, etc.
            _firstRunIndex = 0;

            if (Frame.CanGoBack && Frame.BackStack[Frame.BackStackDepth - 1].SourcePageType == typeof(HubPage))
            {
                App.FirstRunDismiss = true;
                Frame.GoBack();
            }
            else
            {
                App.FirstRunDismiss = false;
                Frame.Navigate(typeof(HubPage), -1);
            }

        }

        #endregion
 
    }
}
