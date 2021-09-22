using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        
        //문제
        public string Question { get; set; }
        
        //보기
        public List<string> Answers { get; set; }
        
        //정답
        public string CorrectAnswer { get; set; }
        
        //정답(복수선택)
        public string[] CorrectAnswers { get; set; }

        //문제유형 - 주/객/다지
        public string QuestionType { get; set; }

        public string OrginalText { get; set; }

        public override string ToString()
        {
            return string.Join(",", Answers);
        }
    }
}
