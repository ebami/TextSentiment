using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    public class Createmodel
    {
        public TextAnalysisSentiment CreateOutput(TextAnalysisSentiment textAnalysis, List<string> key, string journalDate = null)
        {
            var sb = new StringBuilder();
            foreach(var word in key)
            {
                sb.Append(word + " ");
            }

            var output = new TextAnalysisSentiment
            {
                JournalDate = DateTime.Parse(journalDate),
                DocumentSentiment = textAnalysis.DocumentSentiment,
                Negative = textAnalysis.Negative,
                Neutral = textAnalysis.Neutral,
                Positive = textAnalysis.Positive,
                Keywords = sb.ToString()
            };

            return output;
        }
    }
}
