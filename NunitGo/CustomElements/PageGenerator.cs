﻿using System;
using System.Collections.Generic;
using System.IO;
using NunitGo.CustomElements.CSSElements;
using NunitGo.CustomElements.HtmlCustomElements;
using NunitGo.CustomElements.ReportSections;
using NunitGo.NunitGoItems;
using NunitGo.Utils;

namespace NunitGo.CustomElements
{
	internal static class PageGenerator
    {
        public static string GenerateTestPage(this NunitGoTest nunitGoTest, string fullPath)
        {
            try
            {
                var page = new HtmlPage("Test page", "./../../" + Output.Files.ReportStyleFile);
                var htmlTest = new NunitTestHtml(nunitGoTest);
                page.AddToBody(htmlTest.HtmlCode);

                page.SavePage(fullPath);
                return page.GetFullPage();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating test html page");
            }
            return "";
        }

        public static void GenerateTestOutputPage(string fullPath, string outputText, string backHref)
        {
            try
            {
                var page = new HtmlPage("Output page", "./../../" + Output.Files.ReportStyleFile);

                var reportMenuTitle = new PageTitle("Test output", "test-output", "10%");
                page.AddToBody(reportMenuTitle.HtmlCode);

                var outputSection = new TestOutputSection(outputText, backHref);
                page.AddToBody(outputSection.HtmlCode);

                page.SavePage(fullPath);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating test output page");
            }
        }

        public static void GenerateMainStatisticsPage(this MainStatistics stats, string fullPath)
        {
            try
            {
                var page = new HtmlPage("Main statistics page");

                var reportMenuTitle = new PageTitle("Main statistics", "main-statistics", "10%");
                page.AddToBody(reportMenuTitle.HtmlCode);

                var statisticsSection = new StatisticsSection(stats);
                page.AddToBody(statisticsSection.HtmlCode);

                page.SavePage(fullPath);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating main statistics page");
            }
        }

        public static void GenerateTestListPage(this List<NunitGoTest> tests, string fullPath)
        {
            try
            {
                var page = new HtmlPage("Test list page");

                var reportMenuTitle = new PageTitle("Test list", "main-test-list", "10%");
                page.AddToBody(reportMenuTitle.HtmlCode);

                var testListSection = new TestListSection(tests);
                page.AddToBody(testListSection.HtmlCode);

                page.SavePage(fullPath);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating test list page");
            }
        }

        public static void GenerateTimelinePage(this List<NunitGoTest> tests, string fullPath)
        {
            try
            {
                var page = new HtmlPage("Timeline page");

                var reportMenuTitle = new PageTitle("Tests timeline", "tests-timeline", "10%");
                page.AddToBody(reportMenuTitle.HtmlCode);

                var timeline = new TimelineSection(tests);
                page.AddToBody(timeline.HtmlCode);

                page.SavePage(fullPath);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating timeline page");
            }
        }

	    public static void GenerateStyleFile(string pathToSave)
        {
            try
            {
                var cssPage = new CssPage();
                cssPage.AddStyles(new List<string>
                {
                    PageTitle.StyleString,
                    HtmlPage.StyleString,
                    Tooltip.StyleString,
                    HorizontalBar.StyleString,
                    FooterSection.StyleString,
                    MainInformationSection.StyleString,
                    Bullet.StyleString,
                    HrefButtonBase.StyleString,
                    Tree.StyleString,
                    NunitTestHtml.StyleString,
                    MenuSection.StyleString,
                    OpenButton.StyleString
                });

                var cssPageName = Output.Files.ReportStyleFile;
                cssPage.SavePage(Path.Combine(pathToSave, cssPageName));
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating css style page");
            }
	    }

	    public static void GenerateReportMainPage(this List<NunitGoTest> tests, 
            string pathToSave, MainStatistics mainStats)
        {
            try
            {
                var report = new HtmlPage();

                var mainTitle = new PageTitle();
                report.AddToBody(mainTitle.HtmlCode);

                var mainInformation = new MainInformationSection(mainStats);
                report.AddToBody(mainInformation.HtmlCode);

                var reportMenuTitle = new PageTitle("Report menu", "report-main-menu");
                report.AddToBody(reportMenuTitle.HtmlCode);

                var menuElements = new List<ReportMenuItem>
			    {
				    new ReportMenuItem("Main statistics", Output.Files.TestStatisticsFile),
				    new ReportMenuItem("Test list", Output.Files.TestListFile),
				    new ReportMenuItem("Timeline", Output.Files.TimelineFile)
			    };
                var reportMenu = new MenuSection(menuElements, "main-menu", "Main Menu");
                report.AddToBody(reportMenu.ReportMenuHtml);

                var footer = new FooterSection();
                report.AddInsideTag("footer", footer.HtmlCode);

                var reportPageName = Output.Files.FullReportFile;
                report.SavePage(Path.Combine(pathToSave, reportPageName));
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while generating full report page");
            }
		}
	}
}