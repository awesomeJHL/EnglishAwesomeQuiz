using EnglishAwesomeQuizShared.Models;
using EnglishAwesomeQuizShared.Models.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Helpers
{
    public class QuizProvider
    {
        public static List<QuestionModel> GenerateWordQuestiion(QuizOptionModel param, WordModel data)
        {
            var words = data.Words.Where(x => x.Level == (int) param.Level).ToList();

            var rnd = new Random();
            var randomNumbers = Enumerable.Range(0, words.Count-1).OrderBy(x => rnd.Next()).Take(param.QuizCount != 0 ? param.QuizCount : QuizConstant.DEFAULT_QUIZ_COUNT).ToList();

            var questions = new List<QuestionModel>();

            //문제, 답생성
            foreach (var randomNumber in randomNumbers)
            {
                if (param.QuestionLanguageType == QuestionLanguageType.EnglishToKorean)
                {
                    questions.Add(new QuestionModel { Id = randomNumber, Question = words[randomNumber].Word, CorrectAnswer = words[randomNumber].KorWord, QuizType = param.QuizType });
                }
                else if (param.QuestionLanguageType == QuestionLanguageType.KoreanToEnglish)
                {
                    questions.Add(new QuestionModel { Id = randomNumber, Question = words[randomNumber].KorWord, CorrectAnswer = words[randomNumber].Word, QuizType = param.QuizType });
                }
                else
                {
                    throw new ArgumentException("파라메터 오류 발생");
                }
            }

            int questionId = 0;

            //보기생성
            foreach (var question in questions)
            {
                var answers = new List<string>();
                rnd = new Random();
                randomNumbers = Enumerable.Range(0, questions.Count-1).OrderBy(x => rnd.Next()).Take(5 - 1).ToList();

                //정답을 보기에 넣기
                answers.Add(question.CorrectAnswer);

                //오답을 보기에 넣기
                foreach (var randomNumber in randomNumbers)
                {
                    if (param.QuestionLanguageType == QuestionLanguageType.EnglishToKorean)
                    {
                        answers.Add(words[randomNumber].KorWord);
                    }
                    else if (param.QuestionLanguageType == QuestionLanguageType.KoreanToEnglish)
                    {
                        answers.Add(words[randomNumber].Word);
                    }
                    else
                    {
                        //TODO : 구현할것
                    }
                }

                question.Options = answers.OrderBy(x => rnd.Next()).ToList();
                question.Id = ++questionId;
            }
            PrintQuestion(questions);
            return questions;
        }

        public static void PrintQuestion(List<QuestionModel> questions)
        {
            //출력해보기
            foreach (var question in questions)
            {
                Console.WriteLine($"아이디 : {question.Id} - 문제 : {question.Question} \n 답 : {question.CorrectAnswer}");
                Console.WriteLine($"보기 : {question.Options.Aggregate((x, y) => x + ", " + y)}");
            }
        }

    }
}
