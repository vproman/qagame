using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QAGame.About
{
    public partial class OtherApp : UserControl
    {
        public OtherApp()
        {
            InitializeComponent();
            UpdateVisibility();
        }

        public static readonly DependencyProperty WindowsPhoneAppIdProperty = DependencyProperty.Register(
            "WindowsPhoneAppId",
            typeof(string),
            typeof(OtherApp),
            new PropertyMetadata(null, WindowsPhoneAppIdChanged)
        );

        public static readonly DependencyProperty WindowsPackageFamilyNameProperty = DependencyProperty.Register(
            "WindowsPackageFamilyName",
            typeof(string),
            typeof(OtherApp),
            new PropertyMetadata(null, WindowsPackageFamilyNameChanged)
        );
        public static readonly DependencyProperty AppTitleProperty = DependencyProperty.Register(
            "AppTitle",
            typeof(string),
            typeof(OtherApp),
            null
        );
        public static readonly DependencyProperty AppDescriptionProperty = DependencyProperty.Register(
            "AppDescription",
            typeof(string),
            typeof(OtherApp),
            null
        );
        public static readonly DependencyProperty ImageSrcProperty = DependencyProperty.Register(
            "ImageSrc",
            typeof(string),
            typeof(OtherApp),
            null
        );

        private static void WindowsPhoneAppIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OtherApp oa = (OtherApp)d;
            oa.UpdateVisibility();
        }

        private static void WindowsPackageFamilyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OtherApp oa = (OtherApp)d;
            oa.UpdateVisibility();
        }
        public void UpdateVisibility()
        {
#if WINDOWS_PHONE_APP
            Visibility = string.IsNullOrEmpty(WindowsPhoneAppId) ? Visibility.Collapsed : Visibility.Visible;
#else
            Visibility = string.IsNullOrEmpty(WindowsPackageFamilyName) ? Visibility.Collapsed : Visibility.Visible;
#endif
        }

        public string WindowsPhoneAppId
        {
            set
            {
                this.SetValue(WindowsPhoneAppIdProperty, value);
            }
            get
            {
                return (string)this.GetValue(WindowsPhoneAppIdProperty);
            }
        }
        public string WindowsPackageFamilyName
        {
            set
            {
                this.SetValue(WindowsPackageFamilyNameProperty, value);
            }
            get
            {
                return (string)this.GetValue(WindowsPackageFamilyNameProperty);
            }
        }
        public string AppTitle
        {
            set
            {
                this.SetValue(AppTitleProperty, value);
            }
            get
            {
                return (string)this.GetValue(AppTitleProperty);
            }
        }
        public string AppDescription
        {
            set
            {
                this.SetValue(AppDescriptionProperty, value);
            }
            get
            {
                return (string)this.GetValue(AppDescriptionProperty);
            }
        }
        public string ImageSrc
        {
            set
            {
                this.SetValue(ImageSrcProperty, value);
            }
            get
            {
                return (string)this.GetValue(ImageSrcProperty);
            }
        }

        private async void me_Tap(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
            //Uri uri = new Uri("http://windowsphone.com/s?appId=" + AppId);
            if (string.IsNullOrEmpty(WindowsPhoneAppId)) return;
            Uri uri = new Uri("zune:navigate?appid=" + WindowsPhoneAppId);
#else
            if (string.IsNullOrEmpty(WindowsPackageFamilyName)) return;
            Uri uri = new Uri("ms-windows-store:PDP?PFN=" + WindowsPackageFamilyName);
#endif
            await Launcher.LaunchUriAsync(uri);
        }

        public static readonly DependencyProperty SharedNormalSizeProperty = DependencyProperty.Register(
            "SharedNormalSize",
            typeof(double),
            typeof(OtherApp),
            new PropertyMetadata((double)20)
        );

        public double SharedNormalSize
        {
            set
            {
                this.SetValue(SharedNormalSizeProperty, value);
            }
            get
            {
                return (double)this.GetValue(SharedNormalSizeProperty);
            }
        }

        public static readonly DependencyProperty SharedLargerSizeProperty = DependencyProperty.Register(
            "SharedLargerSize",
            typeof(double),
            typeof(OtherApp),
            new PropertyMetadata((double)26)
        );

        public double SharedLargerSize
        {
            set
            {
                this.SetValue(SharedLargerSizeProperty, value);
            }
            get
            {
                return (double)this.GetValue(SharedLargerSizeProperty);
            }
        }

        public static readonly DependencyProperty PhoneFontFamilySemiBoldProperty = DependencyProperty.Register(
            "PhoneFontFamilySemiBold",
            typeof(string),
            typeof(OtherApp),
            new PropertyMetadata("Segoe WP Semibold")
        );

        public string PhoneFontFamilySemiBold
        {
            set
            {
                this.SetValue(PhoneFontFamilySemiBoldProperty, value);
            }
            get
            {
                return (string)this.GetValue(PhoneFontFamilySemiBoldProperty);
            }
        }

    }
}
