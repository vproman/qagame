using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QAGame.Data;
using QAGame.Common;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace QAGame
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
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
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel["Section3Items"] = sampleDataGroup;

            if (e.PageState != null && e.PageState.Count > 0)
            {
                SetFirstRunActive((bool)e.PageState["FirstRunActive"]);
                _firstRunIndex = (int)e.PageState["FirstRunIndex"];
                // hack :-(
                if (App.FirstRunAdvanceHubPage)
                {
                    App.FirstRunAdvanceHubPage = false;
                    _firstRunIndex = ITEMVIEW_RETURNRUNINDEX;
                }
                else if (App.FirstRunDismiss)
                {
                    App.FirstRunDismiss = false;
                    DismissTutorialCore();
                }
                UpdateFirstRunCanvas();
            }
            else if (e.NavigationParameter is int)
            {
                if ((int)e.NavigationParameter == -1)
                {
                    DismissTutorialCore();
                }
                else
                {
                    SetFirstRunActive(true);
                    _firstRunIndex = (int)e.NavigationParameter;
                }
                UpdateFirstRunCanvas();
            }
            else
            {
                CheckFirstRun();
            }
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["FirstRunActive"] = _firstRunActive;
            e.PageState["FirstRunIndex"] = _firstRunIndex;
        }

        /// <summary>
        /// Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemPage), itemId);
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
        // To add another screen in the first-run tutorial, here's what you need to do:
        // - In HubPage.xaml, add another child to the FirstRunCanvas
        // - In HubPage.xaml, add Storyboard's to show and hide your new canvas
        // - Adjust MAX_FIRSTRUNINDEX
        // - If necessary, adjust ITEMVIEW_FIRSTRUNINDEX and ITEMVIEW_RETURNRUNINDEX
        // - In UpdateFirstRunCanvas(), check for the appropriate _firstRunIndex
        //   value and follow the mode to show/hide your new canvas as appropriate.
        //   Also hide it if _firstRunActive is false, just like the other storyboards.
        // Once you've done these things, you can also add custom storyboards to make elements
        // blink or whatever (see the FirstRunLastHighlightStoryboard Storyboard in ItemPage.xaml
        // for an example).  The sky's the limit!
        private int _firstRunIndex = 0;
        private bool _firstRunActive = false;
        private DateTime? _lastFirstRunTap = null;
        // COMMENTFIRSTRUN - this constant is the step at which the first-run tutorial will
        // look at an item page
        private const int ITEMVIEW_FIRSTRUNINDEX = 4;
        // COMMENTFIRSTRUN - this constant is the step at which we come back from the item
        // page
        private const int ITEMVIEW_RETURNRUNINDEX = 6;
        // COMMENTFIRSTRUN - this constant is the step when the first-run tutorial is over
        private const int MAX_FIRSTRUNINDEX = 7;
        private void CheckFirstRun()
        {
            // COMMENTFIRSTRUN - if you want to disable the first run tutorial, comment out the next line of code
            Utils.CheckFirstRun(SetFirstRunActive, ref _firstRunIndex, UpdateFirstRunCanvas);
        }

        private void SetFirstRunActive(bool firstRunActive)
        {
            _firstRunActive = firstRunActive;
            // COMMENTFIRSTRUN - add more controls here that shouldn't be able to be interacted
            // with while the tutorial is active
            Hub.IsHitTestVisible = !firstRunActive;

            FirstRunCanvas.Visibility = _firstRunActive ? Visibility.Visible : Visibility.Collapsed;
            // COMMENTFIRSTRUN - set opacity on any other controls here
            double mainOpacity = _firstRunActive ? 0.6 : 1.0;
            Hub.Opacity = mainOpacity;
        }

        private void UpdateFirstRunCanvas()
        {
            // check if time to navigate to item page
            if (_firstRunIndex == ITEMVIEW_FIRSTRUNINDEX)
            {
                this.Frame.Navigate(typeof(ItemPage), _firstRunIndex);
                return;
            }
            // check if done
            if (_firstRunIndex >= MAX_FIRSTRUNINDEX)
            {
                DismissTutorialCore();
            }
            if (_firstRunActive)
            {
                // COMMENTFIRSTRUN - here are where you can show things relating to your
                // first-run tutorial
                if (_firstRunIndex == 0)
                {
                    FirstRunFirstShowStoryboard.Begin();
                    NavigateToHubSection("section1");
                    FirstRunFirst.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunFirstHideStoryboard.Begin();
                }
                if (_firstRunIndex == 1)
                {
                    FirstRunSecondShowStoryboard.Begin();
                    NavigateToHubSection("section2");
                    FirstRunSecond.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunSecondHideStoryboard.Begin();
                }
                if (_firstRunIndex == 2)
                {
                    FirstRunThirdShowStoryboard.Begin();
                    NavigateToHubSection("section3");
                    FirstRunThird.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunThirdHideStoryboard.Begin();
                }
                if (_firstRunIndex == 3)
                {
                    FirstRunFourthShowStoryboard.Begin();
                    NavigateToHubSection("section1");
                    FirstRunFourth.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunFourthHideStoryboard.Begin();
                }
                if (_firstRunIndex == 6)
                {
                    FirstRunFinalShowStoryboard.Begin();
                    NavigateToHubSection("section1");
                    FirstRunFinal.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstRunFinalHideStoryboard.Begin();
                }
            }
            else
            {
                FirstRunFirstHideStoryboard.Begin();
                FirstRunSecondHideStoryboard.Begin();
                FirstRunThirdHideStoryboard.Begin();
                FirstRunFourthHideStoryboard.Begin();
                FirstRunFinalHideStoryboard.Begin();
            }
        }

        private void HideWhenDone(object sender, object e)
        {
            Utils.HideWhenDone(sender, this.FindName);
        }

        private void NavigateToHubSection(string tag)
        {
            var section = Hub.Sections.Where((hs) => (hs.Tag as string == tag)).First();
            Hub.ScrollToSection(section);
        }

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
        }

        #endregion

    }
}
