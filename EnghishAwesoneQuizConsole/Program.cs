using EnglishAwesomeQuizShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EnghishAwesoneQuizConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*            GenerateFillBlankQuestion();
                        var userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserFillQuestion.json");
                        var userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserFillQuestionAnswer.json");
                        GenerateUserAnswerResult(userQuestionModel, userAnswerResult);
                        //Caculator();

                        GenerateOrderingQuestion();
                        userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserOrderingQuestion.json");
                        userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserOrderingQuestionAnswer.json");
                        GenerateUserAnswerResult(userQuestionModel, userAnswerResult);

                        GenerateSpellingQuestion();
                        userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserWordSpellingQuestion.json");
                        userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserWordSpellingAnswer.json");
                        GenerateUserAnswerResult(userQuestionModel, userAnswerResult);

                        GenerateWordQuestiion();*/
            //TODO 객관식문제 답, 점수체크로직 개발

            //GenerateOrderingExtendQuestion();

            //GenerateDialouge(QuestionLevel.H);

            //GenerateFillBlankQuestion(3);
            GenerateFillBlankQuestion(3);
            GenerateWordQuestiion(QuestionLanguageType.KoreanToEnglish);

            /*            GenerateWordQuestiion(QuestionLanguageType.KoreanToEnglish);

                        var userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserWordQuestion.json");
                        var userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserWordAnswer.json");
                        GenerateUserAnswerResult(userQuestionModel, userAnswerResult);*/
        }

        // 정답체크로직
        public static void GenerateUserAnswerResult(UserQuestionModel userQuestionModel, UserAnswerModel userAnswerResult)
        {
            Console.WriteLine("\n정답체크");

            var userAnswers = userAnswerResult.Question.Answers;
            var quizResult = new List<QuizResultModel>();

            //TODO : 사용자의 정답이 아닌 문제뱅크에등록된 모델을 기준으로 판단해야함( 원장이 기준이 되어야 한다는 뜻)
            foreach (var answer in userAnswers)
            {
                var question = userQuestionModel.UserQuestion.Questions.Where(x => x.Id == answer.Id).FirstOrDefault();

                Console.WriteLine($"사용자답변 : {answer.Answer} 정답: {question.CorrectAnswer}");

                quizResult.Add(
                    new QuizResultModel()
                    {
                        IsCorrect = IsCorrectAnswer(answer.Answer, question.CorrectAnswer, replaceOption: true),
                        QuestionId = question.Id
                    }
                );
            }

            var userResult = new UserAnswerResultModel()
            {
                UserResult = new UserResult()
                {
                    Score = GetScore(quizResult),
                    QuizResult = quizResult
                }
            };

            Console.WriteLine($"점수 : {userResult.UserResult.Score}");
            Console.WriteLine("------------------------\n");
        }

        private static string GetUnderBarExceptFirstCharacter(string str, BlankPosition position)
        {
            string rtn = "";
            for(int i = 0; i < str.Length-1; i++)
            {
                rtn += "_";
            }

            switch (position)
            {
                case BlankPosition.FIRST:
                    return str.First() + rtn;
                case BlankPosition.LAST:
                    return rtn + str.Last();
                default:
                    return str;
            }
        }

        public static void Caculator()
        {

            // Declare variables and then initialize to zero.
            int num1 = 0; int num2 = 0;

            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            // Ask the user to type the first number.
            Console.WriteLine("Type a number, and then press Enter");
            num1 = Convert.ToInt32(Console.ReadLine());

            // Ask the user to type the second number.
            Console.WriteLine("Type another number, and then press Enter");
            num2 = Convert.ToInt32(Console.ReadLine());

            // Ask the user to choose an option.
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            // Use a switch statement to do the math.
            switch (Console.ReadLine())
            {
                case "a":
                    Console.WriteLine($"Your result: {num1} + {num2} = " + (num1 + num2));
                    break;
                case "s":
                    Console.WriteLine($"Your result: {num1} - {num2} = " + (num1 - num2));
                    break;
                case "m":
                    Console.WriteLine($"Your result: {num1} * {num2} = " + (num1 * num2));
                    break;
                case "d":
                    Console.WriteLine($"Your result: {num1} / {num2} = " + (num1 / num2));
                    break;
            }
            // Wait for the user to respond before closing.
            Console.Write("Press any key to close the Calculator console app...");
            Console.ReadKey();

        }

        // 주어진 문장내의 순서 맞추기 문제생성
        public static void GenerateOrderingQuestion()
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"다음 문장을 순서에 맞게 답을 채우세요\n");

            var sentenceModels = GetJsonFileToModel<SentencesModel>("Sentences.json");
            var randomNumbers = GetRangeQuestion<Sentence>(sentenceModels.Sentences);

            var questions = new List<QuestionModel>();

            foreach (var randomNumber in randomNumbers)
            {
                var question = new QuestionModel();

                var sentence = sentenceModels.Sentences[randomNumber].SentenceEng.Split(" ");
                var sentenceKor = sentenceModels.Sentences[randomNumber].SentenceKor.Split(" ");

                Random random = new Random();   
                var randomSentence = sentence.OrderBy(x => random.Next()).ToArray();

                question.Question = GetStringFromArray(randomSentence) + "[" + GetStringFromArray(sentenceKor) + "]";
                question.CorrectAnswer = GetStringFromArray(sentence);

                Console.WriteLine($"문제: {question.Question}   답:{question.CorrectAnswer}");

                questions.Add(question);
            }
        }

        //정답이 맞는지 체크
        private static bool IsCorrectAnswer(string userAnswer, string answer, bool replaceOption)
        {
            string _userAnswer = userAnswer.ToLower();
            string _answer = answer.ToLower();

            //특수문자 제거
            if (replaceOption)
            {
                _userAnswer = RemoveSpecialCharaters(_userAnswer);
                _answer = RemoveSpecialCharaters(_answer);
            }

            return _userAnswer.Equals(_answer);
        }

        // 주어진 문장 순서 + 보기생성
        public static void GenerateOrderingExtendQuestion()
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"다음 문장을 순서에 맞게 답을 채우세요\n");

            var sentenceModels = GetJsonFileToModel<SentencesModel>("Sentences.json");
            var randomNumbers = GetRangeQuestion<Sentence>(sentenceModels.Sentences);

            var questions = new List<QuestionModel>();

            foreach (var randomNumber in randomNumbers)
            {
                var question = new QuestionModel();

                var sentence = sentenceModels.Sentences[randomNumber].SentenceEng.Split(" ");
                var sentenceKor = sentenceModels.Sentences[randomNumber].SentenceKor.Split(" ");

                Random random = new Random();
                var randomSentence = sentence.OrderBy(x => random.Next()).ToArray();

                question.Question = GetStringFromArray(sentenceKor);
                question.CorrectAnswer = GetStringFromArray(sentence);
                
                //정답을 보기에 담기
                question.Options = randomSentence.ToList();

                //오답을 보기에 담기 - TODO : 추후 문제난이도에 따라 동사, 명사, 형용사 등 지정해서 가져올 수 있도록 변경
                var word = GetJsonFileToModel<WordModel>("Words.json");
                var wordRandomNumbers = GetRangeQuestion<Words>(word.Words);
                var wrongAnswers = new List<string>();
                foreach (var wordRandomNumber in wordRandomNumbers)
                {
                    wrongAnswers.Add(word.Words[wordRandomNumber].Word);
                }
                
                question.Options.AddRange(wrongAnswers);

                Console.WriteLine($"문제: {question.Question}   보기:{question.ToString()}");

                questions.Add(question);
            }
        }

        // 대화형태의 문장생성
        public static void GenerateDialouge(QuizLevel level)
        {
            var sentenceModels = GetJsonFileToModel<DialougeModel>("Dialouge.json");
            var numbers = GetRangeQuestion<Sentence>(sentenceModels.Dialouge, isRandom:false);

            var questions = new List<QuestionModel>();

            foreach (var number in numbers)
            {
                var question = new QuestionModel();

                var sentence = sentenceModels.Dialouge[number].SentenceEng.Split(" ");

                Random random = new Random();
                var randomSentence = sentence.OrderBy(x => random.Next()).Take(sentence.Length - GetRemoveAnswerCount(level)).ToArray();

                question.Question = sentenceModels.Dialouge[number].SentenceKor;
                question.CorrectAnswer = sentenceModels.Dialouge[number].SentenceEng;

                //정답을 보기에 담기
                question.Options = randomSentence.ToList();

                //TODO 정답을 보기에 넣을때 난이도에 따라 일부 값은 넣지 않도록

                ////오답을 보기에 담기 - TODO : 추후 문제난이도에 따라 동사, 명사, 형용사 등 지정해서 가져올 수 있도록 변경
                //var word = GetJsonFileToModel<WordModel>("Words.json");
                //var wordRandomNumbers = GetRangeQuestion<Words>(word.Words);
                //var wrongAnswers = new List<string>();
                //foreach (var wordRandomNumber in wordRandomNumbers)
                //{
                //    wrongAnswers.Add(word.Words[wordRandomNumber].Word);
                //}
                //question.Answers.AddRange(wrongAnswers);

                Console.WriteLine($"문제: {question.Question}");
                Console.WriteLine($"보기: {question.ToString()}");
                Console.WriteLine($"답  : {question.CorrectAnswer}");

                questions.Add(question);
            }
        }

        // 레벨에 따른 보기를 제거
        public static int GetRemoveAnswerCount(QuizLevel level)
        {
            switch (level)
            {
                case QuizLevel.H:
                    return -2;
                case QuizLevel.M:
                    return -1;
                case QuizLevel.L:
                    return 0;
                default:
                    return 0;
            }
        }

        // 스펠링 맞추기 문제생성
        public static void GenerateSpellingQuestion()
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"다음 단어의 정확한 스켈링을 입력하세요\n");


            var word = GetJsonFileToModel<WordModel>("Words.json");
            var randomNumbers = GetRangeQuestion<Words>(word.Words);

            var questions = new List<QuestionModel>();

            foreach (var randomNumber in randomNumbers)
            {
                var question = new QuestionModel();

                question.Question = $"{GetUnderBarExceptFirstCharacter(word.Words[randomNumber].Word, BlankPosition.FIRST)} [{word.Words[randomNumber].KorWord}]";
                question.CorrectAnswer = word.Words[randomNumber].Word;

                Console.WriteLine($"문제: {question.Question}   답: {question.CorrectAnswer}");

                question.Id = randomNumber;

                questions.Add(question);
            }

        }

        // 빈칸채우기
        public static void GenerateFillBlankQuestion(int blankcount = 1)
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"다음 문장의 ()에 알맞는 단어를 채우세요\n");

            var rawdata = GetJsonFileToModel<SentencesModel>("Sentences.json");
            var numbers = GetRangeQuestion<Sentence>(rawdata.Sentences);

            var questions = new List<QuestionModel>();
            var correctAnswers = new String[blankcount];

            foreach (var number in numbers)
            {
                var question = new QuestionModel();

                //Console.WriteLine(sentenceModels.Sentences[randomNumber].SentenceEng);
                var sentence = rawdata.Sentences[number].SentenceEng.Split(" ");
                var sentenceKor = rawdata.Sentences[number].SentenceKor.Split(" ");

                question.OrginalText = GetStringFromArray(sentence);

                //빈칸갯수에따른 문제생성
                var rnd = new Random();
                var randomIndexs = Enumerable.Range(0, sentence.Count())
                                  .OrderBy(x => rnd.Next()).Take(blankcount > sentence.Count() ? sentence.Count() : blankcount)
                                  .ToList();

                //순서대로 정답을 넣기위해 내림차순으로 소트
                randomIndexs.Sort();

                foreach (var index in randomIndexs.Select((value, i) => new { i, value }))
                {
                    question.CorrectAnswer = sentence[index.value];
                    correctAnswers[index.i] = sentence[index.value];
                    sentence[index.value] = "( )";
                }

                question.Question = GetStringFromArray(sentenceKor) + " [ " + GetStringFromArray(sentence) + " ]";

                question.Id = number;
                question.CorrectAnswers = correctAnswers;
                question.Options = correctAnswers.ToList();

                //오답을 보기에 담기 - TODO : 추후 문제난이도에 따라 동사, 명사, 형용사 등 지정해서 가져올 수 있도록 변경
                var word = GetJsonFileToModel<WordModel>("Words.json");
                var wordRandomNumbers = GetRangeQuestion<Words>(word.Words);
                var wrongAnswers = new List<string>();
                foreach (var wordRandomNumber in wordRandomNumbers)
                {
                    wrongAnswers.Add(word.Words[wordRandomNumber].Word);
                }
                question.Options.AddRange(wrongAnswers);
                
                //보기를 섞기
                question.Options = question.Options.OrderBy(x => rnd.Next()).ToList();

                questions.Add(question);
                correctAnswers = new String[blankcount];

                Console.WriteLine($"문제: {question.Question}");
                Console.WriteLine($"정답: {GetStringFromArray(question.CorrectAnswers)}");
                Console.WriteLine($"보기: {question.ToString()}");
            }
        }

        public static double GetScore(List<QuizResultModel> quizResult)
        {
            var correctCount = quizResult.Where(x => x.IsCorrect).Count();
            return Math.Round(((double)correctCount / (double)quizResult.Count * 100),1);
        }

        public static string GetStringFromArray(string[] inputs)
        {
            return string.Join(" ", inputs);
        }

/*        public static string RemoveSpecialCharaters(string imput)
        {
            return Regex.Replace(imput, @"[^a-zA-Z0-9가-힣]", "");
        }*/

        public static string RemoveSpecialCharaters(string input)
        {
            string[] charaters = { "?", "!", "." };

            string result = input;

            foreach(var charater in charaters)
            {
                result = result.Replace(charater,"");
            }

            return result;
        }
/*
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '\u0022' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
*/
        public static void GenerateKoreanToEnglishQuestiion()
        {
        }

        public static T GetJsonFileToModel<T>(string filePath)
        {
            StreamReader r = new StreamReader(filePath);
            string jsonString = r.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        //public static IEnumerable<int> GetRangeQuestion<T>(T models) where T : List<Sentence>
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

        public static void GenerateWordQuestiion(QuestionLanguageType questionLanguageType)
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"다음 단어에 맞는 뜻을 선택하세요\n");

            var m = GetJsonFileToModel<WordModel>("Words.json");

            //Console.WriteLine(m.Words.Count);

            //for(var i=0; i<MAX_QUIZ_COUNT; i++)
            //{
            //    var random = new Random();
            //    var index = random.Next(m.Words.Count);
            //    Console.WriteLine(m.Words[index].KorWord);
            //}

            var questions = new List<QuestionModel>();

            var rnd = new Random();
            var randomNumbers = Enumerable.Range(0, 12).OrderBy(x => rnd.Next()).Take(5).ToList();
            
            //문제, 답생성
            foreach (var randomNumber in randomNumbers)
            {
                //Console.WriteLine($"{m.Words[randomNumber].Word} -: {m.Words[randomNumber].KorWord}\n");
                if(questionLanguageType == QuestionLanguageType.EnglishToKorean)
                {
                    questions.Add(new QuestionModel { Id = randomNumber, Question = m.Words[randomNumber].Word, CorrectAnswer = m.Words[randomNumber].KorWord });
                }
                else if (questionLanguageType == QuestionLanguageType.KoreanToEnglish)
                {
                    questions.Add(new QuestionModel { Id = randomNumber, Question = m.Words[randomNumber].KorWord, CorrectAnswer = m.Words[randomNumber].Word });
                }
                else
                {
                    //TODO : 구현할것
                }
            }

            int questionId = 0;
            //보기생성
            foreach (var question in questions)
            {
                var answers = new List<string>();
                rnd = new Random();
                randomNumbers = Enumerable.Range(0, 12).OrderBy(x => rnd.Next()).Take(5 - 1).ToList();

                //정답을 보기에 넣기
                answers.Add(question.CorrectAnswer);

                //오답을 보기에 넣기
                foreach (var randomNumber in randomNumbers)
                {
                    if (questionLanguageType == QuestionLanguageType.EnglishToKorean)
                    {
                        answers.Add(m.Words[randomNumber].KorWord);
                    }
                    else if (questionLanguageType == QuestionLanguageType.KoreanToEnglish)
                    {
                        answers.Add(m.Words[randomNumber].Word);
                    }
                    else
                    {
                        //TODO : 구현할것
                    }
                }

                question.Options = answers.OrderBy( x => rnd.Next()).ToList();
                question.Id = questionId++;
            }

            //출력해보기
            foreach (var question in questions)
            {
                Console.WriteLine($"아이디 : {question.Id} - 문제 : {question.Question} \n 답 : {question.CorrectAnswer}");
                Console.WriteLine($"보기 : {question.Options.Aggregate((x, y) => x + ", " + y)}");
            }
/*
            //문제 은행만들기
            var bank = new List<QuestionBankModel>();
            bank.Add(new QuestionBankModel()
            {
                BankId = 1,
                Bank = questions
            });

            //TODO: 답변하기 로직

            //TODO: 사용자 답변
            var r = new StreamReader("UserWordAnswer.json");
            string answerJsonString = r.ReadToEnd();
            var userAnswerResult = JsonConvert.DeserializeObject<UserAnswerModel>(answerJsonString);

            Console.WriteLine($"사용자명 : {userAnswerResult.UserName}");

            //TODO: 문제 아이디로 답안지를 가져온다
            var myQuestionBank = bank.Where(x => x.BankId == userAnswerResult.Question.Id).FirstOrDefault();

            //TODO: 가져온 답안지와 답변을 비교한다
            var userAnswers = userAnswerResult.Question.Answers;

            foreach (var answer in userAnswers)
            {
                var compareQuestion = myQuestionBank.Bank.Where(x => x.Id == answer.Id).FirstOrDefault();
                //myQuestionBank.Bank.Any(x => x.Id == answer.Id);
                Console.WriteLine($"사용자답변 : {answer.Answer} 정답: {compareQuestion.CorrectAnswer}");
                //TODO 점수계산
            }

            Console.ReadKey();
*/
        }

        public static void CheckOrderingQuestion()
        {
            var userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserOrderingQuestion.json");
            var userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserOrderingQuestionAnswer.json");

            var userAnswers = userAnswerResult.Question.Answers;
            var quizResult = new List<QuizResultModel>();

            //TODO : 사용자의 정답이 아닌 문제뱅크에등록된 모델을 기준으로 판단해야함( 원장이 기준이 되어야 한다는 뜻)
            foreach (var answer in userAnswers)
            {
                var question = userQuestionModel.UserQuestion.Questions.Where(x => x.Id == answer.Id).FirstOrDefault();
                Console.WriteLine($"사용자답변 : {answer.Answer} 정답: {question.CorrectAnswer}");

                quizResult.Add(
                    new QuizResultModel()
                    {
                        IsCorrect = IsCorrectAnswer(answer.Answer, question.CorrectAnswer, replaceOption: true),
                        QuestionId = question.Id
                    }
                );
            }

            var userResult = new UserAnswerResultModel()
            {
                UserResult = new UserResult()
                {
                    Score = GetScore(quizResult),
                    QuizResult = quizResult
                }
            };

            Console.WriteLine($"점수 : {userResult.UserResult.Score}");
        }

        public static void CheckFillQuestion()
        {
            var userQuestionModel = GetJsonFileToModel<UserQuestionModel>("UserFillQuestion.json");
            var userAnswerResult = GetJsonFileToModel<UserAnswerModel>("UserFillQuestionAnswer.json");

            var userAnswers = userAnswerResult.Question.Answers;
            var quizResult = new List<QuizResultModel>();

            //TODO : 사용자의 정답이 아닌 문제뱅크에등록된 모델을 기준으로 판단해야함( 원장이 기준이 되어야 한다는 뜻)
            foreach (var answer in userAnswers)
            {
                var question = userQuestionModel.UserQuestion.Questions.Where(x => x.Id == answer.Id).FirstOrDefault();

                Console.WriteLine($"사용자답변 : {answer.Answer} 정답: {question.CorrectAnswer}");

                quizResult.Add(
                    new QuizResultModel()
                    {
                        IsCorrect = IsCorrectAnswer(answer.Answer, question.CorrectAnswer, replaceOption: false),
                        QuestionId = question.Id
                    }
                );
            }

            var userResult = new UserAnswerResultModel()
            {
                UserResult = new UserResult()
                {
                    Score = GetScore(quizResult),
                    QuizResult = quizResult
                }
            };

            Console.WriteLine($"점수 : {userResult.UserResult.Score}");
        }
    }
}