using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class UserQuestionModel
    {
        public string UserName { get; set; }
        public UserQuestion UserQuestion { get; set; }
    }
    public class UserQuestion
    {
        public int Id { get; set; }
        public List<QuestionModel> Questions { get; set; }
    }
}
