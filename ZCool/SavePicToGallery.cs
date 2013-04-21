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
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;


namespace ZCool
{
    public class SavePicToGallery
    {
        private readonly string filePath = null;
        private const string CacheDirectory = "CachedImages";
        public static Action ShowSuccTip;

        private SavePicToGallery()
        {
        }
        public static void Save(Image img)
        {
            try 
            {
                BitmapImage BitImg = img.Source as BitmapImage;

                string FileName = BitImg.UriSource.AbsolutePath.TrimStart('/').Replace('/', '_');

                Save(img, FileName);

            }
            catch
            {
                MessageBox.Show("无法获取图片地址，保存失败！");
                
            }
        }

        public static void Save(Image img, string FileName)
        {
            if (img == null)
            {
                return;
            }
            BitmapImage BitImg = img.Source as BitmapImage;
            if (BitImg.UriSource == null)
            {
                return;
            }
            try
            {
                string Name = PicName(FileName);
                MediaLibrary library = new MediaLibrary();

                var myStore = IsolatedStorageFile.GetUserStoreForApplication();


                if (myStore.FileExists(Name))
                {
                    MessageBox.Show("图片已经存在");
                    return;
                }
                else
                {
                    IsolatedStorageFileStream myFileStream = myStore.CreateFile(Name);
                    WriteableBitmap CaptureImage = new WriteableBitmap(BitImg);

                    // 将WriteableBitmap转换为JPEG流编码，并储存到独立存储里.
                    Extensions.SaveJpeg(CaptureImage, myFileStream, CaptureImage.PixelWidth, CaptureImage.PixelHeight, 0, 85);
                    myFileStream.Close();               // 关闭文件流.

                    //从独立存储里读出刚存入的图片文件.
                    myFileStream = myStore.OpenFile(Name, FileMode.Open, FileAccess.Read);

                    //把图片加在WP7 手机的媒体库.
                    Picture pic = library.SavePicture(Name, myFileStream);
                    myFileStream.Close();
                    myStore.DeleteFile(Name);
                }
            }
            catch
            {
                MessageBox.Show("保存失败");
                return;
            }
            if (ShowSuccTip != null)
            {
                ShowSuccTip();
            }
        }
        private void SavePictureToLib()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        MediaLibrary mediaLibrary = new MediaLibrary();
                        Picture pic = mediaLibrary.SavePicture("deeweibo_" + DateTime.Now.ToString("yyyyMMddhhmmss"), fileStream);
                        //成功
                    }
                    catch
                    {
                        //失败
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                }
            }
        }
        private static string PicName(string tempJPEG)
        {
            int pos = tempJPEG.LastIndexOf('/');
            tempJPEG = tempJPEG.Substring(pos + 1);
            return tempJPEG;
        }

    }

}
