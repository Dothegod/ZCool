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
using System.Collections.Generic;
using HtmlAgilityPack;

namespace MyLib.HttpLib
{
    public class HtmlParser
    {
        string m_Content;
//         HtmlDocument Doc = new HtmlDocument();
        HtmlDocument Doc = new HtmlDocument();
        public HtmlParser(string Content)
        {
            m_Content = Content;
//             m_Content = m_Content.Replace("<br/>", "\n");
            m_Content = System.Net.HttpUtility.HtmlDecode(m_Content);
            Doc.LoadHtml(m_Content);
        }
        public List<string> GetHtmlValueList(HtmlNode Node, string Element, string Property, string Value)
        {
            List<string> ValueList = new List<string>();

            var NodeList = Node.Descendants(Element);
            foreach (HtmlNode node in NodeList)
            {
                if (null == node.Attributes[Property])
                {
                    continue;
                }
                if (Value == node.Attributes[Property].Value)
                {
                    ValueList.Add(node.InnerText);
                }
            }
            return ValueList;
        }
        public List<string> GetHtmlValueList(string Element, string Property, string Value)
        {
            return GetHtmlValueList(Doc.DocumentNode, Element, Property, Value);
        }
        public string GetHtmlValue(HtmlNode Node, string Element, string Property, string Value)
        {
            List<string> ValueList;

            ValueList = GetHtmlValueList(Node, Element, Property, Value);
            if (0 != ValueList.Count)
            {
                return ValueList[0];
            }
            return null;
        }

        public string GetHtmlValue(string Element, string Property, string Value)
        {
            return GetHtmlValue(Doc.DocumentNode, Element, Property, Value);
        }

        public string GetHtmlAttribute(HtmlNode Node, string Element, string Property, string Value, string Attribute)
        {
            var NodeList = Node.Descendants(Element);
            foreach (HtmlNode node in NodeList)
            {
                if (null == node.Attributes[Property])
                {
                    continue;
                }
                if (Value == node.Attributes[Property].Value)
                {
                    return node.Attributes[Attribute].Value;
                }
            }
            return null;
        }

        public string GetHtmlAttribute(HtmlNode Node, string Element, string Attribute)
        {
            var NodeList = Node.Descendants(Element);
            foreach (HtmlNode node in NodeList)
            {
                if (null == node.Attributes[Attribute])
                {
                    continue;
                }
                return node.Attributes[Attribute].Value;
            }
            return null;
        }

        public string GetHtmlAttribute(string Element, string Property, string Value, string Attribute)
        {
            return GetHtmlAttribute(Doc.DocumentNode, Element, Property, Value, Attribute);
        }

        public List<HtmlNode> GetHtmlNodeList(HtmlNode Node, string Element, string Property, string Value)
        {
            List<HtmlNode> ValueList = new List<HtmlNode>();

            var NodeList = Node.Descendants(Element);
            foreach (HtmlNode node in NodeList)
            {
                if (null == node.Attributes[Property])
                {
                    continue;
                }
                if (Value == node.Attributes[Property].Value)
                {
                    ValueList.Add(node);
                }
            }
            return ValueList;
        }
        public List<HtmlNode> GetHtmlNodeList(string Element, string Property, string Value)
        {
            return GetHtmlNodeList(Doc.DocumentNode, Element, Property, Value);
        }
        public HtmlNode GetHtmlNode(string Element, string Property, string Value)
        {
            return GetHtmlNode(Doc.DocumentNode, Element, Property, Value);
        }

        public HtmlNode GetHtmlNode(HtmlNode Node, string Element, string Property, string Value)
        {
            List<HtmlNode> ValueList = new List<HtmlNode>();

            var NodeList = Node.Descendants(Element);
            foreach (HtmlNode node in NodeList)
            {
                if (null == node.Attributes[Property])
                {
                    continue;
                }
                if (Value == node.Attributes[Property].Value)
                {
                    return node;
                }
            }
            return null;
        }
        
    }
}
