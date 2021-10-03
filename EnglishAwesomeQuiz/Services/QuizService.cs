using EnglishAwesomeQuiz.Helpers;
using EnglishAwesomeQuiz.Interfaces;
using EnglishAwesomeQuizShared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Services
{
    public class QuizService : IQuizService
    {
        public List<QuestionModel> GetWordQuiz(QuizOptionModel param)
        {
            var words = GetWordList();
            return QuizProvider.GenerateWordQuestiion(param, words);
        }

        public WordModel GetWordList()
        {
            return JsonHelper.GetJsonFileToModel<WordModel>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Words.json"));
        }
    }
}
