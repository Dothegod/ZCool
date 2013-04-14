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

namespace ZCool
{
    public partial class Home : UserControl
    {
        private string HomeUri = "http://www.zcool.com.cn/";
        public Home()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadHelper dl = new DownloadHelper();
            dl.DownloadCallbackEvent += new DownloadHelper.CallbackEvent(OnLoadReviewsComplete);
            dl.HttpWebRequestDownloadGet(HomeUri);
        }
        private void OnLoadReviewsComplete(object sender, DownloadEventArgs e)
        {
            HomeParser parser = new HomeParser();
            Issues Issue = parser.Parse(e._DownloadString);

            UpdateIndexShow(Issue);

            UpdateCams(Issue);
        }

        private void UpdateCams(Issues Issue)
        {
            List<Issue> CamList = Issue.CamList;
            double width = ImagesWrapPanel.ActualWidth / 2;
            foreach (Issue i in CamList)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(i.ImageUri, UriKind.Absolute));
                img.Tag = i.TargetUri;
                img.Width = width;

                ImagesWrapPanel.Children.Add(img);

            }
        }

        private void UpdateIndexShow(Issues Issue)
        {
            List<Issue> IndexSHow = Issue.IndexSHowList;
            foreach (Issue i in IndexSHow)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(i.ImageUri, UriKind.Absolute));
                img.Tag = i.TargetUri;
                PivotItem Item = new PivotItem();
                Item.Margin = new Thickness(0);
                Item.Content = img;

                SuggestPivot.Items.Add(Item);
            }
        }

    }
}
