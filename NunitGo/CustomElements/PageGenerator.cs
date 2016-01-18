﻿using System.Collections.Generic;
using System.IO;
using NunitGo.CustomElements.CSSElements;
using NunitGo.CustomElements.HtmlCustomElements;
using NunitGo.CustomElements.ReportSections;
using NunitGo.Utils;

namespace NunitGo.CustomElements
{
	public static class PageGenerator
    {
        public static void GenerateTestPage(this NunitGoTest nunitGoTest, string fullPath)
        {
            var page = new HtmlPage("Test page", "./../../" + Output.Outputs.ReportStyle);
            var htmlTest = new NunitTestHtml(nunitGoTest);
            page.AddToBody(htmlTest.HtmlCode);

            page.SavePage(fullPath);
        }

        public static void GenerateTestOutputPage(string fullPath, string outputText, string backHref)
        {
            var page = new HtmlPage("Output page", "./../../" + Output.Outputs.ReportStyle);

            var reportMenuTitle = new PageTitle("Test output", "test-output", "10%");
            page.AddToBody(reportMenuTitle.HtmlCode);

            var outputSection = new TestOutputSection(outputText, backHref);
            page.AddToBody(outputSection.HtmlCode);
            
            page.SavePage(fullPath);
        }

        public static void GenerateMainStatisticsPage(this MainStatistics stats, string fullPath)
        {
            var page = new HtmlPage("Main statistics page");

            var reportMenuTitle = new PageTitle("Main statistics", "main-statistics", "10%");
            page.AddToBody(reportMenuTitle.HtmlCode);

            var statisticsSection = new StatisticsSection(stats);
            page.AddToBody(statisticsSection.HtmlCode);

            page.SavePage(fullPath);
        }

        public static void GenerateTestListPage(this List<NunitGoTest> tests, string fullPath)
        {
            var page = new HtmlPage("Test list page");

            var reportMenuTitle = new PageTitle("Test list", "main-test-list", "10%");
            page.AddToBody(reportMenuTitle.HtmlCode);

            var testListSection = new TestListSection(tests);
            page.AddToBody(testListSection.HtmlCode);

            page.SavePage(fullPath);
        }

        public static void GenerateTimelinePage(this List<NunitGoTest> tests, string fullPath)
        {
            var page = new HtmlPage("Timeline page");

            var reportMenuTitle = new PageTitle("Tests timeline", "tests-timeline", "10%");
            page.AddToBody(reportMenuTitle.HtmlCode);

            var timeline = new TimelineSection(tests);
            page.AddToBody(timeline.HtmlCode);

            page.SavePage(fullPath);
        }

	    public static void GenerateStyleFile(string pathToSave)
	    {
            var cssPage = new CssPage();
            cssPage.AddStyles(new List<string>
            {
                PageTitle.StyleString,
                HtmlPage.StyleString,
                Tooltip.StyleString,
                HorizontalBar.StyleString,
                ReportFooter.StyleString,
                MainInformation.StyleString,
                Bullet.StyleString,
                HrefButtonBase.StyleString,
                Tree.StyleString,
                NunitTestHtml.StyleString,
                Accordion.StyleString,
                ReportMenu.StyleString,
                OpenButton.StyleString
            });

            var cssPageName = Output.Outputs.ReportStyle;
            cssPage.SavePage(Path.Combine(pathToSave, cssPageName));
	    }

	    public static void GenerateReport(this List<NunitGoTest> tests, 
            string pathToSave, MainStatistics mainStats)
        {
            var report = new HtmlPage();
            
			var mainTitle = new PageTitle();
			report.AddToBody(mainTitle.HtmlCode);
            
			var mainInformation = new MainInformation(mainStats);
			report.AddToBody(mainInformation.HtmlCode);

			var reportMenuTitle = new PageTitle("Report menu", "report-main-menu");
			report.AddToBody(reportMenuTitle.HtmlCode);
            
            var menuElements = new List<ReportMenuItem>
			{
				new ReportMenuItem("Main statistics", Output.Outputs.TestStatistics),
				new ReportMenuItem("Test list", Output.Outputs.TestList),
				new ReportMenuItem("Timeline", Output.Outputs.Timeline)
			};
            var reportMenu = new ReportMenu(menuElements, "main-menu", "Main Menu");
            report.AddToBody(reportMenu.ReportMenuHtml);

			var footer = new ReportFooter();
            report.AddInsideTag("footer", footer.HtmlCode);
            
            var reportPageName = Output.Outputs.FullReport;
			report.SavePage(Path.Combine(pathToSave, reportPageName));
		}
	}
}