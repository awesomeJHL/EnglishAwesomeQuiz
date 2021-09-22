using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class UserAnswerResultModel
    {
        public string UserName { get; set; }
        public int BankId { get; set; }
        public UserResult UserResult { get; set; }
    }
    public class UserResult
    {
        public double Score { get; set; }

        public List<QuizResultModel> QuizResult { get; set; }
    }
}
