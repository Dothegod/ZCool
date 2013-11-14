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

using System.IO.IsolatedStorage;
using System.IO;
using System.Text;

namespace MyLib.HttpLib
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    public class DataAccess
    {
        //载入数据成功后的回调
        public delegate void LoadDataCompleted(string result);
        public LoadDataCompleted OnLoadDataCompleted;

        private bool isNeedCache;
        private string uri;
        private IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

        /// <summary>
        /// 从网络获取数据
        /// </summary>
        /// <param name="uri">指定的Uri</param>
        /// <param name="isNeedCache">返回结果是否需要缓存</param>
        public void GetDataFromNet(string uri,bool isNeedCache)
        {
            this.uri = uri;
            this.isNeedCache = isNeedCache;
            WebClient wc = new WebClient();
            wc.Encoding = new HtmlAgilityPack.Gb2312Encoding();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            try
            {
                wc.DownloadStringAsync(new Uri(uri));
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (this.isNeedCache)
            {
                string fileName = ConvertUriToFileName(this.uri);
                SaveStringToLocal(fileName, e.Result);
            }
            try
            {
                if (OnLoadDataCompleted != null)
                {
                    this.OnLoadDataCompleted(e.Result);
                }
            }
            catch (System.Exception ex)
            {
                ex.ToString();
                MessageBox.Show("获取网页内容失败");
            }
        }

        /// <summary>
        /// 通过本地缓存获取数据
        /// </summary>
        /// <param name="uri">指定Uri</param>
        public void GetDataFromCache(string uri)
        {
            this.uri = uri;
            this.isNeedCache = true;
            string fileName = ConvertUriToFileName(uri);
            if (storage.FileExists(fileName))
            {
                string data = GetStringFromLocal(fileName);
                this.OnLoadDataCompleted(data);
            }
            else
            {
                this.GetDataFromNet(uri,true);
            }
        }

        /// <summary>
        /// 从本地存储中读取字符串
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>读取的字符串</returns>
        protected string GetStringFromLocal(string fileName)
        {
            using (StreamReader reader = new StreamReader(storage.OpenFile(fileName, FileMode.Open)))
            {
                return reader.ReadToEnd() ;
            }
        }

        /// <summary>
        /// 保存字符串到本地存储中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">要保存的字符串</param>
        protected void SaveStringToLocal(string fileName,string data)
        {
            using (StreamWriter writer = new StreamWriter(storage.OpenFile(fileName, FileMode.Create)))
            {
                writer.Write(data);
            }
        }

        protected string ConvertUriToFileName(string uri)
        {
            return uri.Replace('/', '_').Replace('?', '!').Replace('&','-').Replace(':','~');
        }
        
    }
}
