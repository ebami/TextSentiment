using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    public class TextAnalysisSentiment
    {
        public DateTime JournalDate { get; set; }

        public double Neutral { get; set; }
        
        public double Positive { get; set; }
        
        public double Negative { get; set; }
        
        public string DocumentSentiment { get; set; }

        public string Keywords { get; set; }
    }
}
