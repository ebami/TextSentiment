using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class TranslatorService
    {
        private const string key_var = "792a503db26f4c4498c0134fef21b18e";
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable(key_var);

        private const string endpoint_var = "https://translator11111.cognitiveservices.azure.com/";
        private static readonly string endpoint = Environment.GetEnvironmentVariable(endpoint_var);

        public async Task<string> TranslateTextRequest(string subscriptionKey, string endpoint, string route, string inputText)
        {
            /*
             * The code for your call to the translation service will be added to this
             * function in the next few sections.
             */
            var sb = new StringBuilder();
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // In the next few sections you'll add code to construct the request.
                // Build the request.
                // Set the method to Post.
                request.Method = HttpMethod.Post;
                // Construct the URI and add headers.
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                // Deserialize the response using the classes created earlier.
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                // Iterate over the deserialized results.
                foreach (TranslationResult o in deserializedOutput)
                {
                    // Print the detected input language and confidence score.
                    Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                    var output = new List<string>();
                    // Iterate over the results and print each translation.
                    foreach (Translation t in o.Translations)
                    {
                        sb.AppendLine(t.Text);
                        Console.WriteLine("Translated to {0}: {1}", t.To, t.Text);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
