using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    public class TranslateText
    {
        private const string key_var = "TRANSLATOR_TEXT_SUBSCRIPTION_KEY";
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable(key_var);

        private const string endpoint_var = "TRANSLATOR_TEXT_ENDPOINT";
        private static readonly string endpoint = Environment.GetEnvironmentVariable(endpoint_var);

        public static string Translate(string text, string language)
        {
            string bodyText;
            string route = $"/translate?api-version=3.0&to={language}";
            TranslatorService translate = new TranslatorService();
            bodyText = translate.TranslateTextRequest(subscriptionKey, "https://api-eur.cognitive.microsofttranslator.com", route, text).Result;
            return bodyText;
        }
    }
}
