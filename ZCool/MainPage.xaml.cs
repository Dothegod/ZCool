﻿using System;
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

namespace ZCool
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            Home.WorkDetial = ShowWorkDeatial;
            LiveTitles.SetWideBackgroundImage("/image/WideTitle.png");
        }
        private void ShowWorkDeatial(string Uri)
        {
            string uri = string.Format("/WorkPage.xaml?Target={0}", Uri);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}