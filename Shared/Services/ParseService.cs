using HtmlAgilityPack;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class ParseService
    {
        private const string LAST_EDITED_PAGES_URL = "http://scp-ukrainian.wikidot.com/most-recently-edited";
        private const string NEW_PAGES_URL = "http://scp-ukrainian.wikidot.com/most-recently-created";

        private HtmlWeb webPage;

        public ParseService()
        {
            webPage = new HtmlWeb();
        }

        public async Task<RecentlyCreatedPage> GetNewPages()
        {
            HtmlDocument document = await webPage.LoadFromWebAsync(NEW_PAGES_URL);

            var newsTable = document.DocumentNode.SelectSingleNode("//table[contains(@class, 'wiki-content-table')]");

            var firstRecord = newsTable.ChildNodes.Where(x => x.Name == "tr").ToList()[1].ChildNodes.Where(x => x.Name == "td").ToList();

            return new RecentlyCreatedPage()
            {
                Title = firstRecord[0].InnerText,
                PageUrl = TextParseService.BuildUrl(firstRecord[0].FirstChild.Attributes.First().Value),
                CreationTime = TextParseService.GetCorrectDateTime(firstRecord[3].FirstChild.InnerText),
                Author = new PageAuthor()
                {
                    Name = firstRecord[2].FirstChild.FirstChild.InnerText,
                    Url = firstRecord[2].FirstChild.FirstChild.Attributes.First().Value
                }
            };
        }

        public UpdatesPage GetLatestEdited()
        {
            var general = ParseLastEdited().Result;
            return general;
        }

        /*
        private async Task<UpdatedPage> ParseDetailedLastEdited()
        {
            UpdatedPage updatedPage = new UpdatedPage();
            List<HtmlNode> tables = new List<HtmlNode>();
            string url = "http://scpsandbox-ua.wikidot.com/system:recent-changes";
            var web = new HtmlAgilityPack.HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(url);

            var result = doc.GetElementbyId("site-changes-list").SelectSingleNode("//div[contains(@class, 'changes-list-item')]");
            var item = result.SelectSingleNode("//td[contains(@class, 'title')]").SelectSingleNode("a");

            updatedPage.Title = CleanDetailedTitle(result.SelectSingleNode("//td[contains(@class, 'title')]").SelectSingleNode("a").InnerText);
            updatedPage.Author = new PageAuthor()
            {
                Name = result.SelectSingleNode("//td[contains(@class, 'mod-by')]").SelectSingleNode("span").FirstChild.InnerText,
                Url = result.SelectSingleNode("//td[contains(@class, 'mod-by')]").SelectSingleNode("span")
                                                                                 .SelectSingleNode("a").Attributes.First().Value
            };
            updatedPage.UpdateTime = GetCorrectDateTime(result.SelectSingleNode("//td[contains(@class, 'mod-date')]").SelectSingleNode("span").InnerText);
            updatedPage.PageUrl = BuildUrl(result.SelectSingleNode("//td[contains(@class, 'title')]").SelectSingleNode("a").Attributes.First().Value);

            return updatedPage;
        }
        */

        private async Task<UpdatesPage> ParseLastEdited()
        {
            List<UpdatesPage> updatedPages = new List<UpdatesPage>();
            List<HtmlNode> tables = new List<HtmlNode>();

            HtmlDocument doc = await webPage.LoadFromWebAsync(LAST_EDITED_PAGES_URL);

            var result = doc.GetElementbyId("page-content").SelectSingleNode("div");

            var lastUpdatedItem = result.SelectSingleNode("div");

            var rawValue = lastUpdatedItem.SelectSingleNode("p");

                UpdatesPage page = new UpdatesPage()
                {
                    Title = TextParseService.ParseTitle(rawValue.SelectSingleNode("a").InnerText),
                    Author = new PageAuthor() { Name = TextParseService.CleanGeneralAuthorName(rawValue.ChildNodes.Where(n => n.NextSibling.Name == "span").First().InnerText), Url = string.Empty },
                    UpdateTime = TextParseService.GetCorrectDateTime(rawValue.SelectSingleNode("span").InnerText),
                    PageUrl = TextParseService.BuildUrl(rawValue.SelectSingleNode("a").Attributes.First().Value)
                };

                updatedPages.Add(page);

            return updatedPages.OrderByDescending(x => x.UpdateTime).First();
        }
    }
}
