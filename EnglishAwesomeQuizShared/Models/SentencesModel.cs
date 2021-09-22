using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class SentencesModel
    {
        public List<Sentence> Sentences { get; set; }
    }

    public class Sentence
    {
        public string Name { get; set; }
        public string SentenceKor { get; set; }
        public string SentenceEng { get; set; }
    }
}
