using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class QuizResultModel
    {
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}