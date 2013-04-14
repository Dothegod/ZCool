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

namespace MyLib.HttpLib
{

    public class DownloadHelper
    {

        public delegate void CallbackEvent(object sender, DownloadEventArgs e);
        public event CallbackEvent DownloadCallbackEvent;
        public void HttpWebRequestDownloadGet(string url)
        {
            Uri _uri = new Uri(url, UriKind.RelativeOrAbsolute);
            HttpWebRequest _httpWebRequest = (HttpWebRequest)WebRequest.Create(_uri);
            _httpWebRequest.Method = "Get";
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
                StreamReader _streamReader = new StreamReader(_streamCallback, Encoding.UTF8);
                string _stringCallback = _streamReader.ReadToEnd();
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
