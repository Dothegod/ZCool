using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyLib.HttpLib;
using System.Windows.Media.Imaging;

namespace ZCool
{
    public partial class WorkPage : PhoneApplicationPage
    {
        public WorkPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.Count > 0)
            {
                string webUri = NavigationContext.QueryString["Target"];
                DownloadHelper dl = new DownloadHelper();
                dl.DownloadCallbackEvent += OnLoadReviewsComplete;
                dl.HttpWebRequestDownloadGet(webUri);
            }
            else
            {
                MessageBox.Show("抱歉，页面参数错误！");
            }
            
        }

        private void OnLoadReviewsComplete(object sender, DownloadEventArgs e)
        {
            string Content = e._DownloadString;
            List<string> ImageList = new WorkParser().Parse(Content);
            foreach (string Source in ImageList)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(Source, UriKind.Absolute));
                img.Stretch = System.Windows.Media.Stretch.Uniform;
                img.Width = ImageStackPanel.ActualWidth;
                ImageStackPanel.Children.Add(img);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}