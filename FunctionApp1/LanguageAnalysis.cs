using Azure.AI.TextAnalytics;
using System;

namespace FunctionApp1
{
    public class LanguageAnalysis
    {
        public string LanguageDetectionExample(TextAnalyticsClient client, string text)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage(text);
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");

            return detectedLanguage.Iso6391Name.ToString();
        }
    }
}
