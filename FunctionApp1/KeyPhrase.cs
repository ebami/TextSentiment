using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class KeyPhrase
    {
        public List<string> KeyPhraseExtractionExample(TextAnalyticsClient client, string text)
        {
            string document = text.Replace("\r", "").Replace("\n", "");

            var response = client.ExtractKeyPhrases(document);

            // Printing key phrases
            Console.WriteLine("Key phrases:");
            var phrases = new List<string>();
            foreach (string keyphrase in response.Value)
            {
                phrases.Add(keyphrase);
                Console.WriteLine($"\t{keyphrase}");
            }

            return phrases;
        }
    }
}
