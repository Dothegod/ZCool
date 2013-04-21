using HtmlAgilityPack;
using MyLib.HttpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZCool
{
    class Issue
    {
        public string ImageUri { get; set; }
        public string TargetUri { get; set; }
    }

    class Issues
    {
        public List<Issue> IndexSHowList = new List<Issue>();
        public List<Issue> CamList = new List<Issue>();
    }



    class HomeParser
    {
        private const string HomeUri = "http://www.zcool.com.cn/";
        public Issues Parse(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return null;
            }
            Issues issues = new Issues();
            HtmlParser parser = new HtmlParser(Content);
            ParseIndexShow(issues.IndexSHowList, parser);
            ParseImage(issues.CamList, parser);
            return issues;
        }

        private void ParseIndexShow(List<Issue> IssueList, HtmlParser parser)
        {
            try
            {
	            HtmlNode ShowNode = parser.GetHtmlNode("div", "class", "indexShowBox");
	            var ImageNodes = ShowNode.Descendants("a");
	            foreach (HtmlNode Node in ImageNodes)
	            {
	                Issue issue = new Issue();
	                string TargetUri = Node.Attributes["href"].Value;
	                if (TargetUri.Contains("/work/"))
	                {
	                    issue.TargetUri = TargetUri;
	                }
	                else
	                {
	                    continue;
	                }
	
	                issue.ImageUri = Node.Element("img").Attributes["src"].Value;
	                IssueList.Add(issue);
	            }
            }
            catch
            {
                MessageBox.Show("推荐解析错误");
            }
        }

        public List<Issue> ParseImage(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return null;
            }
            List<Issue> IssuesList = new List<Issue>();
            HtmlParser parser = new HtmlParser(Content);
            ParseImage(IssuesList, parser);
            return IssuesList;
        }
        private void ParseImage(List<Issue> IssueList, HtmlParser parser) 
        {
            try
            {
	            HtmlNode CamNode = parser.GetHtmlNode("ul", "class", "layout camWholeBoxUl");
	            var CamList = CamNode.Elements("li");
	
	            foreach (HtmlNode Node in CamList)
	            {
	                Issue issue = new Issue();
	                HtmlNode ImageNode = Node.Element("a");
	                if (ImageNode == null)
	                {
	                    continue;
	                }
	                string TargetUri = ImageNode.Attributes["href"].Value;
	                issue.TargetUri = HomeUri + TargetUri;
	                issue.ImageUri = ImageNode.Element("img").Attributes["src"].Value;
	                IssueList.Add(issue);
	            }
            }
            catch 
            {
                MessageBox.Show("内容解析错误");
            }
        }
    }
}
