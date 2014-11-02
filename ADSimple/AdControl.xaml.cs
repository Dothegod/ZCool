using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MSNADSDK.AD;
using System.Windows.Media;

namespace ADSimple
{
    public partial class AdControl : UserControl
    {
        string defaultAppid = "143796";
        string defaultAdId = "188596";
//         string defaultIntersitialAdId = "100004";
        string defaultSerectKey = "cf546758f1c14444b41dab482dc97c2f";

        static bool IsShow = false;
        bool IsClick = false;

        public AdControl()
        {
            InitializeComponent();
        }
        private void request()
        {

            if (IsShow == true)
            {
                return;
            }

            AdView adView = new AdView();

            adView.Appid = defaultAppid;
            adView.SecretKey = defaultSerectKey;
            adView.SizeForAd = AdSize.Large;
            adView.Adid = defaultAdId;

            adView.TelCapability = true;
            adView.StorePic = true;
            adView.Tap += LayoutRoot_Tap;
            adView.MouseLeftButtonUp += mouseup;

            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(adView);

            adView.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            adView.HorizontalAlignment = HorizontalAlignment.Stretch;
            adView.LogOutput += adView_LogOutput;
            adView.AdRequestSuccessEvent += adView_AdRequestSuccessEvent;
            adView.AdRequestErrorEvent += adView_AdRequestErrorEvent;
            adView.AdSdkExceptionEvent += adView_AdSdkExceptionEvent;
            adView.AdActionEvent += adView_AdActionEvent;
            adView.LeavingApplication += adView_LeavingApplication;
        }

        void adView_LeavingApplication(object sender, EventArgs e)
        {
            IsClick = true;

        }

        void adView_AdActionEvent(object sender, AdActionEventArgs e)
        {
            IsClick = true;

        }

        void adView_AdSdkExceptionEvent(object sender, ADExceptionEventArgs e)
        {
            request();

        }

        void adView_LogOutput(object sender, EventArgs e)
        {
            LogEventArgs logArgs = e as LogEventArgs;
            if (IsClick == true)
            {
                IsClick = false;
                (this.Parent as Panel).Children.Remove(this);
            }

        }

        void adView_AdRequestErrorEvent(object sender, AdRequestStatesEventArgs args)
        {
            request();
        }

        void adView_AdRequestSuccessEvent(object sender, AdRequestStatesEventArgs args)
        {
            if (IsClick == true)
            {
                IsClick = false;
                (this.Parent as UIElementCollection).Remove(this);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            request();
        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IsClick = true;
            IsShow = true;
        }
        private void mouseup(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsClick = true;
            IsShow = true;
        }

        

    }

}
