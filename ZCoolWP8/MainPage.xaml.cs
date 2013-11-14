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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ZCool
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            Home.WorkDetial = ShowWorkDeatial;
            //LiveTitles.SetWideBackgroundImage("/image/WideTitle.png");
            Indicator.Text = "加载中...";

            Home.IsDownLoadFinished = CanGetMore;
            string Version = "";
            DataStorage.GetInstance().LoadData("version", ref Version);
            if (Version != App.Version)
            {
                MessageBox.Show("本应用均为高清大图，建议在WiFi环境下使用\n长按图片可以保存至本地");
                DataStorage.GetInstance().SaveData("version", App.Version);
            }
 
        }
        private void ShowWorkDeatial(string Uri)
        {
            string uri = string.Format("/WorkPage.xaml?Target={0}", Uri);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
        private void CanGetMore(bool flag)
        {
            SystemTray.IsVisible = !flag;
            Indicator.IsIndeterminate = !flag;
            Indicator.IsVisible = !flag;
        }



    }
}