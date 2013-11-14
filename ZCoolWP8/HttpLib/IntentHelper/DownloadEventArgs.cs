using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace MyLib.HttpLib
{
    public class DownloadEventArgs:EventArgs
    {
        public Stream _DownloadStream { get; set; }
        public String _DownloadString { get; set; }
        public int _DownloadState { get; set; }
    }
}
