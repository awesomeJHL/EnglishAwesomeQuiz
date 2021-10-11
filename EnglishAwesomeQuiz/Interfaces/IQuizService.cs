using EnglishAwesomeQuizShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Interfaces
{
    public interface IQuizService
    {
        public List<QuestionModel> GetWordQuiz(QuizOptionModel param);
        public List<QuestionModel> GetSentenceQuiz(QuizOptionModel param);
    }
}
