﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using NunitGo.HtmlCustomElements.CSSElements;
using NunitGo.Utils;

namespace NunitGo.HtmlCustomElements.HtmlCustomElements
{
	public class Tree : HtmlBaseElement
	{
        public string HtmlCode;
        public string HtmlCodeModalWindows;
		public static string StyleString
		{
			get { return GetStyle(); }
		}
		
		private new const string Id = "tests-tree";
		private const string IdString = "#" + Id + " ";
		private static int _idSuiteCounter;

		public static string GetStyle()
		{
			var treeCssSet = new CssSet("tests-tree-style");
			treeCssSet.AddElement(new CssElement(IdString + "ul, " + IdString + "li")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Margin, "0"),
					new StyleAttribute(HtmlTextWriterStyle.Padding, "0"),
					new StyleAttribute("list-style", "none")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "input")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Position, "absolute"),
					new StyleAttribute("opacity", "0")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "a")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.TextDecoration, "none")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "a:hover")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.TextDecoration, "underline")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "input + label + ul")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Margin, "0 0 0 22px"),
					new StyleAttribute(HtmlTextWriterStyle.Display, "none")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "label, " + IdString + "label::before")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Cursor, "pointer")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "input:disabled + label")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Cursor, "default"),
					new StyleAttribute("opacity", ".6")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "input:checked:not(:disabled) + label + ul")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Display, "block")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "label, " + IdString + "a, " + IdString + "label::before")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute(HtmlTextWriterStyle.Display, "block")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "label")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute("background-position", "18px 0")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "label::before")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute("content", "\"\""),
					new StyleAttribute(HtmlTextWriterStyle.Width, "16px"),
					new StyleAttribute(HtmlTextWriterStyle.Margin, "0 22px 0 0"),
					new StyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle"),
					new StyleAttribute("background-position", "0 -32px")
				}
			});
			treeCssSet.AddElement(new CssElement(IdString + "input:checked + label::before")
			{
				StyleFields = new List<StyleAttribute>
				{
					new StyleAttribute("background-position", "0 -16px")
				}
			});
			return treeCssSet.ToString();
		}

		private static string GetSuiteId()
		{
			_idSuiteCounter++;
			return "test-suite-" + _idSuiteCounter.ToString("D");
		}

        private void BuildTree(HtmlTextWriter writer, List<NunitGoTest> tests)
        {
            var id = GetSuiteId();
            var count = tests.Count();
            var passedCount = tests.Count(x => x.IsSuccess());
            var start = tests.First().DateTimeStart.ToString("dd.MM.yy HH:mm:ss");
            var end = tests.Last().DateTimeFinish.ToString("dd.MM.yy HH:mm:ss");
            var labelName = "All tests: " + passedCount + @"/" + count + " " + start + " - " + end;
            writer.OpenTreeItem(labelName, id, "110%");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            
            foreach (var nunitGoTest in tests)
            {
                var testId = Ids.GetTestId(nunitGoTest.Guid.ToString());
                var test = new NunitTestHtml(nunitGoTest);
                var modalId = Ids.GetTestModalId(nunitGoTest.Guid.ToString());
                var modalWindow = new ModalWindow(modalId, test.HtmlCode, 1004, 90);
                var openButton = new JsOpenButton(nunitGoTest.FullName
                    + " " + nunitGoTest.DateTimeStart.ToString("dd.MM.yy HH:mm:ss") + " - " +
                    nunitGoTest.DateTimeFinish.ToString("dd.MM.yy HH:mm:ss"),
                    modalId, modalWindow.BackgroundId, test.BackgroundColor);

                writer.AddAttribute(HtmlTextWriterAttribute.Id, testId);
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, nunitGoTest.FullName);
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                HtmlCodeModalWindows += Environment.NewLine + modalWindow.ModalWindowHtml;
                HtmlCodeModalWindows += Environment.NewLine + test.ModalWindowsHtml;

                writer.Write(openButton.ButtonHtml);
                writer.RenderEndTag(); //A
                writer.RenderEndTag(); //LI

            }
            writer.RenderEndTag(); //UL
            writer.RenderEndTag(); //LI
            writer.RenderEndTag(); //UL
        }

        private void BuildTreeFromSuites(HtmlTextWriter writer, List<NunitGoSuite> suites)
        {
            foreach (var suite in suites)
            {
                Log.Write("A T: " + suite.Tests.Count + ", S: " + suites.Count + ", " + suite.Name);
                var tests = suite.Tests;
                var id = GetSuiteId();
                Log.Write("B T: " + suite.Tests.Count + ", S: " + suites.Count + ", " + suite.Name);
                var allSuiteTests = suite.GetTests();
                Log.Write("C T: " + suite.Tests.Count + ", S: " + suites.Count + ", " + suite.Name);
                var count = allSuiteTests.Count();
                var passedCount = allSuiteTests.Count(x => x.IsSuccess());
                var labelName = suite.Name + " (Tests: " + passedCount + @"/" + count + ")";
                writer.OpenTreeItem(labelName, id, "110%", suite.Tests.Count.Equals(0));
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                Log.Write("D T: " + suite.Tests.Count + ", S: " + suites.Count + ", " + suite.Name);

                foreach (var nunitGoTest in tests)
                {
                    var testId = Ids.GetTestId(nunitGoTest.Guid.ToString());
                    var testHtml = new NunitTestHtml(nunitGoTest);
                    var modalId = Ids.GetTestModalId(nunitGoTest.Guid.ToString());
                    var modalWindow = new ModalWindow(modalId, testHtml.HtmlCode, 1004, 90);
                    var openButton = new JsOpenButton(nunitGoTest.FullName
                        + " " + nunitGoTest.DateTimeStart.ToString("dd.MM.yy HH:mm:ss") + " - " +
                        nunitGoTest.DateTimeFinish.ToString("dd.MM.yy HH:mm:ss"),
                        modalId, modalWindow.BackgroundId, testHtml.BackgroundColor);

                    writer.AddAttribute(HtmlTextWriterAttribute.Id, testId);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, nunitGoTest.FullName);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);

                    HtmlCodeModalWindows += Environment.NewLine + modalWindow.ModalWindowHtml;
                    HtmlCodeModalWindows += Environment.NewLine + testHtml.ModalWindowsHtml;

                    writer.Write(openButton.ButtonHtml);
                    writer.RenderEndTag(); //A
                    writer.RenderEndTag(); //LI

                }
                if (suite.Suites.Any())
                {
                    BuildTreeFromSuites(writer, suite.Suites);
                }
                writer.RenderEndTag(); //UL
                writer.RenderEndTag(); //LI
                writer.RenderEndTag(); //UL
            }
        }

        public Tree(List<NunitGoTest> tests)
		{
			_idSuiteCounter = 0;
			Style = GetStyle();

			var strWr = new StringWriter();
			using (var writer = new HtmlTextWriter(strWr))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, Id);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
                BuildTreeFromSuites(writer, new List<NunitGoSuite> {tests.GetSuite("All tests")});
                writer.RenderEndTag(); //DIV
			}

			HtmlCode = strWr.ToString();
		}
	}
}
