using EnglishAwesomeQuizShared.Helpers;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{

    public enum QuizType
    {
        Word = 0,
        Sentences = 1,
        FillBlank = 2,
        Dialouge = 3,
        Ordering = 4
    }
}