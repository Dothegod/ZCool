using MyLib.HttpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCool
{
    class WorkParser
    {
        public List<string> Parser(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return null;
            }
            HtmlParser Parser = new HtmlParser(Content);


        }
    }
}
