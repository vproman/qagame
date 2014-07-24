using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#if WINDOWS_PHONE_APP
using Windows.ApplicationModel.Email;
#endif

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace QAGame.About
{
    public sealed partial class AboutMain : UserControl
    {
        public AboutMain()
        {
            this.InitializeComponent();
            var nameHelper = Package.Current.Id;
            this.RealApplicationVersion.Text = nameHelper.Version.Major + "." + nameHelper.Version.Minor + "." + nameHelper.Version.Build;
        }

        public static readonly DependencyProperty StandardThicknessProperty = DependencyProperty.Register(
            "StandardThickness",
            typeof(Thickness),
            typeof(AboutMain),
            new PropertyMetadata(new Thickness(0,0,0,0))            
        );

        public Thickness StandardThickness
        {
            set
            {
                this.SetValue(StandardThicknessProperty, value);
            }
            get
            {
                return (Thickness)this.GetValue(StandardThicknessProperty);
            }
        }

        public static readonly DependencyProperty SharedNormalSizeProperty = DependencyProperty.Register(
            "SharedNormalSize",
            typeof(double),
            typeof(AboutMain),
            null
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

        public static readonly DependencyProperty PhoneFontFamilySemiBoldProperty = DependencyProperty.Register(
            "PhoneFontFamilySemiBold",
            typeof(string),
            typeof(AboutMain),
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

        private async void HomepageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // TODOABOUT - fill in your app's homepage URL here
            await Launcher.LaunchUriAsync(new Uri("http://example.com/"));
        }

        private async void MailButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // TODOABOUT - change these as desired
            string subjectName = "QAGame.Shared Support";
            string emailAddress = "me@example.com";
#if WINDOWS_PHONE_APP
            string emailName = "My Name";
#endif
#if WINDOWS_PHONE_APP
            var email = new EmailMessage()
            {
                Subject = subjectName
            };
            email.To.Add(new EmailRecipient(emailAddress, emailName));
            await EmailManager.ShowComposeNewEmailAsync(email);
#else
            var mailto = new Uri("mailto:?to=" + emailAddress + "&subject=" + subjectName);
            await Launcher.LaunchUriAsync(mailto);
#endif
        }

        private async void ReviewButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

    }
}
