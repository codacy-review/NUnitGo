﻿using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using NunitGo.CustomElements.CSSElements;

namespace NunitGo.CustomElements.HtmlCustomElements
{
    public class PageTitle : HtmlBaseElement
    {
        public static string ClassName;
        public static string Height;

        public static string StyleString
        {
            get { return GetStyle(); }
        }

        public string HtmlCode
        {
            get { return GetCode(); }
        }

        public PageTitle(string title = "Test Run Report", string id = "main-title", string height = "")
        {
            Title = title;
            Id = id;
            Style = GetStyle();
            Height = height;
        }

        private static string GetStyle()
        {
            var titleCssSet = new CssSet("title-style");
            titleCssSet.AddElement(new CssElement(".report-title")
            {
                StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Width, "90%"),
					new StyleAttribute(HtmlTextWriterStyle.Display, "inline-block"),
					new StyleAttribute(HtmlTextWriterStyle.FontSize, "30px"),
					new StyleAttribute(HtmlTextWriterStyle.PaddingTop, "30px"),
					new StyleAttribute(HtmlTextWriterStyle.PaddingBottom, "30px")
				}
            });
            return titleCssSet.ToString();
        }

        private string GetCode()
        {
            var stringWriter = new StringWriter();
            using (var writer = new HtmlTextWriter(stringWriter))
            {
                if(!Height.Equals(""))
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height);
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, Id);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "report-title");
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(Title);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }
    }
}
