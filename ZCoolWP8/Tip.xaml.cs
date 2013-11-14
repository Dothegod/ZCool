using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ZCool
{
    public partial class Tip : UserControl
    {
        public string Text
        {
            set
            {
                textblockContent.Text = value;
            }
        }
        public Tip()
        {
            InitializeComponent();
        }
        public void Show()
        {
            this.Appear.Begin();
            Visibility = Visibility.Visible;
            Timer t = new Timer(new TimerCallback(TimerOut), null, 2000, Timeout.Infinite);
        }
        private void TimerOut(object o)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    Visibility = Visibility.Collapsed;
                });
        }
    }
}
