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
using Microsoft.Devices;

namespace ZCool
{
    public partial class WorkPage : PhoneApplicationPage
    {
        private int PageIndex = 1;
        private string WorkUri;
        private int PageCount = 0;
        ProgressIndicator m_ProgressIndicator;

        public WorkPage()
        {
            InitializeComponent();
            m_ProgressIndicator = new Microsoft.Phone.Shell.ProgressIndicator();
            Microsoft.Phone.Shell.SystemTray.ProgressIndicator = m_ProgressIndicator;
            m_ProgressIndicator.Text = "加载中...";
            CanGetMore(false);
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
            if (ImageList.Count == 0)
            {
                MessageBox.Show("抱歉，这个作品包含视频，无法展示");
                NavigationService.GoBack();
            }
            PageCount = Info.PageCount;

            foreach (string Source in ImageList)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(Source, UriKind.Absolute));
                img.Stretch = System.Windows.Media.Stretch.Uniform;
                img.Width = ImageStackPanel.ActualWidth;
                img.Hold += Image_Hold;
                img.ImageOpened += ImageOpen;
                ImageStackPanel.Children.Add(img);
            }

        }
        private void ImageOpen(object sender, RoutedEventArgs e)
        {
            CanGetMore(true);
        }

        private void CanGetMore(bool flag)
        {
            MoreButton.IsEnabled = flag;
            MoreButton.Content = flag ? "更多" : "读取中，请稍后……";
            SystemTray.IsVisible = !flag;
            m_ProgressIndicator.IsIndeterminate = !flag;
            m_ProgressIndicator.IsVisible = !flag;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex >= PageCount)
            {
                MessageBox.Show("后面没有了");
                return;
            }
            PageIndex++;
            CanGetMore(false);
            DownloadHelper dl = new DownloadHelper();
            dl.DownloadCallbackEvent += new DownloadHelper.CallbackEvent(OnLoadComplete);
            string WebUri = string.Format("{0}/{1}.html", WorkUri, PageIndex);
            dl.HttpWebRequestDownloadGet(WebUri);
        }
        private void Image_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            try
            {
	            Image img = sender as Image;
	            BitmapImage bimg = img.Source as BitmapImage;
	
	            SavePicToGallery.Save(img, bimg.UriSource.AbsoluteUri);
                Tip.Text = "保存成功";
                VibrateController.Default.Start(TimeSpan.FromSeconds(0.3));
                Tip.Show();
            }
            catch 
            {
                MessageBox.Show("保存失败！");
            }
        }


    }
}