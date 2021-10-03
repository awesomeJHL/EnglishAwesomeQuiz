using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class QuizOptionModel
    {
        public QuestionLanguageType QuestionLanguageType { get; set; }
        
        public int Blank { get; set; }

        public int Choice { get; set; }

        public int QuizCount { get; set; }

        public  QuizLevel Level { get; set; }

        public QuizType QuizType { get; set; }
    }
}
