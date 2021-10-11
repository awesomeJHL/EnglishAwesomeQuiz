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
            var words = new WordModel();
            if (param.QuestionLanguageType == QuestionLanguageType.EnglishToEnglish)
            {
                words = GetEngToEngWordList();
            }
            else
            {
                words = GetWordList();
            }

            return QuizProvider.GenerateWordQuestiion(param, words);
        }

        public List<QuestionModel> GetSentenceQuiz(QuizOptionModel param)
        {
            var words = GetWordList();
            var sentences = GetSentenceList();
            param.Blankcount = (int)param.Level;

            if(param.QuizType == QuizType.FillBlank)
            {
                return QuizProvider.GenerateFillBlankQuestion(param, sentences, words);
            }

            if (param.QuizType == QuizType.Ordering)
            {
                return QuizProvider.GenerateOrderingExtendQuestion(param, sentences, words);
            }

            throw new ArgumentException("파라메터 확인");
        }

        public WordModel GetWordList()
        {
            return JsonHelper.GetJsonFileToModel<WordModel>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Words.json"));
        }

        public WordModel GetEngToEngWordList()
        {
            return JsonHelper.GetJsonFileToModel<WordModel>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "EngToEngWords.json"));
        }

        public SentencesModel GetSentenceList()
        {
            return JsonHelper.GetJsonFileToModel<SentencesModel>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Sentences.json"));
        }
    }
}