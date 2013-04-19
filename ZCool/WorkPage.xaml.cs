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
        private int PageIndex = 1;
        private string WorkUri;
        private int PageCount = 0;
        public WorkPage()
        {
            InitializeComponent();
            CanGetMore(true);
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.Count > 0)
            {
                string webUri = NavigationContext.QueryString["Target"];
                WorkUri = webUri.Replace(".html", "");
                DownloadHelper dl = new DownloadHelper();
                dl.DownloadCallbackEvent += OnLoadComplete;
                dl.HttpWebRequestDownloadGet(webUri);
            }
            else
            {
                MessageBox.Show("抱歉，页面参数错误！");
            }
            
        }

        private void OnLoadComplete(object sender, DownloadEventArgs e)
        {
            string Content = e._DownloadString;
            WorkInfo Info = new WorkParser().Parse(Content);
            List<string> ImageList = Info.ImageList;
            PageCount = Info.PageCount;
            foreach (string Source in ImageList)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(Source, UriKind.Absolute));
                img.Stretch = System.Windows.Media.Stretch.Uniform;
                img.Width = ImageStackPanel.ActualWidth;
                ImageStackPanel.Children.Add(img);
            }
            CanGetMore(true);

        }

        private void CanGetMore(bool flag)
        {
            MoreButton.IsEnabled = flag;
            MoreButton.Content = flag ? "更多" : "读取中，请稍后……";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex >= PageCount)
            {
                MessageBox.Show("后面没有了");
                return;
            }
            CanGetMore(false);
            DownloadHelper dl = new DownloadHelper();
            dl.DownloadCallbackEvent += new DownloadHelper.CallbackEvent(OnLoadComplete);
            string WebUri = string.Format("{0}/{1}.html", WorkUri, PageIndex);
            dl.HttpWebRequestDownloadGet(WebUri);
        }

    }
}