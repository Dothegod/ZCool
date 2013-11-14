using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;

namespace ZCool
{
    public partial class Versionnfo : UserControl
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public bool IsChn { get; set; }
        public string UpdateInfo { set; get; }
        public Versionnfo()
        {
            InitializeComponent();
        }
        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask Email = new EmailComposeTask();
            Email.To = "highfunstudio@hotmail.com";
            Email.Subject = AppName + " Suggestion";
            Email.Show();
        }

        private void hyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask Rt = new MarketplaceReviewTask();
            Rt.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                textblockUpdateInfo.Text = UpdateInfo;
                textBlockVersion.Text = "Version: " + AppVersion;
                textBlockVersion.Text = "Version: " + AppVersion;
                textBlockVersion.Foreground = this.Foreground;
                textBlockAuthor.Foreground = this.Foreground;
                textBlockSuggestTip.Foreground = this.Foreground;
                hyperlinkButton1.Foreground = this.Foreground;
                hyperlinkButtonReview.Foreground = this.Foreground;
                hyperlinkButtonApp.Foreground = this.Foreground;
                if (IsChn)
                {
                    hyperlinkButtonReview.Content = "给个好评吧，亲！";
                    hyperlinkButtonApp.Content = "获取更多精彩应用";
                    textBlockSuggestTip.Text = "意见反馈：";
                }
            });

        }

        private void hyperlinkButtonApp_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceSearchTask marketsearch = new MarketplaceSearchTask();
            marketsearch.ContentType = MarketplaceContentType.Applications;
            marketsearch.SearchTerms = "HighFunStudio";
            marketsearch.Show();
        }

    }
}
