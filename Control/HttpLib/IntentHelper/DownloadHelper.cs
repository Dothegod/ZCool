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
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.GZip;

namespace MyLib.HttpLib
{

    public class DownloadHelper
    {

        public delegate void CallbackEvent(object sender, DownloadEventArgs e);
        public event CallbackEvent DownloadCallbackEvent;
        private bool IsGzip = false;
        public void HttpWebRequestDownloadGet(string url)
        {
            Uri _uri = new Uri(url, UriKind.RelativeOrAbsolute);
            HttpWebRequest _httpWebRequest = (HttpWebRequest)WebRequest.Create(_uri);
            _httpWebRequest.Method = "Get";
            //_httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Maxthon/4.2.0.4000 Chrome/30.0.1551.0 Safari/537.36";
            System.Net.CookieContainer c = new System.Net.CookieContainer();
            _httpWebRequest.CookieContainer = c;
            AsyncCallback callback = CallBackFunction();
            if (null == callback)
            {
                MessageBox.Show("联网错误");
                return;
            }
            _httpWebRequest.BeginGetResponse(callback, _httpWebRequest);
            _httpWebRequest.AllowAutoRedirect = true;
        }
        public void HttpWebRequestDownloadGet(string url, bool IsGZip)
        {
            HttpWebRequestDownloadGet(url);
            this.IsGzip = IsGZip;
        }


        AsyncCallback CallBackFunction()
        {
            try
            {
                AsyncCallback a = new AsyncCallback(DownLoadCallBack);
                return a;
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络错误");
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        private string UncompressGZip(Stream _streamCallback)
        {
            string Result = "";
            GZipInputStream zipInputStream = new GZipInputStream(_streamCallback);
            long orginalLen = _streamCallback.Length;
            long maxDecompressLen = 20 * orginalLen;

            if (maxDecompressLen < 100000) //缓冲区最小100K,最大8M,原始数据如果大于25KB，则解压缓冲为20倍原始数据大小
            {
                maxDecompressLen = 100000;
            }
            if (maxDecompressLen > 8000000)
            {
                maxDecompressLen = 8000000;
            }
            byte[] data = new byte[maxDecompressLen];
            int size = 0;
            while (true)
            {
                size = zipInputStream.Read(data, 0, data.Length);
                if (size <= 0)
                {

                    break;
                }
                string str = Encoding.UTF8.GetString(data, 0, data.Length);
                Result += str;
            }
            zipInputStream.Close();
            return Result;

        }

        void DownLoadCallBack(IAsyncResult result)
        {

            try
            {
                HttpWebRequest _httpWebRequest = (HttpWebRequest)result.AsyncState;
                if (null == _httpWebRequest)
                {
                    MessageBox.Show("请求错误");
                    return;
                }

                HttpWebResponse _httpWebResponse = (HttpWebResponse)_httpWebRequest.EndGetResponse(result);
                if (null == _httpWebResponse)
                {
                    MessageBox.Show("网络没有应答");
                    return;
                }

                Stream _streamCallback = _httpWebResponse.GetResponseStream();
                string _stringCallback = "";
                if (IsGzip)
                {
                    _stringCallback = UncompressGZip(_streamCallback);
                } 
                else
                {
                    StreamReader _streamReader = new StreamReader(_streamCallback, Encoding.UTF8);
                    _stringCallback = _streamReader.ReadToEnd();
                }
                _stringCallback = _stringCallback.Replace("<br/>", "\n");
                _stringCallback = System.Net.HttpUtility.HtmlDecode(_stringCallback);

                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (DownloadCallbackEvent != null)
                    {
                        DownloadEventArgs _downloadEventArgs = new DownloadEventArgs();
                        _downloadEventArgs._DownloadStream = _streamCallback;
                        _downloadEventArgs._DownloadString = _stringCallback;
                        DownloadCallbackEvent(this, _downloadEventArgs);

                    }
                }));

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
