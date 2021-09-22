using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class UserAnswerModel
    {
        public string UserName { get; set; }
        public Question Question { get; set; }
    }
    public class Question
    {
        public int Id { get; set; }
        public List<Answers> Answers { get; set; }
    }
    public class Answers
    {
        public int Id { get; set; }
        public string Answer { get; set; }
    }

}
