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
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace ZCool
{
    public partial class Home : UserControl
    {
        private const string HomeUri = "http://www.zcool.com.cn/";
        private int PageIndex = 1;
        public Action<string> WorkDetial;
        public Action<bool> IsDownLoadFinished;
        bool isStart = true;
        public Home()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (ImagesWrapPanel.Children.Count > 0)
            {
                return;
            }
            CanGetMore(false);
            DownloadHelper dl = new DownloadHelper();
            dl.DownloadCallbackEvent += new DownloadHelper.CallbackEvent(OnLoadReviewsComplete);
            dl.HttpWebRequestDownloadGet(HomeUri);

        }
        private void OnLoadReviewsComplete(object sender, DownloadEventArgs e)
        {
            HomeParser parser = new HomeParser();
            Issues Issue = parser.Parse(e._DownloadString);

            UpdateIndexShow(Issue.IndexSHowList);

            UpdateCams(Issue.CamList);
            CanGetMore(true);
            if (isStart)
            {
                AskForReview();
            }
            isStart = false;

        }

        private void UpdateCams(List<Issue> CamList)
        {
            double width = ImagesWrapPanel.ActualWidth / 2;
            foreach (Issue i in CamList)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(i.ImageUri, UriKind.Absolute));
                img.Tag = i.TargetUri;
                img.Width = width;
                img.Tap += ImageTap;

                ImagesWrapPanel.Children.Add(img);

            }
        }
        private void ImageTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (WorkDetial != null)
            {
                Image img = sender as Image;
                string Uri = (string)img.Tag;
                if (string.IsNullOrEmpty(Uri))
                {
                    return;
                }
                WorkDetial(Uri);
            }
            
        }


        private void UpdateIndexShow(List<Issue> IndexSHow)
        {
            foreach (Issue i in IndexSHow)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(i.ImageUri, UriKind.Absolute));
                img.Tag = i.TargetUri;
                img.Tap += ImageTap;
                PivotItem Item = new PivotItem();
                Item.Margin = new Thickness(0);
                Item.Content = img;
                
                SuggestPivot.Items.Add(Item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CanGetMore(false);
            PageIndex++;
            DownloadHelper dl = new DownloadHelper();
            dl.DownloadCallbackEvent += new DownloadHelper.CallbackEvent(OnLoadCamComplete);
            string WebUri = string.Format("{0}index.do?p={1}#mainList", HomeUri, PageIndex);
            dl.HttpWebRequestDownloadGet(WebUri);
            
        }

        private void OnLoadCamComplete(object sender, DownloadEventArgs e)
        {
            HomeParser parser = new HomeParser();
            List<Issue> Issue = parser.ParseImage(e._DownloadString);

            UpdateCams(Issue);
            CanGetMore(true);
        }

        private void CanGetMore(bool flag)
        {
            MoreButton.IsEnabled = flag;
            MoreButton.Content = flag ? "更多" : "读取中，请稍后……";
            if (IsDownLoadFinished != null)
            {
                IsDownLoadFinished(flag);
            }
        }

        private void AskForReview()
        {
            int times = 0;
            DataStorage.GetInstance().LoadData("time", ref times);
#if DEBUG
            if (times > 3)
#else
            if (times == 3)
#endif
            {
                if (MessageBoxResult.OK == MessageBox.Show("亲，觉得好用就给个好评吧^_^", "求好评", MessageBoxButton.OKCancel))
                {
                    MarketplaceReviewTask Rt = new MarketplaceReviewTask();
                    Rt.Show();
                }
            }
            times++;
            DataStorage.GetInstance().SaveData("time", times);

        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (App.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/VersionPage.xaml", UriKind.Relative));
        }



    }
}
