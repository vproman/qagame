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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace QAGame.About
{
    public sealed partial class OtherAppsPage : UserControl
    {
        public OtherAppsPage()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty SharedNormalSizeProperty = DependencyProperty.Register(
            "SharedNormalSize",
            typeof(double),
            typeof(OtherAppsPage),
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
            typeof(OtherAppsPage),
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
            typeof(OtherAppsPage),
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
