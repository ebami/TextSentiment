using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Text;
using Azure.AI.TextAnalytics;
using Azure;
using Microsoft.Toolkit.Extensions;
using System.Collections.Generic;
using ClassLibrary1;
using System.Diagnostics;

namespace FunctionApp1
{
    public static class HttpExample
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("688760826e6e49d69615234a4780364b");
        private static readonly Uri endpoint = new Uri("https://carertextana.cognitiveservices.azure.com/");
        private static int MaxLength = 5120;

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("outqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
            ILogger log)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            var urls = DataUrls.URLS;
            List<TextAnalysisSentiment> entries = new List<TextAnalysisSentiment>();
            for (var i = 0; i < urls.Count; i++)
            {
                Console.WriteLine(urls[i]);
                LanguageAnalysis lang = new LanguageAnalysis();
                TextScraper textScraper = new TextScraper();
                Createmodel cm = new Createmodel();
                KeyPhrase keyPhrase = new KeyPhrase();
                var text = textScraper.GetScrapeResultsAsync(urls[i]).Result;

                SentimentAnalysis tas = new SentimentAnalysis();
                var bodyText = text.JournalBody.Length > MaxLength 
                    ? text.JournalBody.Truncate(MaxLength) 
                    : text.JournalBody;
                var language = lang.LanguageDetectionExample(client, bodyText);
                if(language != "en")
                {
                    try
                    {
                        language = "en";
                        bodyText = TranslateText.Translate(bodyText, language);
                    }
                    catch (Exception e)
                    {
                        bodyText = "bad document";
                        Debug.WriteLine("Exception: " + e);
                    }
                }
                var outs = tas.SentimentAnalysisExampleAsync(client, language, bodyText);
                var keywords = keyPhrase.KeyPhraseExtractionExample(client, bodyText);
                
                string journalDate = string.IsNullOrEmpty(text.JournalDate)
                                    ? RandomDay().ToString() : text.JournalDate;
                if(journalDate == text.JournalDate)
                {
                    journalDate = RandomDay().ToString();
                }
                var seri = cm.CreateOutput(outs, keywords,
                    journalDate);
                entries.Add(seri);
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (!string.IsNullOrEmpty(name))
            {
                // Add a message to the output collection.
                msg.Add(string.Format("Name passed to the function: {0}", name));
            }
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            PublishResults.Publish(entries);
            return new OkObjectResult(entries);
        }

        private static Random gen = new Random();

        static DateTime RandomDay()
        {
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
