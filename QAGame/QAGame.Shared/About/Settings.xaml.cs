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
using QAGame.Model;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace QAGame.About
{
    public sealed partial class Settings : UserControl
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty SettingIntegerTextMarginProperty = DependencyProperty.Register(
            "SettingIntegerTextMargin",
            typeof(Thickness),
            typeof(Settings),
            new PropertyMetadata(new Thickness(10, 42, 0, 0))
        );

        public Thickness SettingIntegerTextMargin
        {
            set
            {
                this.SetValue(SettingIntegerTextMarginProperty, value);
            }
            get
            {
                return (Thickness)this.GetValue(SettingIntegerTextMarginProperty);
            }
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            UserSettings.Instance.SettingsChanged += SettingsChanged;
            SettingsChanged(UserSettings.Instance, EventArgs.Empty);
        }

        void SettingsChanged(object sender, EventArgs e)
        {
            // A better way to keep this updated would be to have IntegerSettingText bind
            // to the same property. This is just an example of how to get notified when
            // a setting changes.
            IntegerSettingText.Text = UserSettings.Instance.SettingInteger.ToString();
        }

        private void Settings_Unloaded(object sender, RoutedEventArgs e)
        {
            UserSettings.Instance.SettingsChanged -= SettingsChanged;
        }
    }
}
