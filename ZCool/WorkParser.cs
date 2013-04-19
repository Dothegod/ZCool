using HtmlAgilityPack;
using MyLib.HttpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCool
{
    class WorkParser
    {
        public List<string> Parse(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return null;
            }
            List<string> ImageList = new List<string>();
            HtmlParser Parser = new HtmlParser(Content);
            HtmlNode WorkShow = Parser.GetHtmlNode("div", "class", "workShow");
            var workli = Parser.GetHtmlNodeList("div","class","wsContent");
            foreach (HtmlNode node in workli)
            {
                HtmlNode ImageNode = node.Element("img");
                if (ImageNode == null)
                {
                    continue;
                }
                string ImageSource = ImageNode.Attributes["src"].Value;
                ImageList.Add(ImageSource);
            }
            return ImageList;
        }
    }
}
