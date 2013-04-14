﻿using System;
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
        private const string HomeUri = "http://www.zcool.com.cn/";
        private int PageIndex = 1;
        public Home()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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

                ImagesWrapPanel.Children.Add(img);

            }
        }

        private void UpdateIndexShow(List<Issue> IndexSHow)
        {
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
        }


    }
}
