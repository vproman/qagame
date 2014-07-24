using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace QAGame
{
    public static class Utils
    {
        public static void CheckFirstRun(Action<bool> setFirstRunActive, ref int firstRunIndex, Action updateFirstRunCanvas)
        {
            // COMMENTFIRSTRUN - set this to true to always show the first-run
            // tutorial (useful for testing)
            bool alwaysDoFirstRun = false;
            if (alwaysDoFirstRun ||
    (!ApplicationData.Current.LocalSettings.Values.ContainsKey("LastRun")))
            {
                setFirstRunActive(true);
                firstRunIndex = 0;
            }
            else
            {
                setFirstRunActive(false);
            }
            ApplicationData.Current.LocalSettings.Values["LastRun"] = DateTime.Now.Ticks;
            updateFirstRunCanvas();
        }

        public static void HideWhenDone(object sender, Func<string, object> findName)
        {
            string targetName = (string)((DependencyObject)(sender)).GetValue(Storyboard.TargetNameProperty);
            if (targetName != null)
            {
                FrameworkElement elem = findName(targetName) as FrameworkElement;
                if (elem != null)
                {
                    elem.Visibility = Visibility.Collapsed;
                }
            }
        }

    }
}
