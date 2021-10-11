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
            //TODO : 데이터를 만들기 어려우므로 영>영 문제는 LEVEL 적용하지 않겠음
            var words = data.Words.Where(x => x.Level == (param.QuestionLanguageType == QuestionLanguageType.EnglishToEnglish ? x.Level : (int) param.Level)).ToList();

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
                else if (param.QuestionLanguageType == QuestionLanguageType.EnglishToEnglish)
                {
                    questions.Add(new QuestionModel { Id = randomNumber, Question = words[randomNumber].Meaning, CorrectAnswer = words[randomNumber].Word });
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
                randomNumbers = Enumerable.Range(0, words.Count - 1).OrderBy(x => rnd.Next()).Take(5 - 1).ToList();

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
                    else if (param.QuestionLanguageType == QuestionLanguageType.EnglishToEnglish)
                    {
                        answers.Add(words[randomNumber].Word);
                    }
                    else
                    {
                        throw new ArgumentException("파라메터 오류");
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

        public static List<QuestionModel> GenerateFillBlankQuestion(QuizOptionModel optionModel, SentencesModel sentencesModel, WordModel wordModel)
        {
            var sentences = new List<Sentence>();
            var optionCount = 0;

            if (optionModel.Level == QuizLevel.L)
            {
                sentences = sentencesModel.Sentences.Where(x => x.SentenceEng.Length < 30).ToList();
                optionCount = 3;
            }
            else if (optionModel.Level == QuizLevel.M)
            {
                sentences = sentencesModel.Sentences.Where(x => x.SentenceEng.Length < 60).ToList();
                optionCount = 5;
            }
            else
            {
                sentences = sentencesModel.Sentences;
                optionCount = 8;
            }

            var rnd = new Random();
            var randomNumbers = Enumerable.Range(0, sentences.Count - 1).OrderBy(x => rnd.Next()).Take(optionModel.QuizCount != 0 ? optionModel.QuizCount : QuizConstant.DEFAULT_QUIZ_COUNT).ToList();

            var blankcount = optionModel.Blankcount;
            var questions = new List<QuestionModel>();
            var correctAnswers = new String[optionModel.Blankcount];

            foreach (var number in randomNumbers)
            {
                var question = new QuestionModel();

                //Console.WriteLine(sentenceModels.Sentences[randomNumber].SentenceEng);
                var sentence = sentences[number].SentenceEng.Split(" ");
                var sentenceKor = sentences[number].SentenceKor.Split(" ");

                //TODO : OrginalText 요걸 왜 정의했지... 헷갈리니 주석처리하고 기억나면 풀자
                //question.OrginalText = GetStringFromArray(sentence);
                
                //TODO : 답을 비교할때는 완성된 문장간의 비교하기
                question.CorrectAnswer = GetStringFromArray(sentence);

                //빈칸갯수에따른 문제생성
                rnd = new Random();
                var randomIndexs = Enumerable.Range(0, sentence.Count())
                                  .OrderBy(x => rnd.Next()).Take(blankcount > sentence.Count() ? sentence.Count() : blankcount)
                                  .ToList();

                //순서대로 정답을 넣기위해 내림차순으로 소트
                randomIndexs.Sort();

                foreach (var index in randomIndexs.Select((value, i) => new { i, value }))
                {
                    correctAnswers[index.i] = sentence[index.value];
                    sentence[index.value] = "( )";
                }

                question.Question = GetStringFromArray(sentence);
                question.QuestionKor = GetStringFromArray(sentenceKor);
                question.Id = number;
                question.CorrectAnswers = correctAnswers;
                question.Options = correctAnswers.ToList();

                //TODO : 난이도에 따라 takeCount를 설정하도록 변경
                var wordRandomNumbers = GetRangeQuestion<Words>(wordModel.Words, takeCount: optionCount);

                var wrongAnswers = new List<string>();
                foreach (var wordRandomNumber in wordRandomNumbers)
                {
                    wrongAnswers.Add(wordModel.Words[wordRandomNumber].Word);
                }
                question.Options.AddRange(wrongAnswers);
                
                rnd = new Random();
                //보기를 섞기
                question.Options = question.Options.OrderBy(x => rnd.Next()).ToList();

                questions.Add(question);
                correctAnswers = new String[blankcount];

                Console.WriteLine($"문제(영): {question.Question}");
                Console.WriteLine($"문제(한): {question.QuestionKor}");
                Console.WriteLine($"정답: {GetStringFromArray(question.CorrectAnswers)}");
                Console.WriteLine($"보기: {question.ToString()}");
            }

            return questions;
        }

        public static List<QuestionModel> GenerateOrderingExtendQuestion(QuizOptionModel optionModel, SentencesModel sentencesModel, WordModel wordModel)
        {
            var sentences = new List<Sentence>();

            if (optionModel.Level == QuizLevel.L)
            {
                sentences = sentencesModel.Sentences.Where(x => x.SentenceEng.Length < 30).ToList();

            }else if(optionModel.Level == QuizLevel.M)
            {
                sentences = sentencesModel.Sentences.Where(x => x.SentenceEng.Length < 60).ToList();
            }
            else
            {
                sentences = sentencesModel.Sentences;
            }

            var rnd = new Random();
            var randomNumbers = Enumerable.Range(0, sentences.Count - 1).OrderBy(x => rnd.Next()).Take(optionModel.QuizCount != 0 ? optionModel.QuizCount : QuizConstant.DEFAULT_QUIZ_COUNT).ToList();

            var questions = new List<QuestionModel>();

            foreach (var number in randomNumbers)
            {
                var question = new QuestionModel();

                var sentence = sentences[number].SentenceEng.Split(" ");
                var sentenceKor = sentences[number].SentenceKor.Split(" ");

                Random random = new Random();
                var randomSentence = sentence.OrderBy(x => random.Next()).ToArray();
                //정답을 보기에 담기
                question.Options = randomSentence.ToList();

                question.CorrectAnswer = GetStringFromArray(sentence);

                for (var i = 0; i< sentence.Length; i++)
                {
                    sentence[i] = "( )";
                }

                question.Question = GetStringFromArray(sentence);
                question.QuestionKor = GetStringFromArray(sentenceKor);

                //오답을 보기에 담기
                var wordRandomNumbers = GetRangeQuestion<Words>(wordModel.Words, takeCount: (int)optionModel.Level);

                var wrongAnswers = new List<string>();
                foreach (var wordRandomNumber in wordRandomNumbers)
                {
                    wrongAnswers.Add(wordModel.Words[wordRandomNumber].Word);
                }

                question.Options.AddRange(wrongAnswers);
                questions.Add(question);

                Console.WriteLine($"문제(영): {question.Question}");
                Console.WriteLine($"문제(한): {question.QuestionKor}");
                Console.WriteLine($"보기: {question.ToString()}");
            }

           return questions;
        }

        //TODO : 스트링 확장 메소드로 변경
        public static string GetStringFromArray(string[] inputs)
        {
            return string.Join(" ", inputs);
        }

        public static IEnumerable<int> GetRangeQuestion<T>(List<T> models, bool isRandom = true, int takeCount = 5)
        {
            var rnd = new Random();
            var numbers = new List<int>();

            if (isRandom)
            {
                numbers = Enumerable.Range(0, models.Count).OrderBy(x => rnd.Next()).Take(takeCount).ToList();
            }
            else
            {
                numbers = Enumerable.Range(0, models.Count).ToList();
            }
            return numbers;
        }
    }
}
