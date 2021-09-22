using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class WordModel
    {
        public List<Words> Words { get; set; }
    }

    public class Words
    {
        public string Word { get; set; }
        public string KorWord { get; set; }
        public string[] OrderKorWords { get; set; }
        public string[] ExampleSentences { get; set; }
    }
}
