using Azure.AI.TextAnalytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class SentimentAnalysis
    {
        public TextAnalysisSentiment SentimentAnalysisExampleAsync(TextAnalyticsClient client, string language, string text)
        {
            //string inputText = "I had the best day of my life. I wish you were there with me. I keep forgetting things. I am frustrated.";
            string document = text.Replace("\r", "").Replace("\n", "");
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(document, language);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");
            double sumPostitive = 0;
            double sumNegative = 0;
            double sumNeutral = 0;
            int countPositive = 0, countNegative = 0, countNeutral = 0;
            double negSumScore = 0, posSumScore = 0, neuSumScore = 0;

            var noOfSentences = documentSentiment.Sentences.Count;

            foreach (var sentence in documentSentiment.Sentences)
            {

                Console.WriteLine($"\tText: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");

                switch (sentence.Sentiment.ToString())
                {
                    case "Negative":
                        countNegative += 1;
                        negSumScore += sentence.ConfidenceScores.Negative;
                        break;
                    case "Positive":
                        countPositive += 1;
                        posSumScore += sentence.ConfidenceScores.Positive;
                        break;
                    case "Neutral":
                        countNeutral += 1;
                        neuSumScore += sentence.ConfidenceScores.Neutral;
                        break;

                }
                sumPostitive += sentence.ConfidenceScores.Positive;

                sumNegative += sentence.ConfidenceScores.Negative;

                sumNeutral += sentence.ConfidenceScores.Neutral;
            }

            double avePos = sumPostitive / noOfSentences;
            Console.WriteLine($"\tAverage score Positive: {avePos}");
            double aveNeg = sumNegative / noOfSentences;
            Console.WriteLine($"\tAverage score Negative: {aveNeg}");
            double aveNeu = sumNeutral / noOfSentences;
            Console.WriteLine($"\tAverage score Neutral: {aveNeu}");

            double v = posSumScore / countPositive;
            Console.WriteLine($"\tAverage score: {v}");
            double v1 = negSumScore / countNegative;
            Console.WriteLine($"\tAverage score: {v1}");
            double v2 = neuSumScore / countNeutral;
            Console.WriteLine($"\tAverage score: {v2}");

            TextAnalysisSentiment output = new TextAnalysisSentiment();
            output.DocumentSentiment = documentSentiment.Sentiment.ToString();
            output.Negative = aveNeg;
            output.Positive = avePos;
            output.Neutral = aveNeu;

            return output;
        }
    }
}
