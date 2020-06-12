using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FunctionApp1
{
    public class PublishResults
    {
        public static async void Publish(List<TextAnalysisSentiment> results)
        {
            var requestBody2 = @"";
            var requestBody = JsonConvert.SerializeObject(results);
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                string connString = "https://api.powerbi.com/beta/c15e3385-50db-465c-8073-60e21b75f74c/datasets/ab1f2cb4-7f9c-4ff8-bc50-5bbb0a7a7f2e/rows?noSignUpCheck=1&key=glmZzUBqBNRZ7fMdhsDSG2B%2BEYd%2BSf%2BYFPQqkMAlt%2BBKpuv0Lcc5u2VTfJNjhUcoWv9QCrGalJLwQAf2VDBWPg%3D%3D";
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(connString);
                request.Content = new StringContent(requestBody);

                //dynamic data = await requestBody.Content.ReadAsAsync<object>();
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();

                //HttpContent content = new StringContent("[" + requestBody + "]");

                //HttpResponseMessage response2 = await client.PostAsync(connString, content);
                //string result2 = await response2.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
            }

        }
    }
}
