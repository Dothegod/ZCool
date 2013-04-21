using HtmlAgilityPack;
using MyLib.HttpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZCool
{
    class WorkInfo
    {
        public List<string> ImageList = new List<string>();
        public int PageCount;
    }
    class WorkParser
    {
        public WorkInfo Parse(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return null;
            }
            WorkInfo info = new WorkInfo();
            try
            {
	            List<string> ImageList = info.ImageList;
	            HtmlParser Parser = new HtmlParser(Content);
	            HtmlNode WorkShow = Parser.GetHtmlNode("div", "class", "workShow");
	            var workli = Parser.GetHtmlNodeList("div","class","wsContent");
	            foreach (HtmlNode node in workli)
	            {
                    var Nodes = node.Descendants("img");
                    if (Nodes.Count() == 0)
                    {
                        continue;
                    }
                    HtmlNode ImageNode = Nodes.First();
	                if (ImageNode == null)
	                {
	                    continue;
	                }
	                string ImageSource = ImageNode.Attributes["src"].Value;
	                ImageList.Add(ImageSource);
	            }
	            HtmlNode Page = Parser.GetHtmlNode("div", "class", "bigPage pt30 pb20 vm center");
	            if (Page != null)
	            {
	                info.PageCount = Page.Elements("a").Count() - 2;
	            }
            }
            catch
            {
                MessageBox.Show("作品解析错误");
            }
            return info;
        }
    }
}
