using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class TextScraper
    {
        public async Task<JournalEntry> GetScrapeResultsAsync(string query = null)
        {
            var htmlWeb = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            if (string.IsNullOrEmpty(query))
                query = $"https://www.alzheimers.org.uk/dementia-together-magazine-febmar-20/ar-yr-un-donfedd-dementia-connect";

            try
            {
                doc = await htmlWeb.LoadFromWebAsync(query);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            var text = doc.DocumentNode.SelectSingleNode("//*[@id='system-main-block']");

            var date = doc.DocumentNode.SelectSingleNode("//*[@class='field--node-post-date-optional field field-name-node-post-date-optional']");
            var sb = new StringBuilder();
            var resu = text.SelectNodes("//*[@id='system-main-block']/div/div[5]");

            foreach (var node in resu[0].DescendantsAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    string texts = node.InnerText;
                    if (!string.IsNullOrEmpty(texts))
                        sb.AppendLine(texts.Trim());
                }
            }

            var journalBody = sb.ToString();
            var model = new JournalEntry
            {
                JournalBody = CleanBody(journalBody),
                JournalDate = date.InnerText.ToString().Trim()
            };

            return model;
            //var text = client.DownloadString("https://www.alzheimers.org.uk/blog/mom-seems-afraid-if-she-doesnt-write-things-down-she-will-forget");
            ////*[@id="system-main-block"]
            //
            //var results = text.SelectNodes("//div[@class='b_algo']");
            //var bodytext = doc.DocumentNode.SelectSingleNode("//*[@id='system-main-block']/div/div[5]");
            //var bodytext2 = doc.DocumentNode.SelectSingleNode("//*[@class='node--article--content']");
            //var date = doc.DocumentNode.SelectSingleNode("//*[@id='system-main-block']/div/div[4]/div[1]/div/div/div/div[1]/div/div/div/div/div[1]");

        }

        private static string CleanBody(string journalBody)
        {
            string cleantext = journalBody.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
            return cleantext;
        }
    }
}
